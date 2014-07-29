using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Applified.Core.Entities.Contracts;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.Entities.Identity
{
    public class UserLogin : IApplicationDependant
    {
        public virtual string LoginProvider { get; set; }

        public virtual string ProviderKey { get; set; }

        [Required]
        [Key]
        public virtual Guid UserId { get; set; }

        [ForeignKey("UserId,ApplicationId")]
        public virtual UserAccount UserAccount { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
    }
}