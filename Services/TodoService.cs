using TodoApi.Models;
using TodoApi.Data;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Services;

public class TodoService
{
    private readonly TodoContext _context;

    public TodoService(TodoContext context)
    {
        _context = context;
    }

    public IEnumerable<Todo> GetAll()
    {
        return _context.Todos.AsNoTracking().ToList();
    }

    public Todo? GetById(int id)
    {
        return _context.Todos
            .AsNoTracking()
            .SingleOrDefault(p => p.Id == id);
    }

    public IEnumerable<Todo> GetByType(int tipo)
    {
        TiposEstado estado = (TiposEstado)tipo;
        return _context.Todos
            .AsNoTracking()
            .Where(p => p.Estado == estado);
    }

    public Todo Create(Todo newTodo)
    {
        _context.Todos.Add(newTodo);
        _context.SaveChanges();

        return newTodo;
    }

    public void Update(Todo updatetodo, int id)
    {
        var todo = _context.Todos.Find(id);
        if (todo is not null)
        {
            todo.Nombre = updatetodo.Nombre;
            todo.Descripcion = updatetodo.Descripcion;
            _context.SaveChanges();
        }
    }

    public void UpdateStatus(Todo updatetodo, int id)
    {
        var todo = _context.Todos.Find(id);
        if (todo is not null)
        {
            todo.Estado = updatetodo.Estado;
            _context.SaveChanges();
        }
    }

    public void DeleteById(int id)
    {
        var todoToDelete = _context.Todos.Find(id);
        if (todoToDelete is not null)
        {
            _context.Todos.Remove(todoToDelete);
            _context.SaveChanges();
        }
    }
}
