using ItaTask.Application.DTOs.Persons;
using ItaTask.Domain.Entities;
using ItaTask.Domain.Exceptions;
using ItaTask.Domain.Interfaces.Repositories; 
using MediatR;

namespace ItaTask.Application.Features.Persons.Queries.GetPersonDetails;

public record GetPersonDetailsQuery(int Id) : IRequest<PersonDetailsDto>;

public class GetPersonDetailsQueryHandler(
    IPersonRepository personRepository)
    : IRequestHandler<GetPersonDetailsQuery, PersonDetailsDto>
{
    public async Task<PersonDetailsDto> Handle(GetPersonDetailsQuery request, CancellationToken cancellationToken)
    {
        var person = await personRepository.GetWithDetailsAsync(request.Id, cancellationToken);

        if (person == null)
            throw new NotFoundException(nameof(Person), request.Id);

        return new PersonDetailsDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Gender = person.Gender,
            PersonalNumber = person.PersonalNumber,
            BirthDate = person.BirthDate,
            City = new CityDto(person.City.Id, person.City.Name),
            ImagePath = person.ImagePath,
            PhoneNumbers = person.PhoneNumbers
                .Select(p => new PhoneNumberDto { Type = p.Type, Number = p.Number })
                .ToList(),
            RelatedPersons = person.RelatedPersons
                .Select(r => new RelatedPersonDto
                {
                    RelationType = r.RelationType,
                    RelatedPersonId = r.RelatedPersonId,
                    FirstName = r.RelatedTo!.FirstName,
                    LastName = r.RelatedTo.LastName
                }).ToList()
        };
    }
}