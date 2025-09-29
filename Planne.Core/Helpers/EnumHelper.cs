using System.ComponentModel;
using System.Reflection;

namespace Planne.Core.Helpers;

public static class EnumHelper
{
    public static string GetEnumDescription<T>(this T enumValue) where T : Enum
    {
        var enumFieldName = enumValue.ToString();
        var field = typeof(T).GetField(enumFieldName);
        if (field is null)
        {
            return "No description";
        }

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute is null ? "No description" : attribute.Description;
    }
}
