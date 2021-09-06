using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OneTree.Assessment.API.IntegrationTest.Builder;
using OneTree.Assessment.API.IntegrationTest.Server;
using OneTree.Assessment.Core.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OneTree.Assessment.API.IntegrationTest
{
    [TestClass]
    public class ProductControllerTest
    {

        private TestServer _server;
        private HttpClient _client;
        const string URL = "api/Product";
        ProductBuilder builder; 

        [TestInitialize]
        public void Init()
        {
            _server = ServerInit.GetServer();
            _client = _server.CreateClient();
            builder = new ProductBuilder();
        }

        [TestMethod]
        public void Get_All()
        {
            //Act
            var response = Task.Run(async () => await _client.GetAsync(URL));
            response.Result.EnsureSuccessStatusCode();

            var content = response.Result.Content.ReadAsStringAsync().Result;
            IEnumerable<Product> products = JsonConvert.DeserializeObject<IEnumerable<Product>>(content);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
            Assert.IsTrue(products.Any());
        }

        [TestMethod]
        public async Task Create()
        {

            //Arrange
            ProductToCreate product = builder.GetToCreate();

            var multipartContent = new MultipartFormDataContent();

            multipartContent.Add(new StreamContent(product.File.OpenReadStream()), "file", product.File.FileName);
            multipartContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") 
            { Name = "file", FileName = product.File.FileName };
            multipartContent.Add(new StringContent(JsonConvert.SerializeObject(product.Name)), "Name");
            multipartContent.Add(new StringContent(JsonConvert.SerializeObject(product.Desciption)), "Desciption");
            multipartContent.Add(new StringContent(JsonConvert.SerializeObject(product.Price)), "Price");

            //Act

            var postResponse = await _client.PostAsync(URL, multipartContent);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, postResponse.StatusCode);
        }
    }
}
