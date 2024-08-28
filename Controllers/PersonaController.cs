using ApiProyecto.Modelos;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace ApiProyecto.Controllers
{
    public class PersonaController : Controller
    {
        private readonly ConexionBD conexion;

        public PersonaController(ConexionBD cbd)
        {
            this.conexion = cbd;
        }

        [HttpPost]
        [Route("[controller]/insertar_per")]//insertar en tbl_usario LISTO
        public IActionResult insertarPer([FromBody] PersonaBD usu)//j9kKs9Uaoq
        {
            try
            {
                Persona U = new Persona(conexion);

                U.insertarPersona(usu);
                Mensaje msj = new Mensaje("1", "Persona creada con exito", "succ");
                return Ok(new { msj });

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        }

        [HttpGet("controller/get_clients")]
        public async Task<IActionResult> GetData_usuarios()
        {
            try
            {

                List<PersonaBD> data = new List<PersonaBD>();

                using (MySqlConnection connection = conexion.GetConnection())//var connection = new MySQLConnection(connectionString);
                {
                    await connection.OpenAsync();

                    var sql = "SELECT * FROM tb_persona";
                    using (var command = new MySqlCommand(sql, connection))//MySqlConnection conn = conexion.GetConnection() //var command = new MySqlCommand(sql, connection)
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var id = reader.GetInt32(0);
                                var nombre = reader.GetString(1);
                                var ape = reader.GetString(2);
                                var mail = reader.GetString(3);
                                var dire = reader.GetString(4);
                                var tel = reader.GetInt32(5);
                                var sexo = reader.GetString(6);


                                data.Add(new PersonaBD { per_id = id, per_nombre = nombre, per_apellidos = ape, per_correo = mail, per_direccion = dire, per_telefono = tel, per_sexo= sexo});
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
        
        
        [HttpGet("controller/get_clients_one")]
        public async Task<IActionResult> GetData_persona_oneID(int id_persona)
        {
            try
            {

                List<PersonaBD> data = new List<PersonaBD>();

                using (MySqlConnection connection = conexion.GetConnection())//var connection = new MySQLConnection(connectionString);
                {
                    await connection.OpenAsync();

                    var sql = "SELECT * FROM tb_persona WHERE per_id="+id_persona+";";
                    using (var command = new MySqlCommand(sql, connection))//MySqlConnection conn = conexion.GetConnection() //var command = new MySqlCommand(sql, connection)
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var id = reader.GetInt32(0);
                                var nombre = reader.GetString(1);
                                var ape = reader.GetString(2);
                                var mail = reader.GetString(3);
                                var dire = reader.GetString(4);
                                var tel = reader.GetInt32(5);
                                var sexo = reader.GetString(6);


                                data.Add(new PersonaBD { per_id = id, per_nombre = nombre, per_apellidos = ape, per_correo = mail, per_direccion = dire, per_telefono = tel, per_sexo=sexo });
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
        
        [HttpGet("controller/get_mi_nombre")]
        public async Task<IActionResult> GetData_mi_nombre(int id_persona)
        {
            try
            {

                List<PersonaNomnreBD> data = new List<PersonaNomnreBD>();

                using (MySqlConnection connection = conexion.GetConnection())//var connection = new MySQLConnection(connectionString);
                {
                    await connection.OpenAsync();

                    var sql = "SELECT per_nombre, per_correo FROM tb_persona WHERE per_id=" + id_persona+";";
                    using (var command = new MySqlCommand(sql, connection))//MySqlConnection conn = conexion.GetConnection() //var command = new MySqlCommand(sql, connection)
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                
                                var nombre = reader.GetString(0);
                                var correo = reader.GetString(1);



                                data.Add(new PersonaNomnreBD { per_nombre = nombre,per_correo = correo });
                            }
                        }
                    }
                    await connection.CloseAsync();
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        [HttpPut]
        [Route("Persona/modificar_persona")]
        public IActionResult modificarPersona([FromBody] PersonaBD usu)
        {



            try
            {
                Persona P = new Persona(conexion);



                P.modificarPersona(usu);
                Mensaje msj = new Mensaje("1", "Persona actualizada con exito", "succ");
                return Ok(new { msj });





            }
            catch (Exception ei)
            {
                Mensaje msj = new Mensaje("-1", ei.Message, "error");
                return BadRequest(new { msj });



            }



        }

    }
}
