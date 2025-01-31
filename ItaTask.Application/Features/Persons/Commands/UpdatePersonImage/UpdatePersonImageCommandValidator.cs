using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ItaTask.Application.Features.Persons.Commands.UpdatePersonImage;

public class UpdatePersonImageCommandValidator : AbstractValidator<UpdatePersonImageCommand>
{
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
    private const int MaxFileSizeInMb = 5;

    public UpdatePersonImageCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Image)
            .NotNull()
            .Must(BeValidSize).WithMessage($"File size must not exceed {MaxFileSizeInMb}MB")
            .Must(BeValidExtension).WithMessage($"File extension must be one of: {string.Join(", ", _allowedExtensions)}");
    }

    private bool BeValidExtension(IFormFile? file)
    {
        if (file == null) return false;
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return _allowedExtensions.Contains(extension);
    }

    private bool BeValidSize(IFormFile? file)
    {
        if (file == null) return false;
        return file.Length <= MaxFileSizeInMb * 1024 * 1024;
    }
}
