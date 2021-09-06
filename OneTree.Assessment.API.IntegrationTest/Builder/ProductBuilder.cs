using Microsoft.AspNetCore.Http;
using OneTree.Assessment.Core.Dtos;
using System;
using System.Collections.Generic;
using System.IO;

namespace OneTree.Assessment.API.IntegrationTest.Builder
{
    public class ProductBuilder
    {
        private readonly Product product;
        private readonly ProductToCreate productToCreate;
        private readonly ProductToUpdate productToUpdate;

        public ProductBuilder()
        {
            product = new Product
            {
                Id = Guid.NewGuid(),
                Name = $"{Guid.NewGuid()}-name",
                Desciption = $"{Guid.NewGuid()}-description",
                Price = 43.99,
                Image = $"{Guid.NewGuid()}-product.png",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
            };

            productToCreate = new ProductToCreate
            {
                Name = $"{Guid.NewGuid()}-name",
                Desciption = $"{Guid.NewGuid()}-description",
                Price = 43.99,
                File = NewFile(),
            };

            productToUpdate = new ProductToUpdate
            {
                Id = Guid.NewGuid(),
                Name = $"{Guid.NewGuid()}-name",
                Desciption = $"{Guid.NewGuid()}-description",
                Price = 43.99,
                File = null,
            };
        }

        public Product Get()
        {
            return product;
        }

        public ProductToCreate GetToCreate()
        {
            return productToCreate;
        }

        public ProductToUpdate GetToUpdate()
        {
            return productToUpdate;
        }

        public IEnumerable<Product> Get_All()
        {
            return new List<Product> { product, product };
        }

        private IFormFile NewFile()
        {
            //Setup mock file using a memory stream
            var content = $"Hello World from a Fake File {Guid.NewGuid()}";
            var fileName = $"{Guid.NewGuid()}-name.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            return new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
        }
    }
}
