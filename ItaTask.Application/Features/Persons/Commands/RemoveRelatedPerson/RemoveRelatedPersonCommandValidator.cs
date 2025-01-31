using FluentValidation;
using ItaTask.Domain.Interfaces.Repositories;

namespace ItaTask.Application.Features.Persons.Commands.RemoveRelatedPerson;

public class RemoveRelatedPersonCommandValidator : AbstractValidator<RemoveRelatedPersonCommand>
{
    private readonly IPersonRepository _personRepository;

    public RemoveRelatedPersonCommandValidator(IPersonRepository personRepository)
    {
        _personRepository = personRepository;

        RuleFor(x => x.PersonId)
            .GreaterThan(0)
            .MustAsync(PersonExists)
            .WithMessage("Person not found");

        RuleFor(x => x.RelatedPersonId)
            .GreaterThan(0)
            .MustAsync(PersonExists)
            .WithMessage("Related person not found");
    }

    private async Task<bool> PersonExists(int personId, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(personId, cancellationToken);
        return person != null && !person.IsDeleted;
    }
}