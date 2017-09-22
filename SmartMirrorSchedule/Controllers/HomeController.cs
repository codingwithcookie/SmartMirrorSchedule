using NLog;
using SmartMirrorSchedule.Data.Repositories;
using SmartMirrorSchedule.Data.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SmartMirrorSchedule.Web.Controllers
{
    public class HomeController : Controller
    {
        private static ILogger _log;
        private static IScheduleRepository _scheduleRepository;
        
        public HomeController(ILogger log,
            IScheduleRepository scheduleRepository)
        {
            _log = log;
            _scheduleRepository = scheduleRepository;
        }

        public async Task<ActionResult> Index(int id = 0)
        {
            if (id <= 0)
            {
                id = 1;
            }
            var sessions = await _scheduleRepository.GetSchedulesByTimeSlotAsync(id);
           
            return View(sessions);
        }        
    }
}