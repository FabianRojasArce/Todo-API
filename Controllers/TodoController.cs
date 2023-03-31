using TodoApi.Services;
using TodoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class TodoController : ControllerBase
{
    TodoService _service;

    public TodoController(TodoService service)
    {
        _service = service;
    }

    [Route("/todos")]
    [HttpGet]
    public IEnumerable<Todo> GetAll()
    {
        return _service.GetAll();
    }

    [Route("/todo/{id}")]
    [HttpGet]
    public ActionResult<Todo> GetById(int id)
    {
        var todo = _service.GetById(id);

        if (todo is not null)
        {
            return todo;
        }
        else
        {
            return NotFound();
        }
    }

    [Route("/todoTipo/{tipo}")]
    [HttpGet]
    public IEnumerable<Todo> GetByType(int tipo)
    {
        return _service.GetByType(tipo);
    }

    [Route("/todo")]
    [HttpPost]
    public IActionResult Create(Todo newTodo)
    {
        var todo = _service.Create(newTodo);
        return CreatedAtAction(nameof(GetById), new { id = todo!.Id }, todo);
    }

    [Route("/todo/{id}")]
    [HttpPut]
    public IActionResult Update(Todo updatetodo, int id)
    {

        if (updatetodo is not null)
        {
            _service.Update(updatetodo, id);
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }

    [Route("/todoEstado/{id}")]
    [HttpPut]
    public IActionResult UpdateStatus(Todo updatetodo, int id)
    {

        if (updatetodo is not null)
        {
            _service.UpdateStatus(updatetodo, id);
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }

    [Route("/todo/{id}")]
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var todo = _service.GetById(id);

        if (todo is not null)
        {
            _service.DeleteById(id);
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}
