namespace TodoApi.Models
{
    public class Listado
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public bool? Archivado { get; set; }
        public User? User { get; set; }
        public string? UserId { get; set; }

        public ICollection<Tarea> Tareas { get; set; } = default!;
    }
}
