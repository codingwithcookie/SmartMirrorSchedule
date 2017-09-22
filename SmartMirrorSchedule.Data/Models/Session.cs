using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMirrorSchedule.Data.Models
{
    [Table("Session")]
    public class Session
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Speaker { get; set; }
        public string Description { get; set; }
        public int LocationId { get; set; }
        public int TimeSlotId { get; set; }
        public int CategoryId { get; set; }
    }
}
