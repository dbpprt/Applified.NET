using System.ComponentModel.DataAnnotations;

namespace Applified.Core.Entities.Infrastructure
{
    public class Server
    {
        [Key]
        [Required]
        public string Name { get; set; }

        [Key]
        public string BaseAddress { get; set; }

        public ServerRole ServerRole { get; set; }
    }
}
