using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using SmhiDb;
using SmhiDb.Abstract;
using SmhiDb.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SmhiDbExtensions
    {
        public static IServiceCollection AddSmhiDb(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<IUnitOfWork, SmhiDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            }, ServiceLifetime.Transient, ServiceLifetime.Transient);

            services.AddTransient<ISmhiDbService, SmhiDbService>();

            return services;
        }

        public static IHost AddMigrations(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                db.AddMigrations();
            }
            return host;
        }
    }
}
