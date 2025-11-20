using BeFit.Models;
using System.ComponentModel.DataAnnotations;

namespace BeFit.DTOs
{
    public class ExerciseSessionDTO
    {
        public int Id { get; set; }

        public int ExerciseTypeId { get; set; }
        virtual public ExerciseType ExerciseType { get; set; }

        public int ExerciseId { get; set; }
        virtual public Exercise Exercise { get; set; }
        public ExerciseSessionDTO() { }
        public ExerciseSessionDTO(ExerciseSession exerciseSession)
        {
            Id = exerciseSession.Id;
            ExerciseTypeId = exerciseSession.ExerciseTypeId;
            ExerciseId = exerciseSession.ExerciseId;
        }
    }
}
