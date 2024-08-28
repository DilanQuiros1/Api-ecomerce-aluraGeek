using ApiProyecto.utilidades;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SendGrid;
using System.Net.Mail;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiProyecto.Modelos
{
    public class Usuario
    {

        private ConexionBD conexion;
        public Usuario(ConexionBD cBD)
        {
            this.conexion = cBD;
        }
        public int validarUsuario(string idUsuario, string contra)// LISTO
        {
            using (MySqlConnection conn = conexion.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tb_usuario where usu_correo='" + idUsuario + "';", conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    var contraBD = reader["usu_password"].ToString();
                    var sal = reader["usu_sal"].ToString();
                    Crypto cp = new Crypto();

                    var hashContra = cp.GetHash(sal, contra);

                    if (contraBD == hashContra)//0LwThZqkdL
                    {
                        // el usuario existe y la contraseña es correcta
                        return 1;
                    }
                    else
                    {
                        // la contraseña digitada es incorrecta
                        return -1;
                    }
                }
                else
                {
                    // El usuario no existe
                    return 0;
                }
            };
        }

        public int insertarUsuario(UsuarioBD usuario)//LISTO
        {

            try
            {//binding de parametros

                using (MySqlConnection conn = conexion.GetConnection())
                {
                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();

                    comm.CommandText = "INSERT INTO tb_usuario (usu_correo,usu_persona,usu_password,usu_sal) VALUES (@usu_correo, @usu_persona,@usu_password, @usu_sal);";
                    comm.Parameters.AddWithValue("@usu_correo", usuario.idUsuario);
                    comm.Parameters.AddWithValue("@usu_persona", usuario.idPersona_foranea);
                    comm.Parameters.AddWithValue("@usu_password", usuario.password);
                    comm.Parameters.AddWithValue("@usu_sal", usuario.sal);
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


        public int eliminarUsuario(string id_usu)
        {

            try
            {
                using (MySqlConnection conn = conexion.GetConnection())
                {


                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();
                    comm.CommandText = "DELETE FROM tb_usuario WHERE usu_correo = @usu_correo";
                    comm.Parameters.AddWithValue("@usu_correo", id_usu);
                    comm.ExecuteNonQuery();
                    conn.Close();
                    return 1;
                }
            }
            catch (Exception e)
            {
                return -1;
            }

        }


        public int modificarpassword(string usucorreo, string password, string sal)
        {



            try
            {
                using (MySqlConnection conn = conexion.GetConnection())
                {



                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();
                    comm.CommandText = "UPDATE tb_usuario SET usu_password = @newPassword, usu_sal=@newSal WHERE usu_correo = @usu_correo;";

                    comm.Parameters.AddWithValue("@newPassword", password);
                    comm.Parameters.AddWithValue("@newSal", sal);
                    comm.Parameters.AddWithValue("@usu_correo", usucorreo);

                    comm.ExecuteNonQuery();
                    conn.Close();
                    return 1;
                }
            }
            catch (Exception e)
            {



                return -1;
            }



        }


        public class UsuarioBD
        {
            public string idPersona_foranea { get; set; }
            public string idUsuario { get; set; }

            public string password { get; set; }
            public string sal { get; set; }
        }

        public class Usuario_Count_users
        {
            public string cantidad { get; set; }
        }

    }

}
