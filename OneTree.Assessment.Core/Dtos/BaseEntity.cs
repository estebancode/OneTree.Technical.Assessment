using System;
using System.Runtime.Serialization;

namespace OneTree.Assessment.Core.Dtos
{
    public class BaseEntity<T>
    {
        [DataMember(Name = "Id")]
        public T Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }
    }
}
