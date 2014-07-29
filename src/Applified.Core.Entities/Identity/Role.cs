using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Applified.Core.Entities.Contracts;
using Applified.Core.Entities.Infrastructure;
using Microsoft.AspNet.Identity;

namespace Applified.Core.Entities.Identity
{
    public class Role : IApplicationDependant, IRole<Guid>, IGuidKey
    {
        [Required]
        [Key, Column(Order = 1)]
        [Index("EnsureUniqueRoleName", IsUnique = true, Order =  0)]
        public Guid ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }

        public virtual ICollection<UserAccountRoleMapping> AccountRoleMappings { get; set; }

        [Required]
        [Key, Column(Order = 0)]
        [Index("EnsureUniqueRoleName", IsUnique = true, Order = 1)]
        [Index(IsUnique = true)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(30)]
        [Index("EnsureUniqueRoleName", IsUnique = true, Order = 2)]
        public string Name { get; set; }
    }
}