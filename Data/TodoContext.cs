using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data;

public class TodoContext : IdentityDbContext
{
    public TodoContext (DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<Todo> Todos => Set<Todo>();
}