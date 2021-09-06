using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OneTree.Assessment.Core.Dtos
{
    public class ProductToUpdate
    {
        [Required]
        [DataMember(Name = "Id")]
        public Guid Id { get; set; }

        [Required]
        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "Desciption")]
        public string Desciption { get; set; }

        [Required]
        [DataMember(Name = "Price")]
        public double Price { get; set; }

        [Required]
        [DataMember(Name = "ImageFile")]
        public IFormFile File { get; set; }
    }
}
