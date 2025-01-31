using FluentValidation;
using ItaTask.Domain.Interfaces.Repositories;

namespace ItaTask.Application.Features.Persons.Commands.AddRelatedPerson;

public class AddRelatedPersonCommandValidator : AbstractValidator<AddRelatedPersonCommand>
{
    private readonly IPersonRepository _personRepository;

    public AddRelatedPersonCommandValidator(IPersonRepository personRepository)
    {
        _personRepository = personRepository;

        RuleFor(x => x.PersonId)
            .GreaterThan(0)
            .MustAsync(PersonExists)
            .WithMessage("Person not found");

        RuleFor(x => x.RelatedPersonId)
            .GreaterThan(0)
            .MustAsync(PersonExists)
            .WithMessage("Related person not found")
            .NotEqual(x => x.PersonId)
            .WithMessage("Person cannot be related to themselves");

        RuleFor(x => x.RelationType)
            .IsInEnum();
    }

    private async Task<bool> PersonExists(int personId, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(personId, cancellationToken);
        return person != null && !person.IsDeleted;
    }
}