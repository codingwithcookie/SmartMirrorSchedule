using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMirrorSchedule.Data.Models
{
    [Table("Location")]
    public class Location
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
