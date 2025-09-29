using Planne.Core.Entities.Base;
using Planne.Core.Enums;

namespace Planne.Core.Entities;

public class TaskCard : BaseEntity
{
    public string? Description { get; set; }
    public DateTime? InitialDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? TaskDate { get; set; }
    public PriorityEnum Priority { get; set; } = PriorityEnum.Normal;

    public int ColumnId { get; set; }
    public Column Column { get; set; } = null!;
}