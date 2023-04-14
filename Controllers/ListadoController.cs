using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi
{
    [Route("listados")]
    [ApiController]
    [Authorize]
    public class ListadoController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly UserManager<User> _userManager;

        public ListadoController(TodoContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Tablero
        [HttpGet]
        public async Task<IActionResult> GetTableros()
        {
            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);

            if (usuarioActual == null)
            {
                return Unauthorized();
            }
            var tableros = _context.Listados
                .Where(t => t.UserId == usuarioActual.Id)
                .Select(
                    t =>
                        new
                        {
                            Id = t.Id,
                            Nombre = $"{t.Nombre}",
                            Archivado = t.Archivado
                        }
                )
                .ToList();

            return Ok(tableros);
        }

        // GET: api/Tablero/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTablero(int id)
        {
            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);
            if (usuarioActual == null)
            {
                return Unauthorized();
            }

            var tablero = _context.Listados
                .Where(t => t.UserId == usuarioActual.Id && t.Id == id)
                .Select(
                    t =>
                        new
                        {
                            Id = t.Id,
                            Nombre = $"{t.Nombre}",
                            Archivado = t.Archivado
                        }
                )
                .SingleOrDefault();

            if (tablero == null)
            {
                return NotFound();
            }

            return Ok(tablero);
        }

        // PUT: api/Tablero/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTablero(int id, TableroForm tableroForm)
        {
            if (id != tableroForm.Id)
            {
                return BadRequest();
            }

            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);

            if (usuarioActual == null)
            {
                return Unauthorized();
            }

            var tablero = _context.Listados.Find(id);
            if (tablero == null)
            {
                return BadRequest();
            }

            tablero.Nombre = tableroForm.Nombre;
            _context.Entry(tablero).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TableroExists(id))
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

        [HttpPut("archivar/{id}")]
        public async Task<IActionResult> PutTableroArchivado(int id, TableroForm tableroForm)
        {
            if (id != tableroForm.Id)
            {
                return BadRequest();
            }

            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);

            if (usuarioActual == null)
            {
                return Unauthorized();
            }

            var tablero = _context.Listados.Find(id);
            if (tablero == null)
            {
                return BadRequest();
            }

            tablero.Archivado = tableroForm.Archivado;
            _context.Entry(tablero).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TableroExists(id))
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

        // POST: api/Tablero
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Listado>> PostTablero(TableroForm tableroForm)
        {
            if (_context.Listados == null)
            {
                return Problem("Entity set 'TodoContext.Listados' is null.");
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (currentUser == null)
                return NotFound("Usuario no encontrado");

            if (currentUser.Listados == null)
                currentUser.Listados = new List<Listado>();

            currentUser.Tareas = new List<Tarea>();

            Listado newTablero = new Listado { };
            newTablero.Nombre = tableroForm.Nombre;
            newTablero.Archivado = tableroForm.Archivado;
            newTablero.User = currentUser;
            newTablero.UserId = currentUser.Id;
            currentUser?.Listados.Add(newTablero);
            _context.Listados.Add(newTablero);
            await _context.SaveChangesAsync();

            tableroForm.Id = newTablero.Id;

            return CreatedAtAction("GetTablero", new { id = newTablero.Id }, tableroForm);
        }

        // DELETE: api/Tablero/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTablero(int id)
        {
            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);
            if (usuarioActual == null)
            {
                return Unauthorized();
            }

            var tablero = _context.Listados
                .Where(t => t.UserId == usuarioActual.Id && t.Id == id)
                .Select(
                    t =>
                        new
                        {
                            Id = t.Id,
                            Nombre = $"{t.Nombre}",
                            Archivado = t.Archivado
                        }
                )
                .SingleOrDefault();

            if (tablero == null)
            {
                Console.WriteLine("tablero == null");
                return NotFound();
            }

            var tableroEliminar = await _context.Listados.FindAsync(tablero.Id);
            if (tableroEliminar == null)
            {
                return BadRequest();
            }

            //TODO: Eliminar tareas del tablero
            _context.Listados.Remove(tableroEliminar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TableroExists(int id)
        {
            return (_context.Listados?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public class TableroForm
        {
            public int Id { get; set; }
            public string? Nombre { get; set; }
            public bool? Archivado { get; set; }
        }
    }
}
