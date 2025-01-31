using ItaTask.Application.DTOs.Persons;
using ItaTask.Domain.Entities;
using ItaTask.Domain.Enums;
using ItaTask.Domain.Interfaces;
using ItaTask.Domain.Interfaces.Repositories;
using MediatR;

namespace ItaTask.Application.Features.Persons.Commands.CreatePerson;

public record CreatePersonCommand : IRequest<int>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required Gender Gender { get; init; }
    public required string PersonalNumber { get; init; }
    public required DateTime BirthDate { get; init; }
    public required int CityId { get; init; }
    public List<PhoneNumberDto>? PhoneNumbers { get; init; }
}

public class CreatePersonCommandHandler(
    IPersonRepository personRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePersonCommand, int>
{
    public async Task<int> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = new Person
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            PersonalNumber = request.PersonalNumber,
            BirthDate = request.BirthDate,
            CityId = request.CityId
        };

        if (request.PhoneNumbers != null)
        {
            foreach (var phoneNumber in request.PhoneNumbers)
            {
                person.PhoneNumbers.Add(new PhoneNumber
                {
                    Type = phoneNumber.Type,
                    Number = phoneNumber.Number,
                    PersonId = person.Id
                });
            }
        }

        await personRepository.AddAsync(person, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);


        return person.Id;
    }
}