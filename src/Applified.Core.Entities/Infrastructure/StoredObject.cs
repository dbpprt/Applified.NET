using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Applified.Core.Entities.Contracts;

namespace Applified.Core.Entities.Infrastructure
{
    public class StoredObject : IApplicationDependant
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }

        public string Type { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public long Size { get; set; }
    }
}
