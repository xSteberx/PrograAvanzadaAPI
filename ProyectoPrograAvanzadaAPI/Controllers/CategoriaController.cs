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
		/*Este sirve para poder hacer la consulta a nivel general, esto con booleano.*/
		[Authorize]
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
					respuesta.Mensaje = "No hay categorias registradas";
				}
				else
				{
					respuesta.Datos = resultado;
				}

				return Ok(respuesta);
			}
		}


		/*Este sirve para poder hacer la consulta a nivel especifico, precisamente el IdCategoria.*/
		[Authorize]
		[HttpGet]
		[Route("ConsultarCategoria")]
		public IActionResult ConsultarCategoria(long IdCategoria)
		{
			using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				CategoriaRespuesta respuesta = new CategoriaRespuesta();

				var resultado = db.Query<Categoria>("ConsultarCategoria",
					new { IdCategoria },
					commandType: CommandType.StoredProcedure).FirstOrDefault();

				if (resultado == null)
				{
					respuesta.Codigo = "-1";
					respuesta.Mensaje = "No hay categoria registrada";
				}
				else
				{
					respuesta.Dato = resultado;
				}

				return Ok(respuesta);
			}
		}


		/*Este sirve para poder registrar las categorias*/
		[Authorize]
		[HttpPost]
		[Route("RegistrarCategoria")]
		public IActionResult RegistrarCategoria(Categoria entidad)
		{
			using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				Respuesta respuesta = new Respuesta();

				var resultado = db.Execute("RegistrarCategoria",
					new { entidad.Nombre },
					commandType: CommandType.StoredProcedure);

				if (resultado <= 0)
				{
					respuesta.Codigo = "-1";
					respuesta.Mensaje = "Esta Categoria ya se encuentra registrado";
				}
				return Ok(respuesta);

			}
		}


		/*Este sirve para poder actualizar las categorias*/
		[Authorize]
		[HttpPut]
		[Route("ActualizarCategoria")]
		public IActionResult ActualizarCategoria(Categoria entidad)
		{
			using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				Respuesta respuesta = new Respuesta();

				var resultado = db.Execute("ActualizarCategoria",
					new { entidad.IdCategoria, entidad.Nombre },
					commandType: CommandType.StoredProcedure);

				if (resultado <= 0)
				{
					respuesta.Codigo = "-1";
					respuesta.Mensaje = "No se puede realizar la actualización, intentelo de nuevo.";
				}
				return Ok(respuesta);

			}
		}


		[Authorize]
		[HttpGet]
		[Route("ConsultarProductosCat")]
		public IActionResult ConsultarProductosCat(int idcategoria)
		{
			try
			{
				using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
				{
					ProductoRespuesta respuesta = new ProductoRespuesta();

					var resultado = db.Query<Producto>("ConsultarProductosCat",
						new { idcategoria },
						commandType: CommandType.StoredProcedure).ToList();

					if (resultado == null)
					{
						respuesta.Codigo = "-1";
						respuesta.Mensaje = "No hay productos registrados en esta categoria";
					}
					else
					{
						respuesta.Datos = resultado;
					}

					return Ok(respuesta);
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



		/*Este sirve para poder actualizar las categorias*/

		[HttpDelete]
		[Route("EliminarCategoria")]
		public IActionResult EliminarCategoria(long IdCategoria)
		{
			using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
			{
				Respuesta respuesta = new Respuesta();

				var resultado = db.Execute("EliminarCategoria",
					new { IdCategoria },
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
