using System.ComponentModel.DataAnnotations;

namespace SPA_Angular_CRUD.Models
{
    public class Position
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="Position")]
        [Required(ErrorMessage = "Position name is required")]
        public string Name { get; set; }
    }
}