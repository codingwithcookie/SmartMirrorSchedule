using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirrorSchedule.Data.ViewModels
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Speaker { get; set; }
        public string Time { get; set; }
        public string Category { get; set; }
    }
}
