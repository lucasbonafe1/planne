using Planne.Core.Entities.Base;

namespace Planne.Core.Entities;

public class Column : BaseEntity
{
    public int BoardId { get; set; }
    public Board Board { get; set; } = null!;

    public ICollection<TaskCard> TaskCards { get; set; } = new List<TaskCard>();
}