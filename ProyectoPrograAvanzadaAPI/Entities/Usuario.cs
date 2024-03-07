﻿namespace ProyectoPrograAvanzadaAPI.Entities
{
    public class Usuario
    {
        public string? Contrasenna { get; set; }

        public string? NombreUsuario { get; set; }

        public string? Correo { get; set; }

        public short IdRol { get; set; }

        public bool Estado { get; set; }

        public string? Token { get; set; }
    }
    public class UsuarioRespuesta
    {

        public UsuarioRespuesta()
        {
            Codigo = "00";
            Mensaje = string.Empty;
        }
        public string? Codigo { get; set; }

        public string? Mensaje { get; set; }

        public Usuario? Dato { get; set; }

        public List<Usuario>? Datos { get; set; }
    }
}