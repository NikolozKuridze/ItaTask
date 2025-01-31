using ItaTask.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace ItaTask.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly string _tempPath;
    private readonly string _uploadsPath;

    public FileService(IConfiguration configuration)
    {
        var basePath = Directory.GetCurrentDirectory();
        _tempPath = Path.Combine(basePath, "wwwroot", "temp");
        _uploadsPath = Path.Combine(basePath, "wwwroot", configuration["Storage:ImageUploadPath"]!);

        Directory.CreateDirectory(_tempPath);
        Directory.CreateDirectory(_uploadsPath);
    }

    public async Task<string> SaveToTempAsync(Stream imageStream, string fileName,
        CancellationToken cancellationToken = default)
    {
        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var tempFilePath = Path.Combine(_tempPath, uniqueFileName);

        await using var fileStream = new FileStream(tempFilePath, FileMode.Create);
        await imageStream.CopyToAsync(fileStream, cancellationToken);

        return tempFilePath;
    }

    public Task<string> MoveToUploadsAsync(string tempFilePath, int userId,
        CancellationToken cancellationToken = default)
    {
        var userFolder = Path.Combine(_uploadsPath, userId.ToString());
        Directory.CreateDirectory(userFolder);

        var fileName = Path.GetFileName(tempFilePath);
        var destinationPath = Path.Combine(userFolder, fileName);

        File.Move(tempFilePath, destinationPath, true);

        return Task.FromResult(Path.Combine("uploads/images", userId.ToString(), fileName));
    }

    public Task DeleteAsync(string? imagePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(imagePath))
            return Task.CompletedTask;

        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }
}