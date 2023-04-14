using Microsoft.OpenApi.Models;
using TodoApi.Data;
using TodoApi.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultIdentity<User>().AddEntityFrameworkStores<TodoContext>();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<TodoContext>("Data Source=Todos.db");
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Todo API",
            Description = "Tablero de tareas",
            Version = "v1"
        }
    );
});

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
