using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Applified.Core.Entities.Contracts;

namespace Applified.Core.Entities.Infrastructure
{
    public class FeatureApplicationMapping : IApplicationDependant
    {
        [Required]
        [Key, Column(Order = 0)]
        public Guid ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        public Guid FeatureId { get; set; }

        [ForeignKey("FeatureId")]
        public Feature Feature { get; set; }
    }
}
