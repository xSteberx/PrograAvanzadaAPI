namespace ProyectoPrograAvanzadaAPI.Entities
{
    public class Carrito
    {
        public long IdUsuario { get; set; }
        public long IdCarrito { get; set; }
        public long IdProducto { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; }
        public string NombreCategoria { get; set; }
    }
    public class CarritoRespuesta
    {
        public CarritoRespuesta()
        {
            Codigo = "00";
            Mensaje = string.Empty;
        }

        public string? Codigo { get; set; }
        public string? Mensaje { get; set; }
        public Carrito? Dato { get; set; }
        public List<Carrito>? Datos { get; set; }
    }
}
