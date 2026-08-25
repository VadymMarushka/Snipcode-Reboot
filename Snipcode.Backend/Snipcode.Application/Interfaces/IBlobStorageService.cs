namespace Snipcode.Application.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadSnippetAsync(string blobKey, string codeContent, CancellationToken ct = default);
    Task<string> GetSnippetContentAsync(string blobKey, CancellationToken ct = default);
    Task DeleteSnippetAsync(string blobKey, CancellationToken ct = default);
}