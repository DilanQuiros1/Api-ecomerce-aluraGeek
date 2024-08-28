using ApiProyecto.Modelos;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace ApiProyecto.Controllers
{
    public class CarritoController : Controller
    {

        private readonly ConexionBD conexion;

        public CarritoController(ConexionBD cbd)
        {
            this.conexion = cbd;
        }

        [HttpPost]
        [Route("[controller]/insertar_produto-carrito")]
        public IActionResult insertarProducto_carrito([FromBody] CarritoBD pro)
        {
            try
            {
                carrito c = new carrito(conexion);

                Mensaje msj;
                switch (c.insertar_pro_carrito(pro))
                {

                    case -1:
                        msj = new Mensaje("-1", "No se agrego", "info");
                        return BadRequest(new { msj });
                    case 1:
                        msj = new Mensaje("1", "Producto agregado al carrito con exito", "succ");
                        return Ok(new { msj });

                    default:
                        msj = new Mensaje("0", "Error desconocido al agregar al carrito", "error");
                        return BadRequest(new { msj });

                }

                /*c.insertar_pro_carrito(pro);
                Mensaje msj = new Mensaje("1", "Producto agregado al carrito con exito", "succ");
                return Ok(new { msj });*/

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        }


        [HttpGet("controller/get_productos_carrito_idPersona")]
        public async Task<IActionResult> GetData_usuarios(int id_persona)
        {
            try
            {

                List<ProductoBDcarrito> data = new List<ProductoBDcarrito>();

                using (MySqlConnection connection = conexion.GetConnection())//var connection = new MySQLConnection(connectionString);
                {
                    await connection.OpenAsync();

                    var sql = "SELECT tb_productos.id_producto, tb_productos.nombre_producto, tb_productos.url_producto,tb_productos.precio_producto, tb_productos.categoria, tb_persona.per_correo, tb_vendedor.ven_telefono, cantidad" +
                              " FROM tb_carrito JOIN tb_productos ON tb_productos.id_producto = tb_carrito.id_producto " +
                              "INNER JOIN tb_persona ON tb_persona.per_id=tb_carrito.id_persona AND tb_carrito.id_persona=" +
                               id_persona+" INNER JOIN tb_vendedor ON tb_vendedor.id_ven=tb_carrito.id_vendedor;";
                    using (var command = new MySqlCommand(sql, connection))//MySqlConnection conn = conexion.GetConnection() //var command = new MySqlCommand(sql, connection)
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var id = reader.GetInt32(0);
                                var nombre = reader.GetString(1);
                                var url = reader.GetString(2);
                                var precio = reader.GetInt32(3);
                                var categoria = reader.GetString(4);
                                var per_telefono = reader.GetString(5);
                                var telefono_ven = reader.GetString(6);
                                var cantidad_pro = reader.GetString(7);


                                data.Add(new ProductoBDcarrito { id_producto = id, nombre_producto = nombre, url_producto = url, precio_producto = precio, categoria = categoria,telefono_persona=per_telefono,ven_telefono=telefono_ven, cantidad=cantidad_pro}); ;
                            }
                        }
                    }
                    await connection.CloseAsync();//lo agregue
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("[controller]/vaciar_carrito")]
        public IActionResult vaciarCarrito(int per)
        {
            try
            {
                carrito c = new carrito(conexion);

                c.vaciarCarrito(per);
                Mensaje msj = new Mensaje("1", "El carrito has sido vaciado con exito", "succ");
                return Ok(new { msj });

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        }


        [HttpDelete]
        [Route("[controller]/eliminar_produto-carrito")]
        public IActionResult borrarProdCarrito(int prod, int per)
        {
            try
            {
                carrito c = new carrito(conexion);

                c.borrarProdCarrito(prod, per);
                Mensaje msj = new Mensaje("1", "Producto eliminado del carrito con exito", "succ");
                return Ok(new { msj });

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        }

        

    }
}
