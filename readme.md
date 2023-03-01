# Api de tareas (To Do)

Api generada con:

- SDK: .NET 7.0
- Librerías:
    - Microsoft.EntityFrameworkCore.Design
    - Microsoft.EntityFrameworkCore.Sqlite
    - Swashbuckle.AspNetCore


# Instrucciones

1. Generar las migraciones:

    `dotnet ef migrations add InitialCreate`

2. Generar la base de datos SQLITE:

    `dotnet ef database update`

3. Ejecutar la aplicación:

    `dotnet run`