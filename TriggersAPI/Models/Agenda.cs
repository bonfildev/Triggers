using System.ComponentModel.DataAnnotations;

namespace TriggersAPI.Models
{
    public class Agenda
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.MinValue;

        public bool IsBooked { get; set; } = false;

        public float Cost { get; set; } = 0;
    }
}
