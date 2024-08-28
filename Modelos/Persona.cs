using MySql.Data.MySqlClient;

namespace ApiProyecto.Modelos
{
    public class Persona
    {
        private ConexionBD conexion;
        public Persona(ConexionBD cBD)
        {
            this.conexion = cBD;
        }

        public int insertarPersona(PersonaBD usuario)//LISTO
        {

            try
            {//binding de parametros

                using (MySqlConnection conn = conexion.GetConnection())
                {
                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();

                    comm.CommandText = "INSERT INTO tb_persona (per_id,per_nombre,per_apellidos,per_correo,per_direccion,per_telefono, sexo) VALUES (@per_id, @nombre,@apellidos, @correo, @direccion, @tel, @sexo);";
                    comm.Parameters.AddWithValue("@per_id", usuario.per_id);
                    comm.Parameters.AddWithValue("@nombre", usuario.per_nombre);
                    comm.Parameters.AddWithValue("@apellidos", usuario.per_apellidos);
                    comm.Parameters.AddWithValue("@correo", usuario.per_correo);
                    comm.Parameters.AddWithValue("@direccion", usuario.per_direccion);
                    comm.Parameters.AddWithValue("@tel", usuario.per_telefono);
                    comm.Parameters.AddWithValue("@sexo", usuario.per_sexo);
                    comm.ExecuteNonQuery();
                    conn.Close();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public int GetData_Persona()//LISTO
        {
            try
            {

                List<PersonaBD> data = new List<PersonaBD>();

                using (MySqlConnection conn = conexion.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("select * from tb_persona;", conn);
                    var reader = cmd.ExecuteReader();

                    while (reader.HasRows)
                    {
                        var id = reader.GetInt32(0);
                        var nombre = reader.GetString(1);
                        var ape = reader.GetString(2);
                        var mail = reader.GetString(4);
                        int tel = reader.GetInt32(5);
                        var dire = reader.GetString(6);
                        var per_sexo = reader.GetString(6);

                        data.Add(new PersonaBD { per_id = id, per_nombre = nombre, per_apellidos = ape, per_correo = mail, per_direccion = dire, per_telefono = tel, per_sexo= per_sexo});
                    }

                    conn.CloseAsync();//lo agregue
                    return 1;
                }


            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        //TRAER PARA UNA SOLA PERSONA.
        public int modificarPersona(PersonaBD usu)
        {



            try
            {
                using (MySqlConnection conn = conexion.GetConnection())
                {



                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();
                    comm.CommandText = "Update tb_persona SET per_nombre = @per_nombre, per_apellidos = @per_apellidos, per_correo = @per_correo, per_direccion = @per_direccion, per_telefono = @per_telefono, sexo = @per_sexo WHERE per_id = @per_id";



                    comm.Parameters.AddWithValue("@per_id", usu.per_id);
                    comm.Parameters.AddWithValue("@per_nombre", usu.per_nombre);
                    comm.Parameters.AddWithValue("@per_apellidos", usu.per_apellidos);
                    comm.Parameters.AddWithValue("@per_correo", usu.per_correo);
                    comm.Parameters.AddWithValue("@per_direccion", usu.per_direccion);
                    comm.Parameters.AddWithValue("@per_telefono", usu.per_telefono);
                    comm.Parameters.AddWithValue("@per_sexo", usu.per_sexo);
                    comm.ExecuteNonQuery();
                    conn.Close();
                    return 1;
                }



            }
            catch (Exception ex)
            {



                return -1;



            }



        }

    }

    public class PersonaBD
    {
        public int per_id { get; set; }
        public string per_nombre { get; set; }

        public string per_apellidos { get; set; }

        public string per_correo { get; set; }

        public string per_direccion { get; set; }

        public int per_telefono { get; set; }
        public string per_sexo { get; set; }
    }
    
    public class PersonaNomnreBD
    {
        public string per_nombre { get; set; }
        public string per_correo { get; set; }

    }
}
