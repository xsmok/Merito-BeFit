using BeFit.Models;
using System.ComponentModel.DataAnnotations;

namespace BeFit.DTOs
{
    public class ExerciseDTO
    {
        public int Id { get; set; }
        [Required]
        public DateTime DateTimeBeginning { get; set; }
        [Required]
        public DateTime DateTimeEnding { get; set; }

        public ExerciseDTO() { }
        public ExerciseDTO(Exercise exercise)
        {
            Id = exercise.Id;
            DateTimeBeginning = exercise.DateTimeBeginning;
            DateTimeEnding = exercise.DateTimeEnding;
        }
    }
}
