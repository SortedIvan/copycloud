using Microsoft.AspNetCore.Authentication;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
            policy =>
            {
                policy.WithOrigins("http://localhost:8080").AllowCredentials().AllowAnyHeader().AllowAnyMethod();
            });
});


builder.Services.AddOcelot(builder.Configuration);
var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);
app.UseOcelot().Wait();
app.Run();
