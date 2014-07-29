using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Applified.Core.Entities.Infrastructure
{
    [Table("WellKnownApplications")]
    public class WellKnownApplication : Application
    {
        [Required]
        [Key]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
