using Planne.Core.Entities;

namespace Planne.Client.Domain.Services.Interfaces;

public interface IColumnService
{
    Task CreateAsync(string boardName, Guid userId);
    Task DeleteAsync(Guid boardId);
    Task UpdateAsync(Guid boardId);
    Task<Column> GetByIdAsync(Guid boardId);
    Task<IEnumerable<Column>> GetAllAsync();
}