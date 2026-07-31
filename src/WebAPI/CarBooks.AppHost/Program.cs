var builder = DistributedApplication.CreateBuilder(args);

// Structured logs and traces for the whole stack. Persistent so history survives a restart.
var seq = builder.AddSeq("seq")
    .WithLifetime(ContainerLifetime.Persistent);

// The single PostgreSQL instance backing the catalog. A fixed host port keeps `dotnet ef` and
// psql commands predictable during development.
var database = builder.AddPostgres("carbooks-postgres")
    .WithImageTag("18.1")
    .WithHostPort(15433)
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("carbooks");

var api = builder.AddProject<Projects.CarBooks_WebAPI>("carbooks-api")
    .WithReference(database)
    .WaitFor(database)
    .WithReference(seq)
    .WaitFor(seq)
    .WithExternalHttpEndpoints();

// The SPA calls the API through the Vite dev proxy, so the browser only ever sees one origin.
// Production packaging is handled by src/compose instead of the Aspire publisher.
builder.AddViteApp("carbooks-web", "../../WebApp")
    .WithNpm()
    .WithReference(api)
    .WaitFor(api)
    .WithEnvironment("API_URL", api.GetEndpoint("https"))
    .WithExternalHttpEndpoints();

builder.Build().Run();
