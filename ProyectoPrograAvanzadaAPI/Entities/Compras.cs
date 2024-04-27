namespace ProyectoPrograAvanzadaAPI.Entities
{
    public class Compras
    {
        public long IdCompra { get; set; }
        public long IdDetalle { get; set; }
        public long IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public long IdProducto { get; set; }
        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public string? Mes { get; set; }
        public decimal TotalMes { get; set; }

    }
    public class ComprasRespuesta
    {
        public ComprasRespuesta()
        {
            Codigo = "00";
            Mensaje = string.Empty;
        }

        public string? Codigo { get; set; }
        public string? Mensaje { get; set; }
        public Compras? Dato { get; set; }
        public List<Compras>? Datos { get; set; }
    }
}
