using ApiProyecto.Modelos;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace ApiProyecto.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ConexionBD conexion;

        public ProductoController(ConexionBD cbd)
        {
            this.conexion = cbd;
        }

        [HttpPost]
        [Route("[controller]/insertar_produto")]//insertar en tbl_usario LISTO
        public IActionResult insertarProducto([FromBody] ProductoBD pro)//j9kKs9Uaoq
        {
            try
            {
                Productos U = new Productos(conexion);

                U.insertarProducto(pro);
                Mensaje msj = new Mensaje("1", "Producto creado con exito", "succ");
                return Ok(new { msj });

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        }


        //Mostrar SOLO DEL ESPESIFICO DE VEN
        [HttpGet]
        [Route("[controller]/get_productos")]
        public async Task<IActionResult> GetData_productos()//int id
        {
            try
            {

                List<ProductoBD> data = new List<ProductoBD>();

                using (MySqlConnection connection = conexion.GetConnection())//var connection = new MySQLConnection(connectionString);
                {
                    await connection.OpenAsync();

                    var sql = "SELECT * FROM tb_productos";// AND tb_productos.id_vendedor = " + id + "
                    using (var command = new MySqlCommand(sql, connection))//MySqlConnection conn = conexion.GetConnection() //var command = new MySqlCommand(sql, connection)
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var id1 = reader.GetInt32(0);
                                var nombre = reader.GetString(1);
                                var url = reader.GetString(2);
                                var precio = reader.GetInt32(3);
                                var descri = reader.GetString(4);
                                var id_ven = reader.GetInt32(5);
                                var categoria_pro = reader.GetString(6);
                                var stok_pro = reader.GetInt32(7);

                                data.Add(new ProductoBD { id_producto = id1, nombre_producto = nombre, url_producto = url, precio_producto = precio, descri_producto = descri,id_vendedor=id_ven, categoria=categoria_pro, stock=stok_pro});
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

        [HttpGet]
        [Route("[controller]/get_one_producto")]
        public async Task<IActionResult> GetData_one_producto(int id_producto)//int id
        {
            try
            {

                List<ProductoBD> data = new List<ProductoBD>();

                using (MySqlConnection connection = conexion.GetConnection())//var connection = new MySQLConnection(connectionString);
                {
                    await connection.OpenAsync();

                    var sql = "SELECT * FROM tb_productos WHERE id_producto="+id_producto+";";// AND tb_productos.id_vendedor = " + id + "
                    using (var command = new MySqlCommand(sql, connection))//MySqlConnection conn = conexion.GetConnection() //var command = new MySqlCommand(sql, connection)
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var id1 = reader.GetInt32(0);
                                var nombre = reader.GetString(1);
                                var url = reader.GetString(2);
                                var precio = reader.GetInt32(3);
                                var descri = reader.GetString(4);
                                var id_ven = reader.GetInt32(5);
                                var categoria_pro = reader.GetString(6);
                                data.Add(new ProductoBD { id_producto = id1, nombre_producto = nombre, url_producto = url, precio_producto = precio, descri_producto = descri, id_vendedor = id_ven, categoria=categoria_pro });
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



        [HttpGet]
        [Route("[controller]/buscador_productos")]
        public async Task<IActionResult> buscador_productos( Pro_Text pro)//int id
        {
            try
            {

                List<ProductoBD> data = new List<ProductoBD>();

                using (MySqlConnection connection = conexion.GetConnection())
                {
                    await connection.OpenAsync();

                    var sql = "SELECT * FROM tb_productos WHERE nombre_producto LIKE '%"+pro.texto+"%';";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var id1 = reader.GetInt32(0);
                                var nombre = reader.GetString(1);
                                var url = reader.GetString(2);
                                var precio = reader.GetInt32(3);
                                var descri = reader.GetString(4);
                                var id_ven = reader.GetInt32(5);
                                var categoria_pro = reader.GetString(6);
                                data.Add(new ProductoBD { id_producto = id1, nombre_producto = nombre, url_producto = url, precio_producto = precio, descri_producto = descri, id_vendedor = id_ven, categoria=categoria_pro });
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


        [HttpPost]
        [Route("[controller]/eliminar_todos_productos")]//modificar producto en tbl_usario LISTO
        public IActionResult eliminarTodosProductosVend([FromBody] int id)//j9kKs9Uaoq
        {
            try
            {
                Productos U = new Productos(conexion);

                U.eliminarTodosProductosVend(id);
                Mensaje msj = new Mensaje("1", "Productos eliminados con exito", "succ");
                return Ok(new { msj });

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        }


        [HttpGet]
        [Route("[controller]/get_productos_vendedor")]
        public async Task<IActionResult> Get_productos_cadaVendedor(int id_vendedor)
        {
            try
            {

                List<ProductoBD> data = new List<ProductoBD>();

                using (MySqlConnection connection = conexion.GetConnection())//var connection = new MySQLConnection(connectionString);
                {
                    await connection.OpenAsync();
                    
                    var sql = "SELECT * FROM tb_productos WHERE id_vendedor=" + id_vendedor + ";";// AND tb_productos.id_vendedor = " + id + "
                    using (var command = new MySqlCommand(sql, connection))//MySqlConnection conn = conexion.GetConnection() //var command = new MySqlCommand(sql, connection)
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var id1 = reader.GetInt32(0);
                                var nombre = reader.GetString(1);
                                var url = reader.GetString(2);
                                var precio = reader.GetInt32(3);
                                var descri = reader.GetString(4);
                                var id_ven = reader.GetInt32(5);
                                var categoria_pro = reader.GetString(6);
                                var stock_pro = reader.GetInt32(7);
                                data.Add(new ProductoBD { id_producto = id1, nombre_producto = nombre, url_producto = url, precio_producto = precio, descri_producto = descri, id_vendedor = id_ven, categoria = categoria_pro, stock =stock_pro});
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
        [Route("[controller]/eliminar_producto")]
        public IActionResult eliminarProducto(int id)
        {
            try
            {
                Productos U = new Productos(conexion);

                U.eliminarProducto(id);
                Mensaje msj = new Mensaje("1", "Producto eliminado con exito", "succ");
                return Ok(new { msj });

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        }


    }

    public class Pro_Text
    {
        public string texto { get; set; }
    }
}
