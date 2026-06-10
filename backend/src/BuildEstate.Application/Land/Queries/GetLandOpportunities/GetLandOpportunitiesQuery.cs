using BuildEstate.Application.Land.DTOs;
using MediatR;

namespace BuildEstate.Application.Land.Queries.GetLandOpportunities;

public class GetLandOpportunitiesQuery
    : IRequest<IReadOnlyList<LandOpportunityDto>>
{
}