using FluentValidation;
using ItaTask.Application.Common.Validators;
using ItaTask.Application.DTOs.Persons;
using ItaTask.Domain.Enums;
using ItaTask.Domain.Interfaces.Repositories;

namespace ItaTask.Application.Features.Persons.Commands.CreatePerson;

public class CreatePersonCommandValidator : BasePersonValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator(IPersonRepository personRepository)
        : base(personRepository)
    {
        RuleFor(x => x.PersonalNumber)
            .MustAsync(BeUniquePersonalNumber)
            .WithMessage("PersonalNumber already exists");
    }

    protected override string GetFirstName(CreatePersonCommand instance) => instance.FirstName;
    protected override string GetLastName(CreatePersonCommand instance) => instance.LastName;
    protected override string GetPersonalNumber(CreatePersonCommand instance) => instance.PersonalNumber;
    protected override DateTime GetBirthDate(CreatePersonCommand instance) => instance.BirthDate;
    protected override int GetCityId(CreatePersonCommand instance) => instance.CityId;
    protected override Gender GetGender(CreatePersonCommand instance) => instance.Gender;

    protected override IEnumerable<PhoneNumberDto>? GetPhoneNumbers(CreatePersonCommand instance) =>
        instance.PhoneNumbers;

    private async Task<bool> BeUniquePersonalNumber(string personalNumber, CancellationToken cancellationToken)
    {
        var existingPerson = await _personRepository.GetByPersonalNumberAsync(personalNumber, cancellationToken);
        return existingPerson == null;
    }
}