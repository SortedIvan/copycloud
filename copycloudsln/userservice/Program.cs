using MongoDB.Driver;
using userservice.Database;
using userservice.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetValue<string>("UserDbSettings:ConnectionString")));

builder.Services.AddScoped<IUserDbConfig, UserDbConfig>();
builder.Services.AddScoped<IRegisterService, RegisterService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
