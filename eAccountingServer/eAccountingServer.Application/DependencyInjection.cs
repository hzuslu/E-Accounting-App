using eAccountingServer.Application.Behaviors;
using eAccountingServer.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace eAccountingServer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddFluentEmail("info@eaccount.com").AddSmtpSender("localhost", 25);
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            services.AddMediatR(conf =>
            {
                conf.RegisterServicesFromAssemblies(
                    typeof(DependencyInjection).Assembly,
                    typeof(AppUser).Assembly);
                conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
