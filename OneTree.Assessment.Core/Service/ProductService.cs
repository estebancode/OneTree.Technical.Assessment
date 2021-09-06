using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OneTree.Assessment.Core.IService;
using OneTree.Assessment.Domain.Entities;
using OneTree.Assessment.Domain.IRepositories;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OneTree.Assessment.Core.Service
{
    public class ProductService : Service<Product, Dtos.Product>, IProductService
    {
        /// <summary>
        /// Blob storage
        /// </summary>
        private IBlobStorageRepository blobStorageRepository { get; set; }

        /// <summary>
        /// configuration's instance
        /// </summary>
        private readonly IConfiguration Config;

        private readonly IMapper mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper, IBlobStorageRepository blobStorageRepository, IConfiguration Config) : base(productRepository, mapper)
        {
            this.blobStorageRepository = blobStorageRepository;
            this.Config = Config;
            this.mapper = mapper;
        }

        public async Task<Dtos.Product> CreateAsync(Dtos.ProductToCreate productCreate)
        {
            Dtos.Product product = mapper.Map<Dtos.Product>(productCreate);
            product.Image = await SetImage(productCreate.File);
            product.Id = Guid.NewGuid();
            await AddAsync(product);
            return product;
        }

        public async Task<Dtos.Product> ModifyAsync(Dtos.ProductToUpdate productUpdate)
        {
            var productEntity = await FindByIdAsync(productUpdate.Id);
            if (productEntity == null)
            {
                throw new ArgumentNullException(typeof(Product).Name);
            }

            Dtos.Product product = mapper.Map<Dtos.Product>(productUpdate);
            product.Image = await SetImage(productUpdate.File);
            product.DateModified = DateTime.Now;
            product.DateCreated = productEntity.DateCreated;
            await UpdateAsync(product);
            return product;
        }

        private async Task<string> SetImage(IFormFile file)
        {
            byte[] result;
            using (var streamReader = new MemoryStream())
            {
                file.OpenReadStream().CopyTo(streamReader);
                result = streamReader.ToArray();
            }

            return await SaveFileToBlobAsync(file.FileName, result);
        }

        /// <summary>
        /// allow to save the image in blob storage
        /// </summary>
        /// <param name="imageName">Image name</param>
        /// <param name="fileByteArray">Array of bytes the image</param>
        /// <returns></returns>
        private async Task<string> SaveFileToBlobAsync(string imageName, byte[] fileByteArray)
        {
            blobStorageRepository.StorageConnectionString = Config["ConnectionStrings:StorageConnectionString"];
            blobStorageRepository.ContainerName = Config["Blob:ProductFiles"];

            var uploadImage = new UploadFile
            {
                FileName = imageName,
                ByteArray = fileByteArray
            };

            return await blobStorageRepository.SaveFileAsync(uploadImage).ConfigureAwait(false);
        }

        public async Task RemoveAsync(Guid id)
        {
            var product = await FindByIdAsync(id);
            if (product == null)
            {
                throw new ArgumentNullException(typeof(Product).Name);
            }
            await DeleteAsync(product);
        }
    }
}
