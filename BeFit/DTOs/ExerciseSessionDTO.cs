using BeFit.Models;
using System.ComponentModel.DataAnnotations;

namespace BeFit.DTOs
{
    public class ExerciseSessionDTO
    {
        public int Id { get; set; }

        [Required]
        public int ExerciseTypeId { get; set; }

        [Required]
        public int ExerciseId { get; set; }

        [Required]
        public float Weight { get; set; }

        [Required]
        public int Repetitions { get; set; }

        [Required]
        public int Series { get; set; }

        public ExerciseSessionDTO() { }
        public ExerciseSessionDTO(ExerciseSession exerciseSession)
        {
            Id = exerciseSession.Id;
            ExerciseTypeId = exerciseSession.ExerciseTypeId;
            ExerciseId = exerciseSession.ExerciseId;
            Weight = exerciseSession.Weight;
            Repetitions = exerciseSession.Repetitions;
            Series = exerciseSession.Series;
        }
    }
}
