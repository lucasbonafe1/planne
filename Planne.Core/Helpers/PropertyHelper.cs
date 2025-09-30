using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Planne.Core.Helpers;

public static class PropertyHelper
{
    public static void SetDateTimeMinValueToNull(object? dto)
    {
        SetDateTimeMinValueToNull(dto, null);
    }

    private static void SetDateTimeMinValueToNull(object? dto, object? parent)
    {
        if (dto == null) return;

        var properties = dto.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(DateTime?))
            {
                var currentValue = (DateTime?)property.GetValue(dto);

                if (currentValue == DateTime.MinValue)
                    property.SetValue(dto, null);
            }
            else if (property.PropertyType.IsClass && property.PropertyType != typeof(String))
            {
                if (property.IsParentReference(dto, parent))
                    continue;

                var currentValue = property.GetValue(dto);
                SetDateTimeMinValueToNull(currentValue, dto);
            }
        }
    }
    private static bool IsParentReference(this PropertyInfo property, object? obj, object? parent)
    {
        if (parent == null) return false;

        var value = property.GetValue(obj);
        if (value is null) return false;

        if (value is IEnumerable enumValue)
        {
            var array = enumValue.Cast<object?>();

            return array.Contains(parent);
        }

        return parent == value && value.GetHashCode() == parent.GetHashCode();
    }

    public static bool Compare<T>(this T? objA, T? objB, params Expression<Func<T, object>>[] propertiesIgnore) where T : class
    {
        return CompararObj(objA, objB, propertiesIgnore);
    }

    public static void ReplaceObject<T>(this T destObj, T srcObj, Expression<Func<T, object>>[]? propertiesIgnore = null, Expression<Func<T, object>>[]? childUpdate = null) where T : class
    {
        propertiesIgnore ??= Array.Empty<Expression<Func<T, object>>>();// versão acima do .NET 8 usar []
        childUpdate ??= Array.Empty<Expression<Func<T, object>>>();// versão acima do .NET 8 usar []

        ReplaceValues(destObj, srcObj, propertiesIgnore, childUpdate);
    }

    private static void ReplaceValues<T>(object? objDest, object? objSrc, Expression<Func<T, object>>[] propertiesIgnore, Expression<Func<T, object>>[] childUpdate)
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        if (objDest == null && objSrc == null)
            return;

        var properties = objDest?.GetType().GetProperties() ?? objSrc!.GetType().GetProperties();

        foreach (var property in properties)
        {
            var propertyShouldBeIgnored = PropertyShouldentBeReplaced(property, propertiesIgnore);
            if (propertyShouldBeIgnored)
                continue;

            if (objDest is not IEnumerable && property.PropertyType != typeof(string) && property.PropertyType.IsClass)
            {
                var childShouldUpadated = childUpdate.Any(pi => MesmaPropriedadeDaExpressao(property, pi.Body));
                if (childShouldUpadated)
                {
                    var valueDest = property.GetValue(objDest);
                    var valueSrc = property.GetValue(objSrc);

                    if (valueSrc == null)
                        property.SetValue(objDest, null, null);
                    else if (valueDest == null)
                        property.SetValue(objDest, valueSrc, null);
                    else
                        ReplaceValues(valueDest, valueSrc, propertiesIgnore, childUpdate);
                }
            }
            else
            {
                var valueSrc = property.GetValue(objSrc);
                property.SetValue(objDest, valueSrc, null);
            }
        }
    }

    private static bool CompararObj<T>(object? objA, object? objB, Expression<Func<T, object>>[] propertiesIgnore, object? parentA = null, object? parentB = null)
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        if (objA == null && objB == null)
            return true;

        if (objA != null && objB == null || objA == null && objB != null)
            return false;

        var type = objA!.GetType();


        if (type != typeof(string) && objA is IEnumerable enumA && objB is IEnumerable enumB)
        {
            var arrayA = enumA.Cast<object?>();
            var arrayB = enumB.Cast<object?>();

            if (arrayA!.Count() != arrayB!.Count()) return false;

            return arrayA!.All(a => arrayB!.Any(b => CompararObj(a, b, propertiesIgnore, parentA, parentB)));
        }
        else if (type.IsClass && type != typeof(string))
        {
            var properties = objA!.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (PropriedadeNaoDeveSerComparada(property, propertiesIgnore))
                    continue;

                var valueA = property.GetValue(objA);
                var valueB = property.GetValue(objB);

                if (property.IsParentReference(objA, parentA))
                    continue;

                if (property.IsParentReference(objB, parentB))
                    continue;

                if (!CompararObj(valueA, valueB, propertiesIgnore, objA, objB))
                    return false;
            }
        }
        else
        {
            return objA?.ToString()?.Trim() == objB?.ToString()?.Trim();
        }

        return true;
    }



    private static bool PropriedadeNaoDeveSerComparada<T>(PropertyInfo property, Expression<Func<T, object>>[] propertiesIgnore)
    {
        if (!property.CanWrite || property.GetCustomAttributes<IgnoreCompareAttribute>().Any())
            return true;

        var propriedadeNaoDeveSerComparada = propertiesIgnore.Any(pi => MesmaPropriedadeDaExpressao(property, pi.Body));

        if (propriedadeNaoDeveSerComparada)
            return true;

        return false;
    }

    private static bool PropertyShouldentBeReplaced<T>(PropertyInfo property, Expression<Func<T, object>>[] propertiesIgnore)
    {
        if (!property.CanWrite || property.GetCustomAttributes<NeverBeReplacedAttribute>().Any())
            return true;

        var propriedadeNaoDeveSerComparada = propertiesIgnore.Any(pi => MesmaPropriedadeDaExpressao(property, pi.Body));

        if (propriedadeNaoDeveSerComparada)
            return true;

        return false;
    }

    private static bool MesmaPropriedadeDaExpressao(PropertyInfo property, Expression expressionProperty)
    {
        if (expressionProperty is UnaryExpression unaryExpression)
            return MesmaPropriedadeDaExpressao(property, unaryExpression.Operand);

        else if (expressionProperty is MemberExpression member)
            return member.Member.Name == property.Name && property.DeclaringType == member.Member.DeclaringType;

        return false;
    }

    public static TResult ForceCast<TResult>(this object src) where TResult : class
    {
        var result = Activator.CreateInstance<TResult>();

        var srcProperties = src.GetType().GetProperties();
        var resultProperties = result.GetType().GetProperties();

        foreach (var prop in resultProperties)
        {
            var propSrc = srcProperties.FirstOrDefault(f => f.Name == prop.Name && f.PropertyType == prop.PropertyType);

            if (propSrc == null) continue;

            var srcValue = propSrc.GetValue(src, null);
            prop.SetValue(result, srcValue, null);
        }

        return result;
    }
}


[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class IgnoreCompareAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class NeverBeReplacedAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _booleanPropertyName;

    public RequiredIfAttribute(string booleanPropertyName)
    {
        _booleanPropertyName = booleanPropertyName;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(_booleanPropertyName);

        if (property == null)
            return new ValidationResult($"Propriedade '{_booleanPropertyName}' não encontrada.");

        var booleanValue = (bool?)property.GetValue(validationContext.ObjectInstance);


        if (booleanValue == true)
        {
            return value == null ? new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} é obrigatório.")
                : new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} é inválido.");
        }

        return ValidationResult.Success!;
    }
}
