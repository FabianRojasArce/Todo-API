using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi
{
    [Route("tareas")]
    [ApiController]
    [Authorize]
    public class TareaController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly UserManager<User> _userManager;

        public TareaController(TodoContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetTarea()
        {
            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);

            if (usuarioActual == null)
            {
                return Unauthorized();
            }

            var tareas = _context.Tareas
                .Where(t => t.UserId == usuarioActual.Id)
                .Select(
                    t =>
                        new
                        {
                            Id = t.Id,
                            Nombre = $"{t.Nombre}",
                            Descripcion = $"{t.Descripcion}",
                            Estado = t.Estado
                        }
                )
                .ToList();

            return Ok(tareas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTarea(int id)
        {
            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);
            if (usuarioActual == null)
            {
                return Unauthorized();
            }

            var tareas = _context.Tareas
                .Where(t => t.UserId == usuarioActual.Id && t.Id == id)
                .Select(
                    t =>
                        new
                        {
                            Id = t.Id,
                            Nombre = $"{t.Nombre}",
                            Descripcion = $"{t.Descripcion}",
                            Estado = t.Estado
                        }
                )
                .SingleOrDefault();

            if (tareas == null)
            {
                return NotFound();
            }

            return Ok(tareas);
        }

        [HttpGet("tareaTipo/{estado}")]
        public async Task<ActionResult<Tarea>> GetTareaByEstado(int estado)
        {
            TiposEstado e = (TiposEstado)estado;
            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);
            if (usuarioActual == null)
            {
                return Unauthorized();
            }

            var tareas = _context.Tareas
                .Where(t => t.UserId == usuarioActual.Id && t.Estado == e)
                .Select(
                    t =>
                        new
                        {
                            Id = t.Id,
                            Nombre = $"{t.Nombre}",
                            Descripcion = $"{t.Descripcion}",
                            Estado = t.Estado
                        }
                )
                .ToList();

            if (tareas == null)
            {
                return NotFound();
            }

            return Ok(tareas);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarea(int id, TareaForm tareaForm)
        {
            if (id != tareaForm.Id)
            {
                return BadRequest();
            }

            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);

            if (usuarioActual == null)
            {
                return Unauthorized();
            }

            var tarea = _context.Tareas.Find(id);
            if (tarea == null)
            {
                return BadRequest();
            }

            tarea.Nombre = tareaForm.Nombre;
            tarea.Descripcion = tareaForm.Descripcion;
            _context.Entry(tarea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TareaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("estado/{id}")]
        public async Task<IActionResult> PutTareaEstado(int id, TareaForm tareaForm)
        {
            if (id != tareaForm.Id)
            {
                return BadRequest();
            }

            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);

            if (usuarioActual == null)
            {
                return Unauthorized();
            }

            var tarea = _context.Tareas.Find(id);
            if (tarea == null)
            {
                return BadRequest();
            }

            tarea.Estado = tareaForm.Estado;
            _context.Entry(tarea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TareaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Tarea>> PostTarea(TareaForm tarea)
        {
            if (_context.Tareas == null)
            {
                return Problem("Entity set 'TodoContext.Tarea'  is null.");
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (currentUser == null)
                return NotFound();

            if (currentUser.Tareas == null)
                currentUser.Tareas = new List<Tarea>();

            Tarea newTarea = new Tarea { };
            newTarea.Nombre = tarea.Nombre;
            newTarea.Descripcion = tarea.Descripcion;
            newTarea.User = currentUser;
            newTarea.UserId = currentUser.Id;
            currentUser?.Tareas.Add(newTarea);
            _context.Tareas.Add(newTarea);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTarea", new { id = tarea.Id }, tarea);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(int id)
        {
            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);
            if (usuarioActual == null)
            {
                return Unauthorized();
            }

            var tarea = _context.Tareas
                .Where(t => t.UserId == usuarioActual.Id && t.Id == id)
                .Select(
                    t =>
                        new
                        {
                            Nombre = $"{t.Nombre}",
                            Descripcion = $"{t.Descripcion}",
                            Id = t.Id,
                            Estado = t.Estado
                        }
                )
                .SingleOrDefault();

            if (tarea == null)
            {
                Console.WriteLine("tarea == null");
                return NotFound();
            }

            var tareaEliminar = await _context.Tareas.FindAsync(tarea.Id);
            if (tareaEliminar == null)
            {
                return BadRequest();
            }

            _context.Tareas.Remove(tareaEliminar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TareaExists(int id)
        {
            return (_context.Tareas?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public class TareaForm
        {
            public int Id { get; set; }
            public string? Nombre { get; set; }
            public string? Descripcion { get; set; }
            public TiposEstado Estado { get; set; }
        }
    }
}
