using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TodoApi.Models;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    public string Nombre { get; set; } = "";
    public string Apellido { get; set; } = "";
    public byte[] Foto { get; set; } = new byte[0];
    public ICollection<Tarea> Tareas { get; set; } = default!;
    public ICollection<Listado> Listados { get; set; } = default!;
}
