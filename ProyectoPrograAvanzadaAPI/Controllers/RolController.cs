using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoPrograAvanzadaAPI.Entities;
using ProyectoPrograAvanzadaAPI.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ProyectoPrograAvanzadaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController(IConfiguration _configuration, IUtilitariosModel _utilitariosModel,
                                   IHostEnvironment _hostEnvironment) : ControllerBase
    {

        [Authorize]
        [HttpGet]
        [Route("ConsultarRoles")]
        public IActionResult ConsultarRoles()
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                RolRespuesta respuesta = new RolRespuesta();

                var resultado = db.Query<Rol>("ConsultarRoles",
                    commandType: CommandType.StoredProcedure).ToList();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No hay roles registrados";
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
        [Route("ConsultaRolesp")]
        public IActionResult ConsultaRolesp(long IdRol)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                RolRespuesta respuesta = new RolRespuesta();

                var resultado = db.Query<Rol>("ConsultaRolesp",
                    new { IdRol },
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

        [HttpPost]
        [Route("RegistraRol")]
        public IActionResult RegistraRol(Rol entidad)
        {

                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    Respuesta respuesta = new Respuesta();

                    var resultado = db.Execute("CreaRol",
                        new { entidad.Nombre},
                        commandType: CommandType.StoredProcedure);

                    if (resultado <= 0)
                    {
                        respuesta.Codigo = "-1";
                    }

                    return Ok(respuesta);

                }
    
        }

        [HttpPut]
        [Route("tActualizaRol")]
        public IActionResult tActualizaRol(Rol entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                Respuesta respuesta = new Respuesta();

                    var resultado = db.Execute("ActualizaRol",
                    new { entidad.IdRol, entidad.Nombre },
                    commandType: CommandType.StoredProcedure);


                    if (resultado <= 0)
                    {
                        respuesta.Codigo = "-1";
                        respuesta.Mensaje = "No se pudo actualizar este rol";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = "00";
                    respuesta.Mensaje = "Rol actualizado correctamente";
                    return Ok(respuesta);
 
            }
        }


        [Authorize]
        [HttpDelete]
        [Route("EliminarRol")]
        public IActionResult EliminarRol(long IdRol)
        {

                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    Respuesta respuesta = new Respuesta();

                    var resultado = db.Execute("EliminaRol",
                        new { IdRol },
                        commandType: CommandType.StoredProcedure);

                    if (resultado <= 0)
                    {
                        respuesta.Codigo = "-1";
                        respuesta.Mensaje = "Rol no existente";
                    }

                    return Ok(respuesta);

                }
   
        }

    }
}
