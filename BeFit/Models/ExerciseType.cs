using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }
        
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
