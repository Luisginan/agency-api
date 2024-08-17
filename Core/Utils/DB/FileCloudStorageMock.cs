using System.Diagnostics.CodeAnalysis;

namespace Core.Utils.DB;

[ExcludeFromCodeCoverage]
public class FileCloudStorageMock(ILogger<FileCloudStorageMock> logger) : IFileCloudStorage
{
    public void SaveToCloud(string base64String, string folder, string subFolder, string fileName)
    {
        logger.LogWarning("FileCloudStorageMock SaveToCloud: using mock implementation");
    }
}