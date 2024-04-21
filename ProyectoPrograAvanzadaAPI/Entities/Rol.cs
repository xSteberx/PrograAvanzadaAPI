namespace ProyectoPrograAvanzadaAPI.Entities
{
    public class Rol
    {
        public long IdRol { get; set; }
        public string? Nombre { get; set; }
    }
    public class RolRespuesta
    {

        public RolRespuesta()
        {
            Codigo = "00";
            Mensaje = string.Empty;
        }

        public string? Codigo { get; set; }
        public string? Mensaje { get; set; }
        public Rol? Dato { get; set; }
        public List<Rol>? Datos { get; set; }
    }
}
