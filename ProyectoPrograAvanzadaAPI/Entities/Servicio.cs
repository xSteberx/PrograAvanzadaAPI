namespace ProyectoPrograAvanzadaAPI.Entities
{
    public class Servicio
    {

        public long IdServicio { get; set; }
        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
        public string? Imagen { get; set; }
        public string? Video { get; set; }
        public bool Estado { get; set; }

    }



    public class ServicioRespuesta
    {
        public ServicioRespuesta()
        {
            Codigo = "00";
            Mensaje = string.Empty;
        }

        public string? Codigo { get; set; }
        public string? Mensaje { get; set; }
        public Servicio? Dato { get; set; }
        public List<Servicio>? Datos { get; set; }
    }

}
