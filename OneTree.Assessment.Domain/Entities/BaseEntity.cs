using System;

namespace OneTree.Assessment.Domain.Entities
{
    /// <summary>
    /// Entity base
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseEntity<T>
    {
        /// <summary>
        /// Id model
        /// </summary>
        public T Id { get; set; }

        /// <summary>
        /// Date created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Date modified
        /// </summary>
        public DateTime? DateModified { get; set; }
    }
}
