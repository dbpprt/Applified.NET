using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Applified.Core.Entities.Contracts;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.Entities.Identity
{
    public class OAuthClient : IApplicationDependant
    {
        [Key]
        [Index("EnsureUniqueRoleName", IsUnique = true, Order = 0)]
        public string Id { get; set; }

        [Required]
        [Index("EnsureUniqueRoleName", IsUnique = true, Order = 1)]
        public Guid ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }

        public string Secret { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public bool Active { get; set; }

        public int RefreshTokenLifeTime { get; set; }

        [MaxLength(100)]
        public string AllowedOrigin { get; set; }

        public OAuthGrant AllowedGrant { get; set; }
    }
}