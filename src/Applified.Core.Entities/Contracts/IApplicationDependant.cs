using System;
using System.ComponentModel.DataAnnotations;

namespace Applified.Core.Entities.Contracts
{
    public interface IApplicationDependant
    {
        [Required]
        Guid ApplicationId { get; set; }
    }
}
