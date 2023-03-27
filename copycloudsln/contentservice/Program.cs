using contentservice.Auth;
using contentservice.Factories;
using contentservice.Services;
using contentservice.Utility;
using FirebaseAdmin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(FirebaseApp.Create());
// Add services to the container.
builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetValue<string>("ContentDbSettings:ConnectionString")));

// Initialize the open ai connection class
builder.Services.AddSingleton<IOpenAIManager, OpenAIManager>();
builder.Services.AddScoped<IWebsiteCopyService, WebsiteCopyService>();
builder.Services.AddSingleton<IWebsiteCopyFactory, WebsiteCopyFactory>();

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

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
