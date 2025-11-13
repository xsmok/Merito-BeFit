using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public DateTime DateTimeBegining { get; set; }
        public DateTime DateTimeEnding { get; set; }
    }
}
