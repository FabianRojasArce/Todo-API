using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data;

public class TodoContext : IdentityDbContext<User>
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>().HasMany(c => c.Listados).WithOne(e => e.User);
        builder.Entity<Listado>().HasMany(c => c.Tareas).WithOne(e => e.Listado);
    }

    public DbSet<Tarea> Tareas { get; set; } = default!;
    public DbSet<Listado> Listados { get; set; } = default!;
}
