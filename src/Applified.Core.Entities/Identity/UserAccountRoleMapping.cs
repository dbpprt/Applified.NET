using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Applified.Core.Entities.Contracts;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.Entities.Identity
{
    public class UserAccountRoleMapping : IApplicationDependant
    {
        [Required]
        public Guid ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }

        [Required]
        [Key, Column(Order = 0)]
        public Guid UserId { get; set; }

        [ForeignKey("UserId,ApplicationId")]
        public UserAccount User { get; set; }

        public string Test { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        public Guid RoleId { get; set; }

        [ForeignKey("RoleId,ApplicationId")]
        public Role Role { get; set; }
    }
}