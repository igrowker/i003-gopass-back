using Microsoft.EntityFrameworkCore;
using template_csharp_dotnet.Data;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
#region Services Area
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  options.UseSqlServer(connectionString);
});

#endregion Services Area

var app = builder.Build();

#region Middlewares Area
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

#endregion Middlewares Area
Console.WriteLine("Hola");
app.Run();
