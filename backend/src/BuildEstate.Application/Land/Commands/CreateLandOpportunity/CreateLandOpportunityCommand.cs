using BuildEstate.Application.Land.DTOs;
using MediatR;

namespace BuildEstate.Application.Land.Commands.CreateLandOpportunity;

public class CreateLandOpportunityCommand : IRequest<LandOpportunityDto>
{
    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public decimal LandSizeAcres { get; set; }

    public decimal AskingPrice { get; set; }

    public string Source { get; set; } = string.Empty;

    public string? AgentName { get; set; }

    public DateTime? ExpectedAcquisitionDate { get; set; }

    public string? Notes { get; set; }
}