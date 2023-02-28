namespace TodoApi.Models
{
    public enum TiposEstado
    {
        SinEstado,
        PorHacer,
        Haciendo,
        Hecho,
    }
    public class Todo
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public TiposEstado Estado { get; set; } = TiposEstado.SinEstado;
    }
}