namespace ProyectoPrograAvanzadaAPI.Entities
{
    public class Venta
    {
        public long Cantidad { get; set; }
        public string? Mes { get; set; }
        public string? Annio { get; set; }
    }


    public class VentaRespuesta
    {
        public VentaRespuesta()
        {
            Codigo = "00";
            Mensaje = string.Empty;
        }

        public string? Codigo { get; set; }
        public string? Mensaje { get; set;}
        public long Cantidad { get; set; }
        public Venta? Dato { get; set; }
        public List<Venta>? Datos { get; set; }
    }

}
