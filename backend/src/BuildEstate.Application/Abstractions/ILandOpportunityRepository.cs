using BuildEstate.Domain.Land;

namespace BuildEstate.Application.Land.Repositories;

public interface ILandOpportunityRepository
{
    Task AddAsync(
        LandOpportunity landOpportunity,
        CancellationToken cancellationToken = default);

    Task<LandOpportunity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LandOpportunity>> GetAllAsync(
        CancellationToken cancellationToken = default);
}