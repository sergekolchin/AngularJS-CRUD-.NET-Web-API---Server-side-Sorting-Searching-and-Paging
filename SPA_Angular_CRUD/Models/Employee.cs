using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SPA_Angular_CRUD.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required")]
        [JsonProperty("firstName")] //bug? does not work with "first_name"
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        [JsonProperty("lastName")] //bug? does not work with "last_name"
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Display(Name = "Gender")]
        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("positionId")]
        public int PositionId { get; set; }
        public Position Position { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }
    }
}