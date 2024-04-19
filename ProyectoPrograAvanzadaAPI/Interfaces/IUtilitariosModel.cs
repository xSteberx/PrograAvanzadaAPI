namespace ProyectoPrograAvanzadaAPI.Interfaces
{
    public interface IUtilitariosModel
    {
        string GenerarToken(string correo);
        string GenerarNuevaContrasenna();
        public string Encrypt(string texto);
       
        void EnviarCorreo(string Destinatario, string Asunto, string Mensaje);
   
    }
}
