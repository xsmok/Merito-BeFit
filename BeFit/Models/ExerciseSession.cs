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

        [Display(Name = "Created by")]
        public string CreatedById { get; set; }

        [Display(Name = "Created by")]
        public AppUser? CreatedBy { get; set; }
    }
}
