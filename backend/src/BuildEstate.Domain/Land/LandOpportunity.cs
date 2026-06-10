using BuildEstate.Domain.Common;
using BuildEstate.Domain.Land.Enums;

namespace BuildEstate.Domain.Land;

public class LandOpportunity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public decimal LandSizeAcres { get; set; }

    public decimal AskingPrice { get; set; }

    public string Source { get; set; } = string.Empty;

    public string? AgentName { get; set; }

    public DateTime? ExpectedAcquisitionDate { get; set; }

    public LandOpportunityStatus Status { get; set; } =
        LandOpportunityStatus.Identified;

    public string? Notes { get; set; }
}