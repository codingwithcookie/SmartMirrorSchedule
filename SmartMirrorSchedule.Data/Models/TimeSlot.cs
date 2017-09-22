using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMirrorSchedule.Data.Models
{
    [Table("TimeSlot")]
    public class TimeSlot
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
