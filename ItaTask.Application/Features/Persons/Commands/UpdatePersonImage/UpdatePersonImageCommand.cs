using ItaTask.Domain.Entities;
using ItaTask.Domain.Exceptions;
using ItaTask.Domain.Interfaces;
using ItaTask.Domain.Interfaces.Repositories;
using ItaTask.Domain.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ItaTask.Application.Features.Persons.Commands.UpdatePersonImage;

public record UpdatePersonImageCommand : IRequest<string>
{
    public int Id { get; init; }
    public required IFormFile Image { get; init; }
}

public class UpdatePersonImageCommandHandler(
    IPersonRepository repository,
    IFileService fileService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePersonImageCommand, string>
{
    public async Task<string> Handle(UpdatePersonImageCommand request, CancellationToken cancellationToken)
    {
        var person = await repository.GetByIdAsync(request.Id, cancellationToken)
                     ?? throw new NotFoundException(nameof(Person), request.Id);

        // Save to temp first
        var tempPath = await fileService.SaveToTempAsync(
            request.Image.OpenReadStream(),
            request.Image.FileName,
            cancellationToken);

        try
        {
            // Delete old image if exists
            if (!string.IsNullOrEmpty(person.ImagePath))
            {
                await fileService.DeleteAsync(person.ImagePath, cancellationToken);
            }

            // Move to permanent location
            var imagePath = await fileService.MoveToUploadsAsync(tempPath, person.Id, cancellationToken);

            person.ImagePath = imagePath;
            await repository.UpdateAsync(person, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return imagePath;
        }
        catch
        {
            // If anything fails, ensure temp file is cleaned up if it exists
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
            throw;
        }
    }
}