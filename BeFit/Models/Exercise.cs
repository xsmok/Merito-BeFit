using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        [Required]
        public DateTime DateTimeBeginning { get; set; }
        [Required]
        public DateTime DateTimeEnding { get; set; }

        [Required]
        [Display(Name = "Created by")]
        public string CreatedById { get; set; }

        [Display(Name = "Created by")]
        public AppUser? CreatedBy { get; set; }
    }
}
