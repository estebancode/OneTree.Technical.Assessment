using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OneTree.Assessment.Domain.Entities;
using OneTree.Assessment.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OneTree.Assessment.Domain.Repositories
{
    public class BlobStorageRepository : IBlobStorageRepository
    {

        /// <summary>
        /// connectionstring of blob storage
        /// </summary>
        public string StorageConnectionString { get; set; }

        /// <summary>
        /// containerName of blob storage
        /// </summary>
        public string ContainerName { get; set; }

        public BlobStorageRepository()
        {
        }

        /// <summary>
        /// save a file in blob storage
        /// </summary>
        /// <param name="formFile">file to save in the blob storage</param>
        /// <returns></returns>
        public async Task<string> SaveFileAsync(UploadFile formFile)
        {
            CloudBlobContainer cloudBlobContainer = null;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            cloudBlobContainer = cloudBlobClient.GetContainerReference(ContainerName);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(formFile.FileName);
            await cloudBlockBlob.UploadFromByteArrayAsync(formFile.ByteArray, 0, formFile.ByteArray.Length).ConfigureAwait(false);
            return cloudBlockBlob.Uri.AbsoluteUri;
        }

        /// <summary>
        /// delete file from blob storage 
        /// </summary>
        /// <param name="blobName">filename to delete</param>
        /// <returns></returns>
        public async Task<bool> DeleteFileAsync(string blobName)
        {
            CloudBlobContainer cloudBlobContainer = null;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            cloudBlobContainer = cloudBlobClient.GetContainerReference(ContainerName);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);
            return await cloudBlockBlob.DeleteIfExistsAsync().ConfigureAwait(false);
        }

    }
}
