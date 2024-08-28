using MySql.Data.MySqlClient;
using System.Text;

namespace ApiProyecto.Modelos
{
    public class Token
    {

        private ConexionBD conexion;
        public Token(ConexionBD cBD)
        {
            this.conexion = cBD;
        }

        public int insertar_token(string usu_token, string tok_token)//LISTO
        {

            try
            {

                using (MySqlConnection conn = conexion.GetConnection())
                {
                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();

                    comm.CommandText = "INSERT INTO tb_token (usu_token,tok_token, estado_token, fechaex_token) " +
                        "VALUES (@usu_correo, @tok_token,'A' , DATE_ADD(NOW(), INTERVAL 24 HOUR));";
                    comm.Parameters.AddWithValue("@usu_correo", usu_token);
                    comm.Parameters.AddWithValue("@tok_token", tok_token);

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

        public string GenerateRandomString(int length)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(0, allowedChars.Length);
                char randomChar = allowedChars[randomIndex];
                stringBuilder.Append(randomChar);
            }

            return stringBuilder.ToString();
        }


        public int validarToken(string usu_correo, string newPassword, string tok_token)// LISTO
        {
            using (MySqlConnection conn = conexion.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM tb_token WHERE usu_token ='" +
                usu_correo + "' AND tok_token = '"+tok_token+"' AND IF(fechaex_token > NOW(), TRUE, FALSE) AND estado_token ='A';", conn);

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)//Si es valido
                {
                    //POST editar password
                    Usuario U = new Usuario(conexion);
                    Crypto cp = new Crypto();

                    string sal = cp.createRandomSalt();
                    string contra = newPassword;//cp.createRamdomPassword(10);
                    string hash = cp.GetHash(sal, contra);
                    newPassword = hash;
                    string newSal = sal;
                    U.modificarpassword(usu_correo, newPassword, newSal);
                   
                    conn.Close();
                    return 1;
                }
                else
                {
                    // Su token no es valido
                    conn.Close();
                    return 0;

                }

            };
        }

        public int deshabilitar_token(string token)
        {

            try
            {
                using (MySqlConnection conn = conexion.GetConnection())
                {

                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();
                    comm.CommandText = "UPDATE tb_token SET estado_token = 'I' WHERE tok_token = @token_id;";
                    comm.Parameters.AddWithValue("@token_id", token);
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
    
    public class TokenBD
    {
        public string usu_token { get; set; }
        public string tok_token { get; set; }
        public string estado_token { get; set; }

    }
}
