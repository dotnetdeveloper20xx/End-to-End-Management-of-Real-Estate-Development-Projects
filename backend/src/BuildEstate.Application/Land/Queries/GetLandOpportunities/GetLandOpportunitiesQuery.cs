using BuildEstate.Application.Land.DTOs;
using BuildEstate.Shared.Pagination;
using MediatR;

namespace BuildEstate.Application.Land.Queries.GetLandOpportunities;

public class GetLandOpportunitiesQuery : IRequest<PagedResult<LandOpportunityDto>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}