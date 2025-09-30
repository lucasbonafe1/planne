using Planne.Client.Domain.Services.Interfaces;
using Planne.Core.Entities;

namespace Planne.Client.Domain.Services;

public class TaskCardService : ITaskCardService
{
    public Task CreateAsync(string boardName, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid boardId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskCard>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TaskCard> GetByIdAsync(Guid boardId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Guid boardId)
    {
        throw new NotImplementedException();
    }
}
