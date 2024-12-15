using MassTransit;
using NotificationService;

namespace EmailService.Extensions
{
    public static class MessageBrokerExtension
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.AddConsumer<NotificationEventConsumer>();

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
                    {
                        h.Username(configuration["MessageBroker:Username"]!);
                        h.Password(configuration["MessageBroker:Password"]!);
                    });

                    configurator.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
