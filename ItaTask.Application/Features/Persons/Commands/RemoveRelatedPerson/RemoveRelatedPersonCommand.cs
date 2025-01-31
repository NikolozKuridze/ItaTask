using ItaTask.Domain.Entities;
using ItaTask.Domain.Exceptions;
using ItaTask.Domain.Interfaces;
using ItaTask.Domain.Interfaces.Repositories;
using MediatR;

namespace ItaTask.Application.Features.Persons.Commands.RemoveRelatedPerson;

public record RemoveRelatedPersonCommand : IRequest
{
    public int PersonId { get; init; }
    public int RelatedPersonId { get; init; }
}

public class RemoveRelatedPersonCommandHandler(IPersonRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveRelatedPersonCommand>
{
    public async Task Handle(RemoveRelatedPersonCommand request, CancellationToken cancellationToken)
    {
        var person = await repository.GetWithDetailsAsync(request.PersonId, cancellationToken)
                     ?? throw new NotFoundException(nameof(Person), request.PersonId);

        var relation = person.RelatedPersons
                           .FirstOrDefault(x => x.RelatedPersonId == request.RelatedPersonId)
                       ?? throw new NotFoundException("Related person relationship not found", request.RelatedPersonId);

        person.RelatedPersons.Remove(relation);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}