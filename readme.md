# Api de tareas (To Do)

Api generada con:

- SDK: .NET 7.0
- Librerías:
    - Microsoft.EntityFrameworkCore.Design
    - Microsoft.EntityFrameworkCore.Sqlite
    - Swashbuckle.AspNetCore


# Instrucciones

1. Si existe, borre la base de datos de SQlite del directorio ("Todos.db")

2. Gener la base de datos SQLITE:

    `dotnet ef database update`

3. Ejecute la aplicación:

    `dotnet run`