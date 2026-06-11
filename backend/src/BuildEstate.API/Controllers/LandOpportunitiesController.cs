using BuildEstate.Application.Land.Commands.CreateLandOpportunity;
using BuildEstate.Application.Land.Commands.UpdateLandOpportunity;
using BuildEstate.Application.Land.Queries.GetLandOpportunities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BuildEstate.API.Controllers;

[ApiController]
[Route("api/v1/land-opportunities")]
public class LandOpportunitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public LandOpportunitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetLandOpportunitiesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateLandOpportunityCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetAll),
            new { id = result.Id },
            result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
    Guid id,
    UpdateLandOpportunityCommand command,
    CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Route id does not match request body id.");
        }

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}