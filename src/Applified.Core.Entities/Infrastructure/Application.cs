using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Applified.Core.Entities.Contracts;

namespace Applified.Core.Entities.Infrastructure
{
    [Table("Applications")]
    public class Application : IGuidKey
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(128)]
        public string AccessToken { get; set; }

        public ICollection<Binding> Bindings { get; set; }

        public ICollection<Deployment> Deployments { get; set; }

        public Guid? ActiveDeploymentId { get; set; }

        [ForeignKey("ActiveDeploymentId")]
        public Deployment ActiveDeployment { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
