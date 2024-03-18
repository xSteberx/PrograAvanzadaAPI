namespace ProyectoPrograAvanzadaAPI.Services
{
    public interface IUtilitariosModel
    {
        public string GenerarToken(string correo);
        public string Encrypt(string texto);
        public string GenerarNuevaContrasenna();
        public void EnviarCorreo(string Destinatario, string Asunto, string Mensaje);
    }
}
