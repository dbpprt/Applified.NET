using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Applified.Core.Entities.Infrastructure
{
    public class GlobalFeatureSetting
    {
        [Required]
        [Key, Column(Order = 0)]
        public Guid FeatureId { get; set; }

        [ForeignKey("FeatureId")]
        public Feature Feature { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
