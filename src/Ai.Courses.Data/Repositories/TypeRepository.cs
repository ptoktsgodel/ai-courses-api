using Ai.Courses.Data.Contexts;
using Ai.Courses.Data.Entities;
using Microsoft.EntityFrameworkCore;

using Ai.Courses.Data.Repositories.Interfaces;

namespace Ai.Courses.Data.Repositories;

public class TypeRepository(PaymentDbContext db) : ITypeRepository
{
    public async Task<TypeEntity?> FindByNameAsync(Guid userId, string name, CancellationToken cancellationToken)
        => await db.Types
            .FirstOrDefaultAsync(t => t.UserId == userId && t.Name == name, cancellationToken);

    public async Task AddAsync(TypeEntity type, CancellationToken cancellationToken)
    {
        await db.Types.AddAsync(type, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }
}
