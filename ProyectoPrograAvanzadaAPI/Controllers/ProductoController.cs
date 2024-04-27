using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProyectoPrograAvanzadaAPI.Entities;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using ProyectoPrograAvanzadaAPI.Interfaces;

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




       
        [HttpPost]
        [Route("RegistrarProducto")]
        public async Task<IActionResult> RegistrarProducto(Producto entidad)
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
                    new { entidad.IdProducto,entidad.Precio, entidad.IdCategoria, entidad.Estado, entidad.Nombre,entidad.Imagen },
                    commandType: CommandType.StoredProcedure);


                    if (resultado <= 0)
                    {
                        respuesta.Codigo = "-1";
                        respuesta.Mensaje = "No se pudo actualizar este servicio";
                        return Ok(respuesta);
                    }

                    respuesta.Codigo = "00";
                    respuesta.Mensaje = "Producto actualizado correctamente";
                    return Ok(respuesta);
              
            }
        }


        [Authorize]
        [HttpDelete]
        [Route("EliminarProducto")]
        public IActionResult EliminarProducto(long IdProducto)
        {

                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    Respuesta respuesta = new Respuesta();

                    var resultado = db.Execute("EliminaProducto",
                        new { IdProducto },
                        commandType: CommandType.StoredProcedure);

                    if (resultado <= 0)
                    {
                        respuesta.Codigo = "-1";
                        respuesta.Mensaje = "Producto no existente";
                    }

                    return Ok(respuesta);

                }

        }
    }
}
