using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Services;
using Service.Services.Interfaces;

namespace Service.DependencyInjection
{
    public class RegisterService
    {
        public static void ServiceInjection(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IChatMessageService, ChatMessageService>();
        }
    }
}
