namespace Core.Utils.DB;

public interface IFileCloudStorage
{
    public void SaveToCloud(string base64String, string folder, string subFolder, string fileName);
}