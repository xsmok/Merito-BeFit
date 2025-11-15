using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public DateTime DateTimeBegining { get; set; }
        public DateTime DateTimeEnding { get; set; }

        [Display(Name = "Created by")]
        public string CreatedById { get; set; }

        [Display(Name = "Created by")]
        public AppUser? CreatedBy { get; set; }
    }
}
