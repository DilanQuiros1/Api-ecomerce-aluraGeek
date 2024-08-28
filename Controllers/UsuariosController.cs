using ApiProyecto.Modelos;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SendGrid.Helpers.Mail;
using SendGrid;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static ApiProyecto.Modelos.Usuario;
using ApiProyecto.utilidades;

namespace ApiProyecto.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ConexionBD conexion;

        public UsuariosController(ConexionBD cbd)
        {
            this.conexion = cbd;
        } 


        [HttpPost]
        [Route("[controller]/validar")]//login LISTO  admin100 hxBdGHEnsc para pruebas
        public IActionResult validarUsuario(string idUsuario, string contrasena)
        {

            try
            {
                Mensaje msj;
                Usuario u = new Usuario(conexion);
                switch (u.validarUsuario(idUsuario, contrasena))
                {
                    case 0:
                        msj = new Mensaje("0", "Usuario no existe", "info");
                        return Ok(new { msj });
                    case -1:
                        msj = new Mensaje("-1", "La contraseña es incorrecta1", "info");
                        return Ok(new { msj });
                    case 1:
                        msj = new Mensaje("1", "Todo correcto puede iniciar sesión1", "succ");
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



        [HttpPost]
        [Route("[controller]/guardar")]//insertar en tbl_usario LISTO
        public IActionResult insertarUsuario([FromBody] UsuarioBD usu)//j9kKs9Uaoq
        {
            try
            {
                Usuario U = new Usuario(conexion);
                Crypto cp = new Crypto();
              
                string sal = cp.createRandomSalt();
                string contra = usu.password;//cp.createRamdomPassword(10);
                string hash = cp.GetHash(sal, contra);
                usu.password = hash;
                usu.sal = sal;
                U.insertarUsuario(usu);
                Mensaje msj = new Mensaje("1", "Usuario creado con exito. La nueva contra es: " + contra, "succ");
                return Ok(new { msj });

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        }


        [HttpPost]
        [Route("[controller]/guardar_token")]// https://localhost:7262/Usuarios/guardar_token
        public async System.Threading.Tasks.Task<IActionResult> token_usuario([FromBody] CorreoBD Correo)//j9kKs9Uaoq
        {
            try
            {
                Token token = new Token(conexion);
                Mailer m1 = new Mailer();

                string tok_token = token.GenerateRandomString(10);
                string nombre="";
                await m1.enviarEmailNuevoPasswd(Correo.correo, nombre, tok_token);
                token.insertar_token(Correo.correo, tok_token);
                //INSERTAR TOKEN EN LA 
                Mensaje msj = new Mensaje("1", "Se envio Email", "succ");
                return Ok(new { msj });

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        }
        
        [HttpPost]
        [Route("[controller]/envio_reporte_correo")]
        public async System.Threading.Tasks.Task<IActionResult> envio_correo([FromBody] Correo_reporteBD Correo)//j9kKs9Uaoq
        {
            try
            {
               
                Mailer m1 = new Mailer();//https://localhost:7262/Usuarios/envio_reporte_correo
                await m1.envio_correo("quiroschinchilladilan@gmail.com", Correo.nombre, Correo.msj);
               
                Mensaje msj = new Mensaje("1", "Se envio Email de reporte", "succ");
                return Ok(new { msj });

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        } 
        
        [HttpPost]
        [Route("[controller]/envio_compra_correo")]
        public async System.Threading.Tasks.Task<IActionResult> envio_correo_compra([FromBody] Correo_compraBD Correo)//j9kKs9Uaoq
        {
            try
            {
               
                Mailer m1 = new Mailer();//https://localhost:7262/Usuarios/envio_compra_correo

                await m1.envio_correo_compra(Correo.email, Correo.nombre_pro, Correo.precio, Correo.cantidad);
               
                Mensaje msj = new Mensaje("1", "Se envio Email de reporte", "succ");
                return Ok(new { msj });

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        }
        
  

        [HttpGet("controller/get_usuarios_persona")]
        public async Task<IActionResult> GetData_usuarios_persona(int id_persona)
        {
            try
            {

                List<UsuarioBD> data = new List<UsuarioBD>();

                using (MySqlConnection connection = conexion.GetConnection())//var connection = new MySQLConnection(connectionString);
                {
                    await connection.OpenAsync();

                    var sql = "SELECT usu_correo, tb_persona.per_id FROM tb_usuario JOIN tb_persona ON tb_usuario.usu_persona = tb_persona.per_id AND tb_usuario.usu_persona="+id_persona+";";
                    using (var command = new MySqlCommand(sql, connection))//MySqlConnection conn = conexion.GetConnection() //var command = new MySqlCommand(sql, connection)
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var correo = reader.GetString(0);
                                var persona_id = reader.GetString(1);


                                data.Add(new UsuarioBD { idUsuario = correo, idPersona_foranea = persona_id});
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


        [HttpGet("controller/get_persona_id")]
        public async Task<IActionResult> GetData_persona_id_perfil(string usu_correo)
        {
            try
            {

                List<UsuarioBD> data = new List<UsuarioBD>();

                using (MySqlConnection connection = conexion.GetConnection())//var connection = new MySQLConnection(connectionString);
                {
                    await connection.OpenAsync();

                    var sql = "SELECT usu_persona FROM tb_usuario WHERE usu_correo='" + usu_correo + "';";
                    using (var command = new MySqlCommand(sql, connection))//MySqlConnection conn = conexion.GetConnection() //var command = new MySqlCommand(sql, connection)
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                
                                var persona_id = reader.GetString(0);


                                data.Add(new UsuarioBD { idPersona_foranea = persona_id});
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

        [HttpGet("controller/get_users_count")]
        public async Task<IActionResult> GetData_persona_id_perfil(int id_persona)
        {
            try
            {

                List<Usuario_Count_users> data = new List<Usuario_Count_users>();

                using (MySqlConnection connection = conexion.GetConnection())//var connection = new MySQLConnection(connectionString);
                {
                    await connection.OpenAsync();

                    var sql = "SELECT COUNT(usu_correo) FROM tb_usuario WHERE usu_persona = " + id_persona + ";";
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


        [HttpDelete]
        [Route("[controller]/eliminar_Usuarios")]//modificar producto en tbl_usario LISTO
        public IActionResult eliminarVendedor(string correo)//j9kKs9Uaoq
        {
            try
            {

                Usuario V = new Usuario(conexion);
                V.eliminarUsuario(correo);
                Mensaje msj = new Mensaje("1", "Usuario eliminado con exito", "succ");
                return Ok(new { msj });

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        }






       
    

    }


    public class CorreoBD
    {
        public string correo { get; set; }
    } 
    
    public class Correo_reporteBD
    {
        public string nombre { get; set; }
        public string msj { get; set; }
    }
    
    public class Correo_compraBD
    {
        public string email { get; set; }
        public string nombre_pro { get; set; }
        public string precio { get; set; }
        public string cantidad { get; set; }
    }
    
   

}

