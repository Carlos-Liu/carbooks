using System.Text.Json;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CarBooks.Application;
using CarBooks.Database.Ef;
using CarBooks.Database.Ef.Seeding;
using CarBooks.Infrastructure;
using CarBooks.Repository;
using CarBooks.ServiceDefaults;
using CarBooks.WebAPI;
using Microsoft.OpenApi;

const string CorsPolicyName = "CarBooksSpa";

var builder = WebApplication.CreateBuilder(args);

// Autofac owns composition: each layer contributes its own module.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule<InfrastructureModule>();
    container.RegisterModule<RepositoryModule>();
    container.RegisterModule<ApplicationModule>();
});

builder.AddServiceDefaults();

// Aspire supplies the connection string; the resource is named "carbooks" in the AppHost.
builder.AddNpgsqlDbContext<CarBooksDbContext>("carbooks");

// Seq is optional: the API still runs when no Seq resource is wired up.
if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("seq")))
{
    builder.AddSeqEndpoint("seq");
}

builder.Services.AddScoped<CarBooksDbSeeder>();
builder.Services.AddHostedService<DatabaseInitializer>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CarBooksExceptionHandler>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CarBooks API", Version = "v1" }));

// Only needed when the SPA is served from a different origin than the API. Behind the Nginx
// reverse proxy (and behind the Vite dev proxy) requests are same-origin and this stays empty.
var corsOrigins = builder.Configuration.GetValue<string>("CorsOrigins")
    ?.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    ?? [];

if (corsOrigins.Length > 0)
{
    builder.Services.AddCors(options => options.AddPolicy(
        CorsPolicyName,
        policy => policy.WithOrigins(corsOrigins).AllowAnyHeader().AllowAnyMethod()));
}

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "CarBooks API v1"));
}

if (corsOrigins.Length > 0)
{
    app.UseCors(CorsPolicyName);
}

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
