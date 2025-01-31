using ItaTask.Application.DTOs.Common;
using ItaTask.Application.DTOs.Persons;
using ItaTask.Application.Features.Persons.Commands.AddRelatedPerson;
using ItaTask.Application.Features.Persons.Commands.CreatePerson;
using ItaTask.Application.Features.Persons.Commands.DeletePerson;
using ItaTask.Application.Features.Persons.Commands.RemoveRelatedPerson;
using ItaTask.Application.Features.Persons.Commands.UpdatePerson;
using ItaTask.Application.Features.Persons.Commands.UpdatePersonImage;
using ItaTask.Application.Features.Persons.Queries.GetPersonDetails;
using ItaTask.Application.Features.Persons.Queries.GetPersonsList;
using ItaTask.Application.Features.Persons.Queries.GetRelatedPersonsReport;
using ItaTask.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItaTask.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreatePersonCommand command)
    {
        var personId = await mediator.Send(command);
        return Ok(personId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<int>> Update(int id, [FromBody] UpdatePersonCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { Message = "Id from route does not match Id from body" });

        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id}/image")]
    public async Task<ActionResult<string>> UploadImage(int id, IFormFile image)
    {
        var command = new UpdatePersonImageCommand
        {
            Id = id,
            Image = image
        };

        var imagePath = await mediator.Send(command);
        return Ok(new { ImagePath = imagePath });
    }

    [HttpPost("{id}/related-persons")]
    public async Task<IActionResult> AddRelatedPerson(int id, [FromBody] AddRelatedPersonCommand command)
    {
        if (id != command.PersonId)
            return BadRequest(new { Message = "Id from route does not match PersonId from body" });

        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}/related-persons/{relatedPersonId}")]
    public async Task<IActionResult> RemoveRelatedPerson(int id, int relatedPersonId)
    {
        var command = new RemoveRelatedPersonCommand
        {
            PersonId = id,
            RelatedPersonId = relatedPersonId
        };

        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await mediator.Send(new DeletePersonCommand(id));
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PersonDetailsDto>> GetDetails(int id)
    {
        var result = await mediator.Send(new GetPersonDetailsQuery(id));
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<PersonDto>>> GetList([FromQuery] GetPersonsListQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}/related-persons/report")]
    public async Task<ActionResult<Dictionary<RelationType, int>>> GetRelatedPersonsReport(int id)
    {
        var result = await mediator.Send(new GetRelatedPersonsReportQuery(id));
        return Ok(result);
    }
}