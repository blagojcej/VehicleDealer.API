using System.ComponentModel.DataAnnotations;

namespace VehicleDealer.API.Controllers.Resources
{
    public class ContactResource
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(255)]
        public string Phone { get; set; }
    }
}