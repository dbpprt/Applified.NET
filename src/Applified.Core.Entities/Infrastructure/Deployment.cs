using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Applified.Core.Entities.Contracts;

namespace Applified.Core.Entities.Infrastructure
{
    public class Deployment : IApplicationDependant
    {
        [Key]
        public Guid DeploymentId { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }

        [Required]
        public DateTime PubDate { get; set; }

        [Required]
        public Guid StoredObjectId { get; set; }

        [ForeignKey("StoredObjectId")]
        public StoredObject StoredObject { get; set; }

        public string CommitMessage { get; set; }
    }
}
