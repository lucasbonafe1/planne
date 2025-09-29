using Planne.Core.Entities.Base;

namespace Planne.Core.Entities;

public class Board : BaseEntity
{
    public ICollection<Column> Columns { get; set; } = new List<Column>();
}
