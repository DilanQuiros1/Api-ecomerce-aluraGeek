using ApiProyecto.Modelos;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using static ApiProyecto.Modelos.Usuario;

namespace ApiProyecto.Controllers
{
    public class VenController : Controller
    {
        private readonly ConexionBD conexion;

        public VenController(ConexionBD cbd)
        {
            this.conexion = cbd;
        }

        [HttpPost]
        [Route("[controller]/insertar_vendedor")]//insertar en tbl_usario LISTO
        public IActionResult insertarVendedor([FromBody] VendedorBD ven)//j9kKs9Uaoq
        {
            try
            {
                Vendedor v = new Vendedor(conexion);
                Crypto cp = new Crypto();
                string sal = cp.createRandomSalt();
                string contra = ven.ven_password;//cp.createRamdomPassword(10);
                string hash = cp.GetHash(sal, contra);
                ven.ven_password = hash;
                ven.ven_sal = sal;
                v.insertarVendedor(ven);
                Mensaje msj = new Mensaje("1", "Vendedor creada con exito su contraseña es: "+contra, "succ");
                return Ok(new { msj });

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        }


        [HttpPost]
        [Route("[controller]/validar_vendedor")]  
        public IActionResult validarUsuario(string idVendedor, string contrasena)
        {

            try
            {
                Mensaje msj;
                Vendedor v = new Vendedor(conexion);
                switch (v.validarVendedor(idVendedor, contrasena))
                {
                    case 0:
                        msj = new Mensaje("0", "Vendedor no existe", "info");
                        return Ok(new { msj });
                    case -1:
                        msj = new Mensaje("-1", "La contraseña es incorrecta", "info");
                        return Ok(new { msj });
                    case 1:
                        msj = new Mensaje("1", "Todo correcto puede iniciar sesión", "succ");
                        return Ok(new { msj });

                    default:
                        msj = new Mensaje("-2", "Error desconocido al iniciar sesión", "error");
                        return Ok(new { msj });

                }
            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-3", e.Message, "error");
                return BadRequest(new { msj });
            }
        }



        [HttpGet("controller/get_vendedores_de_persona")]
        public async Task<IActionResult> GetData_vendedores_persona(int id_persona)
        {
            try
            {

                List<VendedorBD> data = new List<VendedorBD>();

                using (MySqlConnection connection = conexion.GetConnection())//var connection = new MySQLConnection(connectionString);
                {
                    await connection.OpenAsync();

                    var sql = "SELECT id_ven, nombre_ven, ven_telefono FROM tb_vendedor JOIN tb_persona ON tb_vendedor.id_persona = tb_persona.per_id AND tb_vendedor.id_persona="+id_persona+";";
                    using (var command = new MySqlCommand(sql, connection))//MySqlConnection conn = conexion.GetConnection() //var command = new MySqlCommand(sql, connection)
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var id_ven = reader.GetString(0);
                                var nombre = reader.GetString(1);
                                var telefono = reader.GetString(2);



                                data.Add(new VendedorBD { id_ven = id_ven, ven_nombre = nombre, ven_telefono=telefono });
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

        [HttpPut]
        [Route("Vendedor/modificar_vendor")]
        public IActionResult modificarNombreVendedor([FromBody] VendedorBD_Editar ven)
        {
            try
            {
                Vendedor V = new Vendedor(conexion);



                V.modificarVendedor(ven);
                Mensaje msj = new Mensaje("1", "Nombre de Vendedor modificado con exito", "succ");
                return Ok(new { msj });
            }
            catch (Exception ex)
            {
                Mensaje msj = new Mensaje("-1", ex.Message, "error");
                return BadRequest(new { msj });
            }
        }



        [HttpGet("controller/get_vendedores_count")]
        public async Task<IActionResult> GetData_persona_id_perfil(int id_persona)
        {
            try
            {

                List<Usuario_Count_users> data = new List<Usuario_Count_users>();

                using (MySqlConnection connection = conexion.GetConnection())//var connection = new MySQLConnection(connectionString);
                {
                    await connection.OpenAsync();

                    var sql = "SELECT COUNT(id_ven) FROM tb_vendedor WHERE id_persona =" + id_persona + ";";
                    using (var command = new MySqlCommand(sql, connection))//MySqlConnection conn = conexion.GetConnection() //var command = new MySqlCommand(sql, connection)
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {

                                var cantidad_user = reader.GetString(0);


                                data.Add(new Usuario_Count_users { cantidad = cantidad_user });
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

        //Eliminar Vendedor con sus Productos---------------------------------
        //********************


        [HttpDelete]
        [Route("[controller]/eliminar_Vendedor_productos")]
        public IActionResult eliminarVendedor(int id)
        {
            try
            {
                Productos U = new Productos(conexion);
                U.eliminarTodosProductosVend(id);
                Vendedor V = new Vendedor(conexion);
                V.eliminarVendedor(id);
                Mensaje msj = new Mensaje("1", "Vendedor eliminado con exito", "succ");
                return Ok(new { msj });

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        }


        //Eliminar Vendedor con sus Productos---------------------------------
        //


    }
}
