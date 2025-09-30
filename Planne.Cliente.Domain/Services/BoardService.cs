using Planne.Client.Domain.Services.Interfaces;
using Planne.Core.Entities;

namespace Planne.Client.Domain.Services;

public class BoardService : IBoardService
{
    public Task CreateAsync(string boardName, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid boardId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Board>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Board> GetByIdAsync(Guid boardId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Guid boardId)
    {
        throw new NotImplementedException();
    }
}
