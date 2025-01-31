using FluentValidation;

namespace ItaTask.Application.Features.Persons.Queries.GetPersonsList;

public class GetPersonsListQueryValidator : AbstractValidator<GetPersonsListQuery>
{
    public GetPersonsListQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(50);
    }
}
