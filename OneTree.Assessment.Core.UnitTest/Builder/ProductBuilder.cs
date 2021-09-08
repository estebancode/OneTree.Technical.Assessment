
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using OneTree.Assessment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace OneTree.Assessment.Core.UnitTest.Builder
{
    public class ProductBuilder
    {
        private readonly Product product;
        private readonly Dtos.ProductToCreate productToCreate;
        private readonly Dtos.ProductToUpdate productToUpdate;
        private readonly Dtos.Product productDto;

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
                DateModified = DateTime.Now
            };

            productToCreate = new Dtos.ProductToCreate
            {
                Name = $"{Guid.NewGuid()}-name",
                Desciption = $"{Guid.NewGuid()}-description",
                Price = 43.99,
                File = NewFile(),
            };

            productToUpdate = new Dtos.ProductToUpdate
            {
                Id = Guid.NewGuid(),
                Name = $"{Guid.NewGuid()}-name",
                Desciption = $"{Guid.NewGuid()}-description",
                Price = 43.99,
                File = NewFile(),
            };

            productDto = new Dtos.Product
            {
                Id = Guid.NewGuid(),
                Name = $"{Guid.NewGuid()}-name",
                Desciption = $"{Guid.NewGuid()}-description",
                Price = 43.99,
                Image = $"{Guid.NewGuid()}-product.png",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };
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

        public Product Get()
        {
            return product;
        }

        public Dtos.ProductToCreate GetToCreate()
        {
            return productToCreate;
        }

        public Dtos.ProductToUpdate GetToUpdate()
        {
            return productToUpdate;
        }

        public IEnumerable<Product> Get_All()
        {
            return new List<Product> { product,product };
        }

        public Dtos.Product GetDto()
        {
            return productDto;
        }
    }
}
