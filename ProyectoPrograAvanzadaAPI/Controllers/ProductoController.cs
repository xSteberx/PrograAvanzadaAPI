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
    public class ProductoController(IConfiguration _configuration) : ControllerBase
    {

    
        [HttpGet]
        [Route("ConsultarProductos")]
        public IActionResult ConsultarProductos(bool MostrarTodos)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                ProductoRespuesta respuesta = new ProductoRespuesta();

                var resultado = db.Query<Producto>("ConsultarProductos",
                    new { MostrarTodos },
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




        [Authorize]
        [HttpGet]
        [Route("ConsultarProducto")]
        public IActionResult ConsultarProducto(long IdProducto)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                ProductoRespuesta respuesta = new ProductoRespuesta();

                var resultado = db.Query<Producto>("ConsultarProducto",
                    new { IdProducto },
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





        [Authorize]
        [HttpPost]
        [Route("RegistrarProducto")]
        public IActionResult RegistrarProducto(Producto entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                Respuesta respuesta = new Respuesta();

                var resultado = db.Query<Producto>("RegistrarProducto",
                    new { entidad.Nombre, entidad.Precio, entidad.IdCategoria, entidad.Imagen },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "Este Producto ya se encuentra registrado";
                }
                else
                {
                    respuesta.ConsecutivoGenerado = resultado.IdProducto;
                }

                return Ok(respuesta);

            }
        }

        [Authorize]
        [HttpPut]
        [Route("ActualizarProducto")]
        public IActionResult ActualizarProducto(Producto entidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                Respuesta respuesta = new Respuesta();

                var resultado = db.Execute("ActualizarProducto",
                    new { entidad.IdProducto,entidad.Nombre, entidad.Precio, entidad.IdCategoria, entidad.Estado },
                    commandType: CommandType.StoredProcedure);

                if (resultado <= 0)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No se pudo actualizar este servicio";
                }

                return Ok(respuesta);
            }
        }


        /*Este sirve para poder actualizar los productos*/

        [HttpDelete]
        [Route("EliminarProducto")]
        public IActionResult EliminarProducto(long IdProducto)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                Respuesta respuesta = new Respuesta();

                var resultado = db.Execute("EliminarProducto",
                    new { IdProducto },
                    commandType: CommandType.StoredProcedure);

                if (resultado <= 0)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No se puede realizar la eliminación, inténtelo de nuevo.";
                }
                return Ok(respuesta);

            }
        }









    }
}
