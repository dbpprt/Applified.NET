using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Applified.Core.Entities.Infrastructure
{
    [Table("Applications")]
    public class Application
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(64)]
        public string AccessToken { get; set; }

        public ICollection<Binding> Bindings { get; set; }

        public ICollection<Deployment> Deployments { get; set; }

        public Guid? ActiveDeploymentId { get; set; }

        [ForeignKey("ActiveDeploymentId")]
        public Deployment ActiveDeployment { get; set; }
    }
}
