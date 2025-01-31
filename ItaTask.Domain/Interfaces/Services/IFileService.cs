namespace ItaTask.Domain.Interfaces.Services;

public interface IFileService
{
    Task<string> SaveToTempAsync(Stream imageStream, string fileName, CancellationToken cancellationToken = default);
    Task<string> MoveToUploadsAsync(string tempFilePath, int userId, CancellationToken cancellationToken = default);
    Task DeleteAsync(string? imagePath, CancellationToken cancellationToken = default);
}