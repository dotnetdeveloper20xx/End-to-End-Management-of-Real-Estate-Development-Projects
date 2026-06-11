using BuildEstate.Application.Land.DTOs;
using MediatR;

namespace BuildEstate.Application.Land.Commands.UpdateLandOpportunity;

public class UpdateLandOpportunityCommand : IRequest<LandOpportunityDto>
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public decimal LandSizeAcres { get; set; }

    public decimal AskingPrice { get; set; }

    public string Source { get; set; } = string.Empty;

    public string? AgentName { get; set; }

    public DateTime? ExpectedAcquisitionDate { get; set; }

    public string? Notes { get; set; }
}