using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog.Web;
using System;
using VerifyourUser.DataAccess.Context;

namespace VerifyourUser.API.Extensions
{
    public static class AppBuilderExtension
    {
        public static IApplicationBuilder SetupMigrations(this IApplicationBuilder app, IServiceProvider service)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                var context = service.GetService<VerifyourUserDbContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
            return app;
        }
    }
}
