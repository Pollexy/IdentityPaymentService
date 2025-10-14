using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    Environment.GetEnvironmentVariable("DB_CONNECTION")              
    ?? builder.Configuration.GetConnectionString("DefaultConnection"); 

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Connection string bulunamadı.");

// -------- DbContext --------
builder.Services.AddDbContext<KonsentalkDbContext>(options =>
{
    var dsBuilder = new Npgsql.NpgsqlDataSourceBuilder(connectionString);
    dsBuilder.EnableDynamicJson();
    options.UseNpgsql(dsBuilder.Build());
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
