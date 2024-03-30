namespace ProyectoPrograAvanzadaAPI.Entities
{
    public class Detalle
    {
        public long IdDetalle { get; set; }
        public long IdProducto { get; set; }

        public long IdCarrito { get; set; }

        public string? CodigoFactura { get; set; }
        public int? Cantidad { get; set; }

        public int? PrecioUnitario { get; set; }

    }
    public class DetalleRespuesta
    {
        public DetalleRespuesta()
        {
            Codigo = "00";
            Mensaje = string.Empty;
        }

        public string? Codigo { get; set; }
        public string? Mensaje { get; set; }
        public Detalle? Dato { get; set; }
        public List<Detalle>? Datos { get; set; }
    }
}
