using FluentValidation;
using ItaTask.Application.Common.Validators;
using ItaTask.Application.DTOs.Persons;
using ItaTask.Domain.Enums;
using ItaTask.Domain.Interfaces.Repositories;

namespace ItaTask.Application.Features.Persons.Commands.UpdatePerson;

public class UpdatePersonCommandValidator : BasePersonValidator<UpdatePersonCommand>
{
    public UpdatePersonCommandValidator(IPersonRepository personRepository) 
        : base(personRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Person Id is required");

        When(x => HasPersonalNumberChanged(x).Result, () => {
            RuleFor(x => x.PersonalNumber)
                .MustAsync(BeUniquePersonalNumberExceptCurrent)
                .WithMessage("PersonalNumber already exists");
        });
    }

    protected override string GetFirstName(UpdatePersonCommand instance) => instance.FirstName;
    protected override string GetLastName(UpdatePersonCommand instance) => instance.LastName;
    protected override string GetPersonalNumber(UpdatePersonCommand instance) => instance.PersonalNumber;
    protected override DateTime GetBirthDate(UpdatePersonCommand instance) => instance.BirthDate;
    protected override int GetCityId(UpdatePersonCommand instance) => instance.CityId;
    protected override Gender GetGender(UpdatePersonCommand instance) => instance.Gender;
    protected override IEnumerable<PhoneNumberDto>? GetPhoneNumbers(UpdatePersonCommand instance) => instance.PhoneNumbers;

    private async Task<bool> BeUniquePersonalNumberExceptCurrent(UpdatePersonCommand command, string personalNumber, CancellationToken cancellationToken)
    {
        var existingPerson = await _personRepository.GetByPersonalNumberAsync(personalNumber, cancellationToken);
        return existingPerson == null || existingPerson.Id == command.Id;
    }

    private async Task<bool> HasPersonalNumberChanged(UpdatePersonCommand command)
    {
        var existingPerson = await _personRepository.GetByIdAsync(command.Id);
        return existingPerson != null && existingPerson.PersonalNumber != command.PersonalNumber;
    }
}