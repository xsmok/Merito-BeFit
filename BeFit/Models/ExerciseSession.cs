using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseSession
    {
        public int Id { get; set; }

        public int ExerciseTypeId { get; set; }
        public ExerciseType ExerciseType { get; set; }
        
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }

        [Required]
        public float Weight { get; set; }

        [Required]
        public int Series { get; set; }

        [Required]
        public int Repetitions { get; set; }

        [Required]
        [Display(Name = "Created by")]
        public string CreatedById { get; set; }

        [Display(Name = "Created by")]
        public AppUser? CreatedBy { get; set; }
    }
}
