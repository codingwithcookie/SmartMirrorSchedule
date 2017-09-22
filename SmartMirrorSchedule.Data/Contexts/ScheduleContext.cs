using SmartMirrorSchedule.Data.Models;
using System.Data.Entity;

namespace SmartMirrorSchedule.Data.Contexts
{
    public interface IScheduleContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<Session> Sessions { get; set; }
        DbSet<TimeSlot> TimeSlots { get; set; }
        Database GetDb();
        DbContext GetDbContext();
    }

    public class ScheduleContext : DbContext, IScheduleContext
    {
        public ScheduleContext() : base("name=ScheduleContext")
        {
            Database.SetInitializer<ScheduleContext>(null);
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<TimeSlot> TimeSlots { get; set; }
        public Database GetDb() => Database;
        public DbContext GetDbContext() => this;
    }
}
