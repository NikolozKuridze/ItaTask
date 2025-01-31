using ItaTask.Application.DTOs.Persons;
using ItaTask.Application.Features.Cities.Queries.GetAllCities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ItaTask.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CityDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllCitiesQuery());
        return Ok(result);
    }
}