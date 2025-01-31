using ItaTask.Domain.Enums; 
using ItaTask.Domain.Interfaces.Repositories;
using MediatR;

namespace ItaTask.Application.Features.Persons.Queries.GetRelatedPersonsReport;

public record GetRelatedPersonsReportQuery(int PersonId) : IRequest<Dictionary<RelationType, int>>;


public class GetRelatedPersonsReportQueryHandler(IPersonRepository repository)
    : IRequestHandler<GetRelatedPersonsReportQuery, Dictionary<RelationType, int>>
{
    public async Task<Dictionary<RelationType, int>> Handle(
        GetRelatedPersonsReportQuery request,
        CancellationToken cancellationToken)
    { 
        return (Dictionary<RelationType, int>)await repository.GetRelatedPersonsCountByTypeAsync(request.PersonId,
            cancellationToken);
    }
}
