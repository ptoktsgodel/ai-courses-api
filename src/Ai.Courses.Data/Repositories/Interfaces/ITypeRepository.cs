using Ai.Courses.Data.Entities;

namespace Ai.Courses.Data.Repositories.Interfaces;

public interface ITypeRepository
{
    Task<TypeEntity?> FindByNameAsync(Guid userId, string name, CancellationToken cancellationToken);
    Task AddAsync(TypeEntity type, CancellationToken cancellationToken);
}
