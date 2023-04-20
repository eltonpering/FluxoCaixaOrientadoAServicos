using Microsoft.OpenApi.Models;
using Questao5.Infrastructure.Extensions;
using Questao5.Infrastructure.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// sqlite
var connectionString = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite");
builder.Services.AddSingleton(new DatabaseConfig { Name = connectionString });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// Add dependencies
builder.Services.AddDependencies(connectionString);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Conta Corrente", Version = "v1" });
} );

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

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();

// Informações úteis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html


