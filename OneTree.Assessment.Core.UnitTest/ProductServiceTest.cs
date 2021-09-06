using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using OneTree.Assessment.Core.Service;
using OneTree.Assessment.Core.UnitTest.Builder;
using OneTree.Assessment.Domain.Entities;
using OneTree.Assessment.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneTree.Assessment.Core.UnitTest
{
    [TestClass]
    public class ProductServiceTest
    {

        private ProductService productService { get; set; }

        private IBlobStorageRepository blobStorageRepository { get; set; }

        private IConfiguration Config { get; set; }

        private IMapper mapper { get; set; }

        private IProductRepository productRepository { get; set; }

        private ProductBuilder builder = null;

        IERepository<Product> eRepository { get; set; }

        [TestInitialize]
        public void Setup()
        {
            blobStorageRepository = Substitute.For<IBlobStorageRepository>();
            Config = Substitute.For<IConfiguration>();
            mapper = Substitute.For<IMapper>();
            productRepository = Substitute.For<IProductRepository>();
            eRepository = Substitute.For<IERepository<Product>>();
            productService = new ProductService(productRepository,mapper,blobStorageRepository,Config);
            builder = new ProductBuilder();
        }

        [TestMethod]
        public async Task Get_All_Async()
        {
            int counter = 0;
            int expectedResult = 1;
            productRepository.When(c => c.GetAllAsync()).DoNotCallBase();
            eRepository.GetAllAsync().Returns(builder.Get_All());
            productRepository.GetAllAsync().Returns(builder.Get_All());
            productRepository.When(c => c.GetAllAsync()).Do(o => counter++);
            IEnumerable<Dtos.Product> products = await productService.GetAllAsync().ConfigureAwait(false);
            Assert.AreEqual(expectedResult, counter);
            Assert.IsTrue(products.Any());
            Assert.IsTrue(products.Count() == expectedResult);
        }

        [TestMethod]
        public async Task Create_Async()
        {
            int counter = 0;
            int expectedResult = 2;
            blobStorageRepository.SaveFileAsync(Arg.Any<UploadFile>()).ReturnsForAnyArgs($"https://{Guid.NewGuid()}");
            blobStorageRepository.When(b=> b.SaveFileAsync(Arg.Any<UploadFile>())).Do(x=> counter++);
            productRepository.When(c => c.AddAsync(Arg.Any<Product>())).Do(o => counter++);
            await productService.CreateAsync(builder.GetToCreate());
            Assert.AreEqual(expectedResult, counter);
        }

        [TestMethod]
        public async Task Modify_Async()
        {
            int counter = 0;
            int expectedResult = 2;
            blobStorageRepository.SaveFileAsync(Arg.Any<UploadFile>()).ReturnsForAnyArgs($"https://{Guid.NewGuid()}");
            blobStorageRepository.When(b => b.SaveFileAsync(Arg.Any<UploadFile>())).Do(x => counter++);
            productRepository.When(c => c.AddAsync(Arg.Any<Product>())).Do(o => counter++);
            await productService.ModifyAsync(builder.GetToUpdate());
            Assert.AreEqual(expectedResult, counter);
        }

        [TestMethod]
        public async Task Remove_Async()
        {
            int counter = 0;
            int expectedResult = 2;
            productRepository.FindByIdAsync(Arg.Any<Guid>()).Returns(builder.Get());
            productRepository.When(c => c.FindByIdAsync(Arg.Any<Product>())).Do(o => counter++);
            productRepository.When(c => c.DeleteAsync(Arg.Any<Product>())).Do(o => counter++);
            await productService.RemoveAsync(builder.Get().Id);
            Assert.AreEqual(expectedResult, counter);
        }

    }
}
