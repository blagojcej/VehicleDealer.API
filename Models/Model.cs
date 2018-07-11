using System.ComponentModel.DataAnnotations;

namespace VehicleDealer.API.Models
{
    public class Model
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public Make Make { get; set; }
        public int MakeId { get; set; }
    }
}