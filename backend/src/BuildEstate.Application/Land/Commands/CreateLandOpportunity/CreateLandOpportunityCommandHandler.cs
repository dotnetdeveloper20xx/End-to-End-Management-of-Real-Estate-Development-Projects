using BuildEstate.Application.Abstractions;
using BuildEstate.Application.Land.DTOs;
using BuildEstate.Application.Land.Repositories;
using BuildEstate.Domain.Land;
using BuildEstate.Domain.Land.Enums;
using MediatR;

namespace BuildEstate.Application.Land.Commands.CreateLandOpportunity;

public class CreateLandOpportunityCommandHandler
    : IRequestHandler<CreateLandOpportunityCommand, LandOpportunityDto>
{
    private readonly ILandOpportunityRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;

    public CreateLandOpportunityCommandHandler(
        ILandOpportunityRepository repository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
    }

    public async Task<LandOpportunityDto> Handle(
        CreateLandOpportunityCommand request,
        CancellationToken cancellationToken)
    {
        var entity = new LandOpportunity
        {
            Name = request.Name,
            Location = request.Location,
            LandSizeAcres = request.LandSizeAcres,
            AskingPrice = request.AskingPrice,
            Source = request.Source,
            AgentName = request.AgentName,
            ExpectedAcquisitionDate = request.ExpectedAcquisitionDate,
            Notes = request.Notes,
            Status = LandOpportunityStatus.Identified,
            CreatedAtUtc = _dateTimeProvider.UtcNow,
            CreatedBy = _currentUserService.UserId
        };

        await _repository.AddAsync(entity, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LandOpportunityDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Location = entity.Location,
            LandSizeAcres = entity.LandSizeAcres,
            AskingPrice = entity.AskingPrice,
            Source = entity.Source,
            AgentName = entity.AgentName,
            ExpectedAcquisitionDate = entity.ExpectedAcquisitionDate,
            Status = entity.Status,
            Notes = entity.Notes
        };
    }
}