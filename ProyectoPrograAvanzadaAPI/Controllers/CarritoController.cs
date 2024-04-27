using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProyectoPrograAvanzadaAPI.Entities;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using ProyectoPrograAvanzadaAPI.Interfaces;
using Microsoft.Extensions.Hosting;

namespace ProyectoPrograAvanzadaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController(IConfiguration _configuration,IUtilitariosModel _utilitariosModel, IHostEnvironment _hostEnvironment) : ControllerBase
    {

       
        [HttpGet]
        [Route("ConsultarCarrito")]
        public IActionResult ConsultarCarrito(long IdUsuario)
        {

				using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
				{
					CarritoRespuesta respuesta = new CarritoRespuesta();

					var resultado = db.Query<Carrito>("ConsultarCarrito",
						new { IdUsuario },
						commandType: CommandType.StoredProcedure).ToList();

					if (resultado == null)
					{
						respuesta.Codigo = "-1";
						respuesta.Mensaje = "No hay productos en el carrito";
					}
					else
					{
						respuesta.Datos = resultado;
					}

					return Ok(respuesta);
				}
			

        }


        [HttpPost]
		[Route("AgregarCarrito")]
		public IActionResult AgregarCarrito(Carrito entidad)
		{
				using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
				{
					CarritoRespuesta respuesta = new CarritoRespuesta();

					var resultado = db.Execute("AgregarCarrito",
						new { entidad.IdUsuario, entidad.IdProducto, entidad.Cantidad, entidad.Fecha },
						commandType: CommandType.StoredProcedure);

					if (resultado <= 0)
					{
						respuesta.Codigo = "-1";
						respuesta.Mensaje = "Error al agrgar al carrito";
					}

					return Ok(respuesta);

				}

		}

        [HttpDelete]
        [Route("RemueveProducto")]
        public IActionResult RemueveProducto( long IdProducto)
        {

                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    CarritoRespuesta respuesta = new CarritoRespuesta();

                    var resultado = db.Execute("RemueveProdCarrito",
                        new { IdProducto },
                        commandType: CommandType.StoredProcedure);

                    if (resultado <= 0)
                    {
                        respuesta.Codigo = "-1";
                        respuesta.Mensaje = "Error al remover del carrito";
                    }

                    return Ok(respuesta);

                }
            

        }


        [Authorize]
        [HttpPost]
        [Route("PagarCarrito")]
        public IActionResult PagarCarrito()
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                Respuesta respuesta = new Respuesta();
                var name = User.Identity.Name;
                long IdUsuario = long.Parse(_utilitariosModel.Decrypt(User.Identity.Name));

                var resultado = db.Query<Usuario>("PagarCarrito",
                    new { IdUsuario },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();


                if (resultado == null)
                {
                    respuesta.Codigo = "-1";
                    respuesta.Mensaje = "No se pudo realizar su pago";
                }
                else
                {
                    string ruta = Path.Combine(_hostEnvironment.ContentRootPath, "ConfirmacionCompra.html");
                    string htmlBody = System.IO.File.ReadAllText(ruta);
 

                    _utilitariosModel.EnviarCorreo(resultado.Correo!, "Compra Confirmada!!", htmlBody);
                
                }

                return Ok(respuesta);
            }
        }
    }
}
