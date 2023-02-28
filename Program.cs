using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Todos") ?? "Data Source=Todos.db";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<TodoDb>(connectionString);
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo {
         Title = "Todo API",
         Description = "Tablero de tareas",
         Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
});

app.MapGet("/", () => "Hello World!");

app.Run();
