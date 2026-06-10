using BuildEstate.Application.Land.DTOs;
using BuildEstate.Application.Land.Repositories;
using MediatR;

namespace BuildEstate.Application.Land.Queries.GetLandOpportunities;

public class GetLandOpportunitiesQueryHandler
    : IRequestHandler<GetLandOpportunitiesQuery, IReadOnlyList<LandOpportunityDto>>
{
    private readonly ILandOpportunityRepository _repository;

    public GetLandOpportunitiesQueryHandler(
        ILandOpportunityRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<LandOpportunityDto>> Handle(
        GetLandOpportunitiesQuery request,
        CancellationToken cancellationToken)
    {
        var opportunities = await _repository.GetAllAsync(cancellationToken);

        return opportunities
            .Select(x => new LandOpportunityDto
            {
                Id = x.Id,
                Name = x.Name,
                Location = x.Location,
                LandSizeAcres = x.LandSizeAcres,
                AskingPrice = x.AskingPrice,
                Source = x.Source,
                AgentName = x.AgentName,
                ExpectedAcquisitionDate = x.ExpectedAcquisitionDate,
                Status = x.Status,
                Notes = x.Notes
            })
            .ToList();
    }
}