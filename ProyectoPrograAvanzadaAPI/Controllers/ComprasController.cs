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
    public class ComprasController(IConfiguration _configuration, IUtilitariosModel _utilitariosModel) : ControllerBase
    {


        [Authorize]
        [HttpGet]
        [Route("ConsultarCompras")]
        public IActionResult ConsultarCompras()
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                ComprasRespuesta respuesta = new ComprasRespuesta();
                long IdUsuario = long.Parse(_utilitariosModel.Decrypt(User.Identity!.Name!));

                var resultado = db.Query<Compras>("ConsultarCompras",
                    new { IdUsuario },
                    commandType: CommandType.StoredProcedure).ToList();

                if (resultado.Count <= 0)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No hay compras realizadas";
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
        [Route("ConsultarDetalleCompras")]
        public IActionResult ConsultarDetalleCompras(long IdCompra)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                ComprasRespuesta respuesta = new ComprasRespuesta();

                var resultado = db.Query<Compras>("ConsultarDetalleCompras",
                    new { IdCompra },
                    commandType: CommandType.StoredProcedure).ToList();

                if (resultado.Count <= 0)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "Esta compra no tiene detalles registrados";
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
        [Route("ConsultarComprasMensuales")]
        public IActionResult ConsultarComprasMensuales()
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                ComprasRespuesta respuesta = new ComprasRespuesta();

                var resultado = db.Query<Compras>("ConsultarComprasMensuales",
                    new { },
                    commandType: CommandType.StoredProcedure).ToList();

                if (resultado.Count <= 0)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No hay compras realizadas";
                }
                else
                {
                    respuesta.Datos = resultado;
                }

                return Ok(respuesta);
            }
        }


    }
}
