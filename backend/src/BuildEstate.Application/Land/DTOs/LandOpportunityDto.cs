using BuildEstate.Domain.Land.Enums;

namespace BuildEstate.Application.Land.DTOs;

public class LandOpportunityDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public decimal LandSizeAcres { get; set; }

    public decimal AskingPrice { get; set; }

    public string Source { get; set; } = string.Empty;

    public string? AgentName { get; set; }

    public DateTime? ExpectedAcquisitionDate { get; set; }

    public LandOpportunityStatus Status { get; set; }

    public string? Notes { get; set; }
}