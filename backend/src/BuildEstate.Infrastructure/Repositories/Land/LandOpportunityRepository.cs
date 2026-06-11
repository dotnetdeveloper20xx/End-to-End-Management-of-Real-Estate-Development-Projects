using BuildEstate.Application.Land.Repositories;
using BuildEstate.Domain.Land;
using BuildEstate.Infrastructure.Persistence;

namespace BuildEstate.Infrastructure.Repositories.Land;

public class LandOpportunityRepository
    : GenericRepository<LandOpportunity>, ILandOpportunityRepository
{
    public LandOpportunityRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}