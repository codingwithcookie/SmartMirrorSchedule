using Autofac;
using Autofac.Integration.Mvc;
using NLog;
using SmartMirrorSchedule.Data.Contexts;
using SmartMirrorSchedule.Data.Repositories;
using SmartMirrorSchedule.Data.Services;
using System.Web.Mvc;

namespace SmartMirrorSchedule.Web.App_Start
{
    public static class ContainerConfig
    {
        public static IContainer Container;

        public static void RegisterComponents()
        {
            var builder = new ContainerBuilder();

            //Contexts
            builder.RegisterType<ScheduleContext>().As<IScheduleContext>().SingleInstance();

            //Repositories
            builder.RegisterType<ScheduleRepository>().As<IScheduleRepository>();

            //Services
            builder.Register(x => LogManager.GetCurrentClassLogger()).As<ILogger>();
            builder.RegisterType<RedisCacheService>().As<IRedisCacheService>();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            
            // Set the dependency resolver to be Autofac.
            Container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }
    }
}