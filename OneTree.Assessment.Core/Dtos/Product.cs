using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OneTree.Assessment.Core.Dtos
{
    public class Product : BaseEntity<Guid>
    {
        [Required]
        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "Desciption")]
        public string Desciption { get; set; }

        [Required]
        [DataMember(Name = "Price")]
        public double Price { get; set; }

        public string Image { get; set; }
    }
}
