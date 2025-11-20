using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Created by")]
        public string CreatedById { get; set; }

        [Display(Name = "Created by")]
        public AppUser? CreatedBy { get; set; }
    }
}
