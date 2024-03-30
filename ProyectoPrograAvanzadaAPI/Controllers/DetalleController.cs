using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProyectoPrograAvanzadaAPI.Entities;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ProyectoPrograAvanzadaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleController(IConfiguration _configuration) : ControllerBase
    {

        [Authorize]
        [HttpGet]
        [Route("ConsultarDetalles")]
        public IActionResult ConsultarDetalles(bool MostrarTodos)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                DetalleRespuesta respuesta = new DetalleRespuesta();

                var resultado = db.Query<Detalle>("ConsultarDetalles",
                    new { MostrarTodos },
                    commandType: CommandType.StoredProcedure).ToList();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No hay Detalle registrados";
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
        [Route("RegistrarDetalle")]
        public IActionResult RegistrarDetalle(Detalle entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                Respuesta respuesta = new Respuesta();

                var resultado = db.Query<Detalle>("RegistrarDetalle",
                    new { entidad.Cantidad, entidad.IdCarrito },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "Este servicio ya se encuentra registrado";
                }
                else
                {
                    respuesta.ConsecutivoGenerado = resultado.IdDetalle;
                }

                return Ok(respuesta);

            }
        }






    }
}
