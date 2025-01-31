using FluentValidation;
using ItaTask.Domain.Interfaces.Repositories;

namespace ItaTask.Application.Features.Persons.Queries.GetRelatedPersonsReport;

public class GetRelatedPersonsReportQueryValidator : AbstractValidator<GetRelatedPersonsReportQuery>
{
    private readonly IPersonRepository _personRepository;

    public GetRelatedPersonsReportQueryValidator(IPersonRepository personRepository)
    {
        _personRepository = personRepository;

        RuleFor(x => x.PersonId)
            .GreaterThan(0)
            .MustAsync(PersonExists)
            .WithMessage("Person not found");
    }

    private async Task<bool> PersonExists(int personId, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(personId, cancellationToken);
        return person is { IsDeleted: false };
    }
}