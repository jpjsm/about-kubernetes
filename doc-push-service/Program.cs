using doc_push_service.models;
using doc_push_service.BackgroundServices;
using doc_push_service.Services;

namespace doc_push_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add background services to controller
            builder.Services.AddScoped<DocPusherService>();

            // Register as singleton first so it can be injected through Dependency Injection
            builder.Services.AddSingleton<PeriodicHostedService>();

            // Add as hosted service using the instance registered as singleton before
            builder.Services.AddHostedService(
                provider => provider.GetRequiredService<PeriodicHostedService>());


            // Add services to the container.
            builder.Services.AddHttpClient();
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Set port
            app.Urls.Add("http://*:8080");

            // Create routes to get background status
            app.MapGet("/background", (
                PeriodicHostedService service) =>
            {
                return new PeriodicHostedServiceState(service.IsEnabled);
            });

            app.MapMethods("/background", new[] { "PATCH" }, (
                PeriodicHostedServiceState state,
                PeriodicHostedService service) =>
            {
                service.IsEnabled = state.IsEnabled;
            });



            // Swagger always enabled
            app.UseSwagger();
            app.UseSwaggerUI();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
            }

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}