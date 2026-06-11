using BuildEstate.Application.Abstractions;
using BuildEstate.Application.Land.DTOs;
using BuildEstate.Application.Land.Repositories;
using BuildEstate.Shared.Exceptions;
using MediatR;

namespace BuildEstate.Application.Land.Commands.UpdateLandOpportunity;

public class UpdateLandOpportunityCommandHandler
    : IRequestHandler<UpdateLandOpportunityCommand, LandOpportunityDto>
{
    private readonly ILandOpportunityRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;

    public UpdateLandOpportunityCommandHandler(
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
        UpdateLandOpportunityCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(
                $"Land opportunity with id '{request.Id}' was not found.");
        }

        entity.Name = request.Name;
        entity.Location = request.Location;
        entity.LandSizeAcres = request.LandSizeAcres;
        entity.AskingPrice = request.AskingPrice;
        entity.Source = request.Source;
        entity.AgentName = request.AgentName;
        entity.ExpectedAcquisitionDate = request.ExpectedAcquisitionDate;
        entity.Notes = request.Notes;
        entity.UpdatedAtUtc = _dateTimeProvider.UtcNow;
        entity.UpdatedBy = _currentUserService.UserId;

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