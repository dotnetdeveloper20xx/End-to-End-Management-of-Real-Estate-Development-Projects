using FluentValidation;

namespace BuildEstate.Application.Land.Commands.UpdateLandOpportunity;

public class UpdateLandOpportunityCommandValidator
    : AbstractValidator<UpdateLandOpportunityCommand>
{
    public UpdateLandOpportunityCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Location)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(x => x.LandSizeAcres)
            .GreaterThan(0);

        RuleFor(x => x.AskingPrice)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Source)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.AgentName)
            .MaximumLength(200);

        RuleFor(x => x.Notes)
            .MaximumLength(2000);
    }
}