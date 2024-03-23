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
    public class CategoriaController(IConfiguration _configuration) : ControllerBase
    {

        [HttpGet]
        [Route("ConsultarCategorias")]
        public IActionResult ConsultarCategorias(bool MostrarTodos)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                CategoriaRespuesta respuesta = new CategoriaRespuesta();

                var resultado = db.Query<Categoria>("ConsultarCategorias",
                    new { MostrarTodos }, 
                    commandType: CommandType.StoredProcedure).ToList();

                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No hay categorias registrados";
                }
                else
                {
                    respuesta.Datos = resultado;
                }

                return Ok(respuesta);
            }
        }

		[HttpGet]
		[Route("ConsultarProductosCat")]
		public IActionResult ConsultarProductosCat(int IdCategoria)
		{
			using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				ProductoRespuesta respuesta = new ProductoRespuesta();

				var resultado = db.Query<Producto>("ConsultarProductosCat",
					new { IdCategoria },
					commandType: CommandType.StoredProcedure).ToList();

				if (resultado == null)
				{
					respuesta.Codigo = "-1";
					respuesta.Mensaje = "No hay productos registrados en este categoria";
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
