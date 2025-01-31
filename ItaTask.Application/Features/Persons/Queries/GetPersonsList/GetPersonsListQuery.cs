using ItaTask.Application.DTOs.Common;
using ItaTask.Application.DTOs.Persons;
using ItaTask.Domain.Interfaces.Repositories;
using MediatR;

namespace ItaTask.Application.Features.Persons.Queries.GetPersonsList;

public record GetPersonsListQuery : IRequest<PagedList<PersonDto>>
{
    public string? SearchTerm { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetPersonsListQueryHandler(
    IPersonRepository repository)
    : IRequestHandler<GetPersonsListQuery, PagedList<PersonDto>>
{
    public async Task<PagedList<PersonDto>> Handle(GetPersonsListQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await repository.GetPagedAsync(
            request.SearchTerm,
            request.Page,
            request.PageSize,
            cancellationToken);

        var personDtos = new List<PersonDto>();

        foreach (var person in items)
        {
            personDtos.Add(new PersonDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Gender = person.Gender,
                PersonalNumber = person.PersonalNumber,
                BirthDate = person.BirthDate,
                City = new CityDto(person.City.Id, person.City.Name)
            });
        }

        return new PagedList<PersonDto>
        {
            Items = personDtos,
            TotalCount = totalCount,
            PageSize = request.PageSize,
            CurrentPage = request.Page
        };
    }
}