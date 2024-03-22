using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoPrograAvanzadaAPI.Entities;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ProyectoPrograAvanzadaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController(IConfiguration _configuration) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("ConsultarServicios")]
        public IActionResult ConsultarServicios(bool MostrarTodos)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                ServicioRespuesta respuesta = new ServicioRespuesta();

                var resultado = db.Query<Servicio>("ConsultarServicios",
                    new { MostrarTodos },
                    commandType: CommandType.StoredProcedure).ToList();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No hay servicios registrados";
                }
                else
                {
                    respuesta.Datos = resultado;
                }

                return Ok(respuesta);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("RegistrarServicio")]
        public IActionResult RegistrarServicio(Servicio entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                Respuesta respuesta = new Respuesta();

                var resultado = db.Execute("RegistrarServicio",
                    new { entidad.Nombre, entidad.Precio, entidad.Imagen, entidad.Video },
                    commandType: CommandType.StoredProcedure);

                if (resultado <= 0)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "Este servicio ya se encuentra registrado";
                }

                return Ok(respuesta);

            }
        }

    }
}
