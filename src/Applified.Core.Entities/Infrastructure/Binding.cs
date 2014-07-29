using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Applified.Core.Entities.Contracts;

namespace Applified.Core.Entities.Infrastructure
{
    public class Binding : IApplicationDependant
    {
        [Key]
        public string Hostname { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
    }
}