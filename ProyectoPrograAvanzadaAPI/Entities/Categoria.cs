namespace ProyectoPrograAvanzadaAPI.Entities
{
    public class Categoria
    {
        public long IdCategoria { get; set; }
        public string? Nombre{ get; set; }


    }
    public class CategoriaRespuesta
    {
        public CategoriaRespuesta()
        {
            Codigo = "00";
            Mensaje = string.Empty;
        }

        public string? Codigo { get; set; }
        public string? Mensaje { get; set; }
        public Categoria? Dato { get; set; }
        public List<Categoria>? Datos { get; set; }
    }

}
