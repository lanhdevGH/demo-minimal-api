using Microsoft.EntityFrameworkCore;
using WebSocketChatApp.Data;

namespace WebSocketChatApp.Configurations
{
    public static class DBConfig
    {
        public static void ConfigDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}
