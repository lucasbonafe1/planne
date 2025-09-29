using System.ComponentModel;

namespace Planne.Core.Enums;

public enum PriorityEnum
{
    [Description("Baixa")]
    Low,
    [Description("Normal")]
    Normal,
    [Description("Alta")]
    High,
    [Description("Crítica")]
    Critical
}
