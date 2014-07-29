using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Applified.Core.Entities.Contracts;

namespace Applified.Core.Entities.Infrastructure
{
    public class Feature : IGuidKey
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string VersionIdentifier { get; set; }

        [Required]
        public string Author { get; set; }

        public FeatureType FeatureType { get; set; }

        public Guid? StoredObjectId { get; set; }

        [ForeignKey("StoredObjectId")]
        public StoredObject StoredObject { get; set; }
    }
}
