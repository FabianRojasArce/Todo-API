namespace TodoApi.Models{
    public class Tarea
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public TiposEstado Estado { get; set; } = TiposEstado.SinEstado;
        public User? User { get; set; }
        public string? UserId { get; set; }
    }
    
    public enum TiposEstado
    {
        SinEstado = 1,
        PorHacer = 2,
        Haciendo = 3,
        Hecho = 4,
    }
}