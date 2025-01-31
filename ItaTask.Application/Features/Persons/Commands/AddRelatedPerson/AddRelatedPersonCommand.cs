using ItaTask.Domain.Entities;
using ItaTask.Domain.Enums;
using ItaTask.Domain.Exceptions;
using ItaTask.Domain.Interfaces;
using ItaTask.Domain.Interfaces.Repositories;
using MediatR;

namespace ItaTask.Application.Features.Persons.Commands.AddRelatedPerson;

public record AddRelatedPersonCommand : IRequest
{
    public int PersonId { get; init; }
    public int RelatedPersonId { get; init; }
    public RelationType RelationType { get; init; }
}

public class AddRelatedPersonCommandHandler(IPersonRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<AddRelatedPersonCommand>
{
    public async Task Handle(AddRelatedPersonCommand request, CancellationToken cancellationToken)
    {
        var person = await repository.GetByIdAsync(request.PersonId, cancellationToken)
                     ?? throw new NotFoundException(nameof(Person), request.PersonId);


        person.RelatedPersons.Add(new RelatedPerson
        {
            PersonId = request.PersonId,
            RelatedPersonId = request.RelatedPersonId,
            RelationType = request.RelationType
        });

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}