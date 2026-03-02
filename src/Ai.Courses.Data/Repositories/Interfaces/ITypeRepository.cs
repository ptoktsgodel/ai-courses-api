using Ai.Courses.Data.Entities;

namespace Ai.Courses.Data.Repositories.Interfaces;

public interface ITypeRepository
{
    Task<IEnumerable<TypeEntity>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<TypeEntity?> FindByNameAsync(Guid userId, string name, CancellationToken cancellationToken);
    Task AddAsync(TypeEntity type, CancellationToken cancellationToken);
}
