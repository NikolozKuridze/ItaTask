using ItaTask.Application.DTOs.Persons;
using ItaTask.Domain.Interfaces.Repositories;
using MediatR;

namespace ItaTask.Application.Features.Cities.Queries.GetAllCities;

public record GetAllCitiesQuery : IRequest<List<CityDto>>;

public class GetAllCitiesQueryHandler(ICityRepository repository) : IRequestHandler<GetAllCitiesQuery, List<CityDto>>
{
    public async Task<List<CityDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        var cities = await repository.GetAllCitiesAsync(cancellationToken); 
        return cities.Select(city => new CityDto(city.Id, city.Name)).ToList();
    }
}