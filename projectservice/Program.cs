using FirebaseAdmin;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using MongoDB.Driver;
using projectservice.Auth;
using projectservice.Data;
using projectservice.Services;
using Microsoft.Extensions.Azure;

using projectservice.Utility;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddSingleton(FirebaseApp.Create());

// Add services to the container.
builder.Services.AddAzureClients(serviceAdd =>
{
    serviceAdd.AddServiceBusClient(builder.Configuration.GetSection("ServiceBusConfig:ConnectionString").Value);
});

builder.Services.AddScoped<IBlobStorageHelper, BlobStorageHelper>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddSingleton<IPusherHelper, PusherHelper>();
builder.Services.AddScoped<IProjectDbConfig, ProjectDbConfig>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectInviteService, ProjectInviteService>();
builder.Services.AddHostedService<ProjectEventsService>();

// Add services to the container.
builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetSection("ProjectDbSettings:ConnectionString").Value));


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
            policy =>
            {
                policy.WithOrigins("https://deft-stroopwafel-dec647.netlify.app").AllowCredentials().AllowAnyHeader().AllowAnyMethod();
            });
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, (o) =>
    {
        o.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["token"];
                return Task.CompletedTask;
            }
        };
    }
    ).AddCookie(x =>
    {
        x.Cookie.Name = "token";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();


app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication(); // ALWAYS PUT app.useAuthentication first
app.UseAuthorization();

app.MapControllers();

app.Run();
