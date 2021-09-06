using OneTree.Assessment.Domain.Entities;
using System.Threading.Tasks;

namespace OneTree.Assessment.Domain.IRepositories
{
    public interface IBlobStorageRepository
    {
        string StorageConnectionString { get; set; }
        string ContainerName { get; set; }
        Task<string> SaveFileAsync(UploadFile formFile);
        Task<bool> DeleteFileAsync(string blobName);
    }
}
