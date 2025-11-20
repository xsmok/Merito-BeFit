using BeFit.Models;
using System.ComponentModel.DataAnnotations;

namespace BeFit.DTOs
{
    public class ExerciseTypeDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    
        public ExerciseTypeDTO() { }
        public ExerciseTypeDTO(ExerciseType exerciseType)
        {
            Id = exerciseType.Id;
            Name = exerciseType.Name;
        }
    }
}
