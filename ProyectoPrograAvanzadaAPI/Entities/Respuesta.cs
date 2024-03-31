namespace ProyectoPrograAvanzadaAPI.Entities
{
    public class Respuesta
    {
        public Respuesta()
        {
            Codigo = "00";
            Mensaje = string.Empty;
        }

        public string? Mensaje { get; set; }
        public string? Codigo { get; set; }
        public long ConsecutivoGenerado { get; set; }
    }
}
