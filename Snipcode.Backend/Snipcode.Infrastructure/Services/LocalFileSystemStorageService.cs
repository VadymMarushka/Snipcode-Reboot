using Snipcode.Application.Interfaces;

namespace Snipcode.Infrastructure.Services;

public class LocalFileSystemStorageService : IBlobStorageService
{
    private readonly string _basePath;

    public LocalFileSystemStorageService()
    {
        _basePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "Blobs");
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    public async Task<string> UploadSnippetAsync(string blobKey, string codeContent, CancellationToken ct = default)
    {
        var filePath = Path.Combine(_basePath, blobKey);
        var directory = Path.GetDirectoryName(filePath);

        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(filePath, codeContent, ct);
        return blobKey;
    }

    public async Task<string> GetSnippetContentAsync(string blobKey, CancellationToken ct = default)
    {
        var filePath = Path.Combine(_basePath, blobKey);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Snippet content file was not found.", blobKey);
        }

        return await File.ReadAllTextAsync(filePath, ct);
    }

    public Task DeleteSnippetAsync(string blobKey, CancellationToken ct = default)
    {
        var filePath = Path.Combine(_basePath, blobKey);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}