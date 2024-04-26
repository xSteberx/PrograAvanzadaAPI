using ProyectoPrograAvanzadaAPI.Entities;
using ProyectoPrograAvanzadaAPI.Interfaces;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ProyectoPrograAvanzadaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IConfiguration _configuration, IUtilitariosModel _utilitariosModel,
                                   IHostEnvironment _hostEnvironment) : ControllerBase
    {

        [AllowAnonymous]
        [HttpPost]
        [Route("IniciarSesion")]
        public IActionResult IniciarSesion(Usuario entidad)
        {
            try
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
						respuesta.Dato.Token = _utilitariosModel.GenerarToken(resultado.Correo ?? string.Empty,resultado.IdUsuario);
					}

					return Ok(respuesta);
				}
			}catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RecuperarAcceso")]
        public IActionResult RecuperarAcceso(Usuario entidad)
        {
            try
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
			}catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("tRegistrarUsuario")]
        public IActionResult tRegistrarUsuario(Usuario entidad)
        {
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    Respuesta respuesta = new Respuesta();

                    string NuevaContrasenna = _utilitariosModel.GenerarNuevaContrasenna();
                    string Contrasenna = _utilitariosModel.Encrypt(NuevaContrasenna);
                    entidad.Contrasenna = Contrasenna;
                    bool EsTemporal = true;

                    var resultado = db.Execute("tRegistrarUsuario",
                        new { entidad.Correo, entidad.Contrasenna, entidad.NombreUsuario,entidad.IdRol, EsTemporal },
                        commandType: CommandType.StoredProcedure);

                    if (resultado <= 0)
                    {
                        respuesta.Codigo = "-1";
                        respuesta.Mensaje = "Su correo ya se encuentra registrado";
                    }
                    else
                    {
                        string ruta = Path.Combine(_hostEnvironment.ContentRootPath, "Password2.html");
                        string htmlBody = System.IO.File.ReadAllText(ruta);
                        htmlBody = htmlBody.Replace("@Usuario@", entidad.NombreUsuario);
                        htmlBody = htmlBody.Replace("@Contrasenna@", NuevaContrasenna);

                        _utilitariosModel.EnviarCorreo(entidad.Correo!, "Nueva Contraseña!!", htmlBody);
                        respuesta.ConsecutivoGenerado = resultado;
                    }

                    return Ok(respuesta);

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPut]
        [Route("CambiarContrasenna")]
        public IActionResult CambiarContrasenna(Usuario entidad)
        {
            try
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
			}catch (Exception ex) { 
				return BadRequest(ex.Message);
			}
        }

        [Authorize]
        [HttpPut]
        [Route("CambiarContraUser")]
        public IActionResult CambiarContraUser(Usuario entidad)
        {
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    UsuarioRespuesta respuesta = new UsuarioRespuesta();

					var resultado = db.Query<Usuario>("CambiarContraUser",
						new { entidad.IdUsuario, entidad.Contrasenna },
						commandType: CommandType.StoredProcedure);

                    if (resultado == null)
                    {
                        respuesta.Codigo = "-1";
                        respuesta.Mensaje = "Contraseña no Actualizada";
                    }
                   

                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet]
        [Route("ConsultarUsuarios")]
        public IActionResult ConsultarUsuarios()
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                UsuarioRespuesta respuesta = new UsuarioRespuesta();

                var resultado = db.Query<Usuario>("ConsultarUsuarios",
                    commandType: CommandType.StoredProcedure).ToList();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No hay usuarios registrados";
                }
                else
                {
                    respuesta.Datos = resultado;
                }

                return Ok(respuesta);
            }
        }


        [Authorize]
        [HttpGet]
        [Route("ConsultaUsuarioesp")]
        public IActionResult ConsultaUsuarioesp(long IdUsuario)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                UsuarioRespuesta respuesta = new UsuarioRespuesta();

                var resultado = db.Query<Usuario>("ConsultaUsuarioesp",
                    new { IdUsuario },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No hay usuarios registrados";
                }
                else
                {
                    respuesta.Dato = resultado;
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("CantidadUsuariosAct")]
        public IActionResult CantidadUsuariosAct()
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                UsuarioReportes respuesta = new UsuarioReportes();

                var cantidadUsuarios = db.Query<int>("CantidadUsuariosAct",
                    commandType: CommandType.StoredProcedure).FirstOrDefault(); 

                if (cantidadUsuarios == 0)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No hay usuarios activos";
                }
                else
                {
                    respuesta.Codigo = "00"; 
                    respuesta.Cantidad = cantidadUsuarios; 
                }

                return Ok(respuesta);
            }
        }


        [Authorize]
        [HttpGet]
        [Route("ConsultarUsuario")]
        public IActionResult ConsultarUsuario(long IdUsuario)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                UsuarioRespuesta respuesta = new UsuarioRespuesta();

                var resultado = db.Query<Usuario>("ConsultarUsuario",
                    new { IdUsuario },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No hay Producto registrados";
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
			try
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
			}catch(Exception ex) { 
				return BadRequest(ex.Message); 
			}
        }

        [Authorize]
        [HttpPut]
        [Route("ActualizarUsuario")]
        public IActionResult ActualizarUsuario(Usuario entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                Respuesta respuesta = new Respuesta();

                try
                {
                    var resultado = db.Execute("ActualizarUsuario",
                    new { entidad.IdUsuario, entidad.NombreUsuario, entidad.IdRol, entidad.Correo, entidad.Estado },
                    commandType: CommandType.StoredProcedure);


                    if (resultado <= 0)
                    {
                        respuesta.Codigo = "-1";
                        respuesta.Mensaje = "No se pudo actualizar este usuario";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = "00";
                    respuesta.Mensaje = "Usuario actualizado correctamente";
                    return Ok(respuesta);
                }
                catch (Exception ex)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "Error al actualizar el usuario: " + ex.Message;
                    return StatusCode(500, respuesta);
                }
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("EliminarUsuario")]
        public IActionResult EliminarUsuario(long  IdUsuario)
        {
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    Respuesta respuesta = new Respuesta();

                    var resultado = db.Execute("EliminarUsuario",
                        new { IdUsuario },
                        commandType: CommandType.StoredProcedure);

                    if (resultado <= 0)
                    {
                        respuesta.Codigo = "-1";
                        respuesta.Mensaje = "Usuario no existente";
                    }

                    return Ok(respuesta);

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
