using ItaTask.Domain.Entities;
using ItaTask.Domain.Exceptions;
using ItaTask.Domain.Interfaces;
using ItaTask.Domain.Interfaces.Repositories;
using MediatR;

namespace ItaTask.Application.Features.Persons.Commands.DeletePerson;

public record DeletePersonCommand(int Id) : IRequest;

public class DeletePersonCommandHandler(IPersonRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePersonCommand>
{
    public async Task Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await repository.GetByIdAsync(request.Id, cancellationToken)
                     ?? throw new NotFoundException(nameof(Person), request.Id);

        await repository.DeleteAsync(person, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}