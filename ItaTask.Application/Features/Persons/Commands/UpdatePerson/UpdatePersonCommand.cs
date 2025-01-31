using ItaTask.Application.DTOs.Persons;
using ItaTask.Domain.Entities;
using ItaTask.Domain.Enums;
using ItaTask.Domain.Exceptions;
using ItaTask.Domain.Interfaces;
using ItaTask.Domain.Interfaces.Repositories;
using MediatR;

namespace ItaTask.Application.Features.Persons.Commands.UpdatePerson;

public record UpdatePersonCommand : IRequest
{
    public int Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required Gender Gender { get; init; }
    public required string PersonalNumber { get; init; }
    public required DateTime BirthDate { get; init; }
    public required int CityId { get; init; }
    public List<PhoneNumberDto> PhoneNumbers { get; init; } = new();
}

public class UpdatePersonCommandHandler(
    IPersonRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePersonCommand>
{
    public async Task Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await repository.GetByIdAsync(request.Id, cancellationToken)
                     ?? throw new NotFoundException(nameof(Person), request.Id);

        person.FirstName = request.FirstName;
        person.LastName = request.LastName;
        person.Gender = request.Gender;
        person.PersonalNumber = request.PersonalNumber;
        person.BirthDate = request.BirthDate;
        person.CityId = request.CityId;

        person.PhoneNumbers.Clear();
        foreach (var phoneNumber in request.PhoneNumbers)
        {
            person.PhoneNumbers.Add(new PhoneNumber
            {
                Type = phoneNumber.Type,
                Number = phoneNumber.Number,
                PersonId = person.Id
            });
        }

        await repository.UpdateAsync(person, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}