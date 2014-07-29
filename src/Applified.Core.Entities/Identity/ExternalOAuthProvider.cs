using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Applified.Core.Entities.Contracts;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.Entities.Identity
{
    public class ExternalOAuthProvider : IApplicationDependant, IGuidKey
    {
        [Required]
        [Index("EnsureUniqueName", IsUnique = true, Order = 1)]
        public Guid ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }

        [Required]
        [Index("EnsureUniqueName", IsUnique = true, Order = 0)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string ClientSecret { get; set; }

        [Required]
        public string ClientId { get; set; }
    }
}
