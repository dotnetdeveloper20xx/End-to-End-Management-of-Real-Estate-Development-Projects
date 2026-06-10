using BuildEstate.Application.Land.Repositories;
using BuildEstate.Domain.Land;
using BuildEstate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BuildEstate.Infrastructure.Repositories.Land;

public class LandOpportunityRepository : ILandOpportunityRepository
{
    private readonly ApplicationDbContext _dbContext;

    public LandOpportunityRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        LandOpportunity landOpportunity,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.LandOpportunities.AddAsync(
            landOpportunity,
            cancellationToken);
    }

    public async Task<LandOpportunity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.LandOpportunities
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<LandOpportunity>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.LandOpportunities
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }
}