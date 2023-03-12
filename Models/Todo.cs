using Microsoft.EntityFrameworkCore;
namespace TodoApi.Models
{
    public enum TiposEstado
    {
        SinEstado = 1,
        PorHacer = 2,
        Haciendo = 3,
        Hecho = 4,
    }
    public class Todo
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public TiposEstado Estado { get; set; } = TiposEstado.SinEstado;
    }

}