using Microsoft.AspNetCore.Identity;
using System.Data;
using WebSocketChatApp.Data.Entity;
using WebSocketChatApp.Data;

namespace WebSocketChatApp.Configurations
{
    public static class IdentityConfig
    {
        public static void ConfigIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
