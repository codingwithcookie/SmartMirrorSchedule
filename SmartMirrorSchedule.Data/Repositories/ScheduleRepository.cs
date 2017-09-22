using NLog;
using SmartMirrorSchedule.Data.Contexts;
using SmartMirrorSchedule.Data.Services;
using SmartMirrorSchedule.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMirrorSchedule.Data.Repositories
{
    public interface IScheduleRepository
    {
        Task<List<ScheduleViewModel>> GetAllSchedulesAsync();
        Task<List<ScheduleViewModel>> GetSchedulesByTimeSlotAsync(int timeSlotId);
    }

    public class ScheduleRepository : IScheduleRepository
    {
        private static ILogger _log;
        private static IScheduleContext _db;
        private static IRedisCacheService _redis;
        private static int RedisScheduleExpirtaionMinutes => Convert.ToInt32(ConfigurationManager.AppSettings["RedisScheduleExpirtaionMinutes"] ?? "5");

        public ScheduleRepository(ILogger log, 
            IScheduleContext db,
            IRedisCacheService redis)
        {
            _log = log;
            _db = db;
            _redis = redis;
        }

        public async Task<List<ScheduleViewModel>> GetAllSchedulesAsync()
        {
            var output = await (from session in _db.Sessions
                                join location in _db.Locations on session.LocationId equals location.Id
                                join timeSlot in _db.TimeSlots on session.TimeSlotId equals timeSlot.Id
                                join category in _db.Categories on session.CategoryId equals category.Id
                                orderby session.TimeSlotId, session.LocationId
                                select new
                                {
                                    Id = session.Id,
                                    Title = session.Title,
                                    Speaker = session.Speaker,
                                    LocationName = location.Name,
                                    StartTime = timeSlot.StartTime,
                                    EndTime = timeSlot.EndTime,
                                    CategoryName = category.Name
                                }).ToListAsync();

            var formattedOutput = output.Select(x => new ScheduleViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Speaker = x.Speaker,
                Location = x.LocationName,
                Time = $"{x.StartTime.ToShortTimeString()} - {x.EndTime.ToShortTimeString()}",
                Category = x.CategoryName
            }).ToList();

            return formattedOutput;
        }

        public async Task<List<ScheduleViewModel>> GetSchedulesByTimeSlotAsync(int timeSlotId)
        {
            var cacheKey = $"IScheduleRepository.GetSchedulesByTimeSlotAsync.{timeSlotId}";
            try
            {
                var cachedSchedules = await _redis.GetAsync<List<ScheduleViewModel>>(cacheKey);
                if (cachedSchedules != null)
                {
                    return cachedSchedules;
                }
                var output = await (from session in _db.Sessions
                                    join location in _db.Locations on session.LocationId equals location.Id
                                    join timeSlot in _db.TimeSlots on session.TimeSlotId equals timeSlot.Id
                                    join category in _db.Categories on session.CategoryId equals category.Id
                                    where session.TimeSlotId == timeSlotId
                                    orderby session.TimeSlotId, session.LocationId
                                    select new
                                    {
                                        Id = session.Id,
                                        Title = session.Title,
                                        Speaker = session.Speaker,
                                        LocationName = location.Name,
                                        StartTime = timeSlot.StartTime,
                                        EndTime = timeSlot.EndTime,
                                        CategoryName = category.Name
                                    }).ToListAsync();

                var formattedOutput = output.Select(x => new ScheduleViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Speaker = x.Speaker,
                    Location = x.LocationName,
                    Time = $"{x.StartTime.ToShortTimeString()} - {x.EndTime.ToShortTimeString()}",
                    Category = x.CategoryName
                }).ToList();
                                
                await _redis.SetAsync(cacheKey, formattedOutput);

                return formattedOutput;
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"IScheduleRepository.GetSchedulesByTimeSlotAsync failed. Error: {ex.Message}");
                return null;
            }            
        }        
    }
}
