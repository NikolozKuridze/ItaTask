using FluentValidation;
using ItaTask.Application.DTOs.Persons;
using ItaTask.Domain.Enums;
using ItaTask.Domain.Interfaces.Repositories;

namespace ItaTask.Application.Common.Validators;

public abstract class BasePersonValidator<T> : AbstractValidator<T>
{
    protected readonly IPersonRepository _personRepository;

    protected BasePersonValidator(IPersonRepository personRepository)
    {
        _personRepository = personRepository;

        RuleFor(x => GetFirstName(x))
            .NotEmpty()
            .Length(2, 50)
            .Must(BeValidName)
            .WithMessage("FirstName must contain only Georgian or only Latin characters");

        RuleFor(x => GetLastName(x))
            .NotEmpty()
            .Length(2, 50)
            .Must(BeValidName)
            .WithMessage("LastName must contain only Georgian or only Latin characters");

        RuleFor(x => GetPersonalNumber(x))
            .NotEmpty()
            .Length(11)
            .Matches("^[0-9]*$")
            .WithMessage("PersonalNumber must contain only digits");

        RuleFor(x => GetBirthDate(x))
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.Today.AddYears(-18))
            .WithMessage("Person must be at least 18 years old");

        RuleFor(x => GetCityId(x))
            .NotEmpty()
            .WithMessage("City is required");

        RuleFor(x => GetGender(x))
            .IsInEnum()
            .WithMessage("Invalid gender value");

        RuleFor(x => GetPhoneNumbers(x))
            .NotEmpty()
            .WithMessage("At least one phone number is required")
            .ForEach(phoneNumber =>
            {
                phoneNumber.ChildRules(phone =>
                {
                    phone.RuleFor(p => p.Number)
                        .NotEmpty()
                        .Length(4, 50)
                        .Matches(@"^\+?[0-9]+$")
                        .WithMessage("Phone number must contain only digits and optionally start with +");
                });
            });
    }

    protected abstract string GetFirstName(T instance);
    protected abstract string GetLastName(T instance);
    protected abstract string GetPersonalNumber(T instance);
    protected abstract DateTime GetBirthDate(T instance);
    protected abstract int GetCityId(T instance);
    protected abstract Gender GetGender(T instance);
    protected abstract IEnumerable<PhoneNumberDto>? GetPhoneNumbers(T instance);

    private bool BeValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        bool hasGeorgian = name.Any(c => c is >= 'ა' and <= 'ჰ');
        bool hasLatin = name.Any(c => c is >= 'A' and <= 'Z' || c is >= 'a' and <= 'z');

        return (hasGeorgian && !hasLatin) || (!hasGeorgian && hasLatin);
    }
}