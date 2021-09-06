using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneTree.Assessment.Domain.Entities
{
    [Serializable]
    [Table("Product", Schema = "Assessment")]
    public class Product : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Desciption { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
    }
}
