using Planne.Core.Entities;

namespace Planne.Client.Domain.Services.Interfaces;

public interface IBoardService
{
    Task CreateAsync(string boardName, Guid userId);
    Task DeleteAsync(Guid boardId);
    Task UpdateAsync(Guid boardId);
    Task<Board> GetByIdAsync(Guid boardId);
    Task<IEnumerable<Board>> GetAllAsync();
}
