using Planne.Client.Domain.Services.Interfaces;
using Planne.Core.Entities;

namespace Planne.Client.Domain.Services;

public class ColumnService : IColumnService
{
    public Task CreateAsync(string boardName, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid boardId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Column>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Column> GetByIdAsync(Guid boardId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Guid boardId)
    {
        throw new NotImplementedException();
    }
}
