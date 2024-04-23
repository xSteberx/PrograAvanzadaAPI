namespace ProyectoPrograAvanzadaAPI.Interfaces
{
    public interface IUtilitariosModel
    {
        public string GenerarToken(string correo, long IdUsuario);
        string GenerarNuevaContrasenna();
        public string Encrypt(string texto);
        public string Decrypt(string texto);
        void EnviarCorreo(string Destinatario, string Asunto, string Mensaje);
   
    }
}
