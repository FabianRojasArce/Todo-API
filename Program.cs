using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                           .AllowAnyMethod();
                      });
});

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

app.UseCors(MyAllowSpecificOrigins);
app.UseSwagger();
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
});

app.MapGet("/todos", async (TodoDb db) => await db.Todos.ToListAsync());

app.MapPost("/todo", async (TodoDb db, Todo todo) =>
{
    await db.Todos.AddAsync(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todo/{todo.Id}", todo);
});

app.MapGet("/todo/{id}", async (TodoDb db, int id) => await db.Todos.FindAsync(id));

app.MapGet("/todoTipo/{tipo}", async (TodoDb db, int tipo) => {

   TiposEstado estado = (TiposEstado)tipo;
   return await db.Todos.Where(a => a.Estado == estado).ToListAsync();
});

app.MapPut("/todo/{id}", async (TodoDb db, Todo updatetodo, int id) =>
{
      var todo = await db.Todos.FindAsync(id);
      if (todo is null) return Results.NotFound();
      todo.Nombre = updatetodo.Nombre;
      todo.Descripcion = updatetodo.Descripcion;
      await db.SaveChangesAsync();
      return Results.NoContent();
});

app.MapDelete("/todo/{id}", async (TodoDb db, int id) =>
{
   var todo = await db.Todos.FindAsync(id);
   if (todo is null)
   {
      return Results.NotFound();
   }
   db.Todos.Remove(todo);
   await db.SaveChangesAsync();
   return Results.Ok();
});
app.Run();
