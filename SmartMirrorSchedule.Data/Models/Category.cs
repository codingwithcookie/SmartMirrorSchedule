using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMirrorSchedule.Data.Models
{
    [Table("Category")]
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
