using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Applified.Core.Entities.Infrastructure
{
    public class ApplicationTemplate
    {
        [Key]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public Guid StoredObjectId { get; set; }

        [ForeignKey("StoredObjectId")]
        public StoredObject StoredObject { get; set; }

        public DateTime LastUpdate { get; set; }

    }
}
