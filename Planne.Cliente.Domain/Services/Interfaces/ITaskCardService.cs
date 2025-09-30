using Planne.Core.Entities;

namespace Planne.Client.Domain.Services.Interfaces;

public interface ITaskCardService
{
    Task CreateAsync(string boardName, Guid userId);
    Task DeleteAsync(Guid boardId);
    Task UpdateAsync(Guid boardId);
    Task<TaskCard> GetByIdAsync(Guid boardId);
    Task<IEnumerable<TaskCard>> GetAllAsync();
}
