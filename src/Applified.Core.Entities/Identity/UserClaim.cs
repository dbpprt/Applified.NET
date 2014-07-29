using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Applified.Core.Entities.Contracts;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.Entities.Identity
{
    public class UserClaim : IApplicationDependant
    {
        [Required]
        public Guid ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }

        [Required]
        [Key, Column(Order = 0)]
        public virtual Guid UserId { get; set; }

        [ForeignKey("UserId,ApplicationId")]
        public UserAccount User { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        public virtual string ClaimType { get; set; }

        [Required]
        public virtual string ClaimValue { get; set; }
    }
}