using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Core.Config;
using Core.Utils.Security;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;

namespace Core.Utils.DB;

[ExcludeFromCodeCoverage]
public class FileCloudStorage(ILogger<FileCloudStorage> logger, IOptions<FileStorageConfig> options , IVault vault)
    : IFileCloudStorage
{
    private readonly string _bucketName = vault.RevealSecret(options.Value).BucketName;
    private readonly ActivitySource _activitySource = new("file.storage");

    private const string DocType = "image/jpeg";
    public void SaveToCloud(string base64String, string folder, string subFolder, string fileName)
    {
        using var activity = _activitySource.StartActivity();
        var imageBytes = Convert.FromBase64String(base64String);
        AddFolder(folder);
        AddFolder(folder + "/" + subFolder);
            
        var client = StorageClient.Create();
        client.UploadObject(
            _bucketName,
            folder + "/" + subFolder + "/" + fileName + ".jpeg",
            DocType,
            new MemoryStream(imageBytes));
            
        logger.LogDebug("FileCloudStorage SaveToCloud: {folder}/{subFolder}/{fileName}.jpeg", folder, subFolder, fileName);
       
        activity?.SetTag("folder", folder);
        activity?.SetTag("subFolder", subFolder);
        activity?.SetTag("fileName", fileName);
    }


    private void AddFolder(string folderName)
    {
        var storageClient = StorageClient.Create();
        if (!folderName.EndsWith('/')) folderName += "/";
        var content = Encoding.UTF8.GetBytes("");
        storageClient.UploadObject(_bucketName, folderName, "application/x-directory", new MemoryStream(content));
    }
}