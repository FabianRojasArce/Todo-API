using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi
{
    [Route("user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly UserManager<User> _userManager;

        public UserController(TodoContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<UserForm>> GetUser()
        {
            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);

            if (usuarioActual == null)
            {
                return Unauthorized();
            }

            var email = "";
            if (usuarioActual.Email != null)
            {
                email = usuarioActual.Email;
            }
            else
            {
                return BadRequest();
            }

            var user = new UserForm
            {
                Nombre = usuarioActual.Nombre,
                Apellido = usuarioActual.Apellido,
                Email = email
            };
            return user;
        }

        [HttpGet("foto")]
        public async Task<IActionResult> GetUserFoto()
        {
            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);

            if (usuarioActual == null)
            {
                return Unauthorized();
            }

            return File(usuarioActual.Foto, "image/jpeg");
        }

        [HttpPut("password")]
        public async Task<IActionResult> PutUserPassword(ChangePasswordForm form)
        {
            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);

            if (usuarioActual == null)
            {
                return Unauthorized("Sin usuario");
            }

            var check = await _userManager.CheckPasswordAsync(usuarioActual, form.CurrentPassword);

            if (check)
            {
                if (form.checkPasswords())
                {
                    var change = await _userManager.ChangePasswordAsync(
                        usuarioActual,
                        form.CurrentPassword,
                        form.Password
                    );

                    if (change.Succeeded)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("No se pudo cambiar");
                    }
                }
                else
                {
                    return (BadRequest("Las contraseñas no coinciden"));
                }
            }
            else
            {
                return Unauthorized("Falla la contraseña");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutUser(UserForm userForm)
        {
            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);

            if (usuarioActual == null)
            {
                return Unauthorized();
            }

            usuarioActual.Nombre = userForm.Nombre;
            usuarioActual.Apellido = userForm.Apellido;
            _context.Entry(usuarioActual).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(usuarioActual.Id))
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

        [HttpPost("foto")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("El archivo de imagen es nulo o vacío.");
            }

            // Verificar si el archivo es una imagen
            if (!file.ContentType.StartsWith("image/"))
            {
                return BadRequest("El archivo no es una imagen.");
            }

            byte[] imageData;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                imageData = stream.ToArray();
            }

            // Update the user's photo property
            user.Foto = imageData;
            await _userManager.UpdateAsync(user);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            var usuarioActual = await _userManager.GetUserAsync(HttpContext.User);

            if (usuarioActual == null)
            {
                return Unauthorized();
            }

            _context.Users.Remove(usuarioActual);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public class UserForm
        {
            public string Nombre { get; set; } = "";
            public string Apellido { get; set; } = "";
            public string Email { get; set; } = "";
        }

        public class ChangePasswordForm
        {
            public string CurrentPassword { get; set; } = "";
            public string Password { get; set; } = "";
            public string ConfirmPassword { get; set; } = "";

            public bool checkPasswords()
            {
                return this.Password == this.ConfirmPassword;
            }
        }
    }
}
