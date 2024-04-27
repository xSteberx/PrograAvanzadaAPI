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
    public class ReportesController(IConfiguration _configuration) : ControllerBase
    {
        [HttpGet]
        [Route("ConsultarStockProductos")]
        public IActionResult ConsultarStockProductos()
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                ProductoRespuesta respuesta = new ProductoRespuesta();

                var resultado = db.Query<Producto>("ReporteStockProductos",
                    commandType: CommandType.StoredProcedure).ToList();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No hay productos registrados";
                }
                else
                {
                    respuesta.Datos = resultado;
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ConsultaProductosCont")]
        public IActionResult ConsultaProductosCont()
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                ProductoRespuesta respuesta = new ProductoRespuesta();

                var resultado = db.Query<Producto>("VentasPorProd",
                    commandType: CommandType.StoredProcedure).ToList();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No hay productos registrados";
                }
                else
                {
                    respuesta.Datos = resultado;
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ConsultaVentasDia")]
        public IActionResult ReporteVentasDia()
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                VentaRespuesta respuesta = new VentaRespuesta();

                var cantidadVentas = db.Query<int>("ReporteVentasDia",
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (cantidadVentas == 0)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No hay ventas registradas";
                }
                else
                {
                    respuesta.Codigo = "00";
                    respuesta.Cantidad = cantidadVentas;
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

        [HttpGet]
        [Route("ReporteVentasMensual")]
        public IActionResult ReporteVentasMensual()
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                VentaRespuesta respuesta = new VentaRespuesta();

                var resultado = db.Query<Venta>("ReporteVentasMensuales",
                    commandType: CommandType.StoredProcedure).ToList();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No hay productos registrados";
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
