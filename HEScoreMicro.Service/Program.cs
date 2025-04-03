using HEScoreMicro.Persistence.MakeConnection;
using Microsoft.EntityFrameworkCore;
using HEScoreMicro.Service;
using HEScoreMicro.Application.MaperConfig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//DBConnection
builder.Services.AddDbContext<DbConnect>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//CORS Policy
builder.Services.AddCors(option =>
{
    option.AddPolicy("ParticuarOrigin", (policy) =>
    {
        policy.WithOrigins(builder.Configuration.GetConnectionString("FrontEndURL")).AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddAutoMapper(typeof(Program)); 

SupportFile.DependencyInjection(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ParticuarOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
