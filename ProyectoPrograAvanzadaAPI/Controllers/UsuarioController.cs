using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using ProyectoPrograAvanzadaAPI.Entities;
using Dapper;
using ProyectoPrograAvanzadaAPI.Models;
using ProyectoPrograAvanzadaAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Hosting;

namespace ProyectoPrograAvanzadaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IConfiguration _configuration,IUtilitariosModel _utilitariosModel, IHostEnvironment _hostEnvironment) : ControllerBase
    {

        [AllowAnonymous]
        [HttpPost]
        [Route("IniciarSesion")]
        public IActionResult IniciarSesion(Usuario entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                UsuarioRespuesta respuesta = new UsuarioRespuesta();

                var resultado = db.Query<Usuario>("IniciarSesion",
                    new { entidad.Correo, entidad.Contrasenna },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "Sus datos no son correctos";
                }
                else
                {
                    respuesta.Dato = resultado;
                    respuesta.Dato.Token = _utilitariosModel.GenerarToken(resultado.Correo ?? string.Empty);
                }

                return Ok(respuesta);
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RecuperarAcceso")]
        public IActionResult RecuperarAcceso(Usuario entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                UsuarioRespuesta respuesta = new UsuarioRespuesta();

                string NuevaContrasenna = _utilitariosModel.GenerarNuevaContrasenna();
                string Contrasenna = _utilitariosModel.Encrypt(NuevaContrasenna);
                bool EsTemporal = true;

                var resultado = db.Query<Usuario>("RecuperarAcceso",
                    new { entidad.Correo, Contrasenna, EsTemporal },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "Sus datos no son correctos";
                }
                else
                {
                    string ruta = Path.Combine(_hostEnvironment.ContentRootPath, "Password.html");
                    string htmlBody = System.IO.File.ReadAllText(ruta);
                    htmlBody = htmlBody.Replace("@Usuario@", resultado.NombreUsuario);
                    htmlBody = htmlBody.Replace("@Contrasenna@", NuevaContrasenna);

                    _utilitariosModel.EnviarCorreo(resultado.Correo!, "Nueva Contraseña!!", htmlBody);
                    respuesta.Dato = resultado;
                }

                return Ok(respuesta);
            }
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("CambiarContrasenna")]
        public IActionResult CambiarContrasenna(Usuario entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                UsuarioRespuesta respuesta = new UsuarioRespuesta();
                bool EsTemporal = false;

                var resultado = db.Query<Usuario>("CambiarContrasenna",
                    new { entidad.Correo, entidad.Contrasenna, entidad.ContrasennaTemporal, EsTemporal },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "Sus datos no son correctos";
                }
                else
                {
                    respuesta.Dato = resultado;
                }

                return Ok(respuesta);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegistrarUsuario")]
        public IActionResult RegistrarUsuario(Usuario entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                Respuesta respuesta = new Respuesta();

                var resultado = db.Execute("RegistrarUsuario",
                    new { entidad.Correo, entidad.Contrasenna, entidad.NombreUsuario },
                    commandType: CommandType.StoredProcedure);

                if (resultado <= 0)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "Su correo ya se encuentra registrado";
                }

                return Ok(respuesta);

            }
        }


    }
}
