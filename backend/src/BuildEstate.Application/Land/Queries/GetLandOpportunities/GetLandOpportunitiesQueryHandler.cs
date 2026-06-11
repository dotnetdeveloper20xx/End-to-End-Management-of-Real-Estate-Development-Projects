using BuildEstate.Application.Land.DTOs;
using BuildEstate.Application.Land.Repositories;
using BuildEstate.Shared.Pagination;
using MediatR;

namespace BuildEstate.Application.Land.Queries.GetLandOpportunities;

public class GetLandOpportunitiesQueryHandler
    : IRequestHandler<GetLandOpportunitiesQuery, PagedResult<LandOpportunityDto>>
{
    private readonly ILandOpportunityRepository _repository;

    public GetLandOpportunitiesQueryHandler(
        ILandOpportunityRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<LandOpportunityDto>> Handle(
        GetLandOpportunitiesQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

        var pagedResult = await _repository.GetPagedAsync(
            pageNumber,
            pageSize,
            null,
            cancellationToken);

        return new PagedResult<LandOpportunityDto>
        {
            Items = pagedResult.Items
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
                .ToList(),

            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalCount = pagedResult.TotalCount
        };
    }
}