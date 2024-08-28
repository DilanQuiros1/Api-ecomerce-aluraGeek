using MySql.Data.MySqlClient;

namespace ApiProyecto.Modelos
{
    public class Vendedor
    {

        private ConexionBD conexion;
        public Vendedor(ConexionBD cBD)
        {
            this.conexion = cBD;
        }

        public int validarVendedor(string id_ven, string contra)// 10 1rYOk31dmr LISTO
        {
            using (MySqlConnection conn = conexion.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tb_vendedor where id_ven='" + id_ven + "';", conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    var contraBD = reader["ven_password"].ToString();
                    var sal = reader["ven_sal"].ToString();
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

        public int insertarVendedor(VendedorBD ven)
        {

            try
            {//binding de parametros

                using (MySqlConnection conn = conexion.GetConnection())
                {
                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();

                    comm.CommandText = "INSERT INTO tb_vendedor (id_ven,nombre_ven,ven_password, ven_sal, id_persona, ven_telefono) VALUES (@id_ven,@ven_nombre, @ven_password, @ven_sal, @id_persona, @telefono);";
                    comm.Parameters.AddWithValue("@id_ven", ven.id_ven);
                    comm.Parameters.AddWithValue("@ven_nombre", ven.ven_nombre);
                    comm.Parameters.AddWithValue("@ven_password", ven.ven_password);
                    comm.Parameters.AddWithValue("@telefono", ven.ven_telefono);
                    comm.Parameters.AddWithValue("@ven_sal", ven.ven_sal);
                    comm.Parameters.AddWithValue("@id_persona", ven.persona_id);
                    comm.ExecuteNonQuery();
                    conn.Close();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }

        }//LISTO AQUI AGREGAR COLUMNA DE IDPERSONA (FK)

        //VER UN VENDEDOR SEGUN ID USUARIO   1 
        public int modificarVendedor(VendedorBD_Editar ven)
        {



            try
            {
                using (MySqlConnection conn = conexion.GetConnection())
                {



                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();
                    comm.CommandText = "Update tb_vendedor SET nombre_ven = @nombre_ven, ven_telefono =@telefono WHERE id_ven = @id_ven";



                    comm.Parameters.AddWithValue("@id_ven", ven.id_ven);
                    comm.Parameters.AddWithValue("@nombre_ven", ven.ven_nombre);
                    comm.Parameters.AddWithValue("@telefono", ven.telefono);

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


        public int eliminarVendedor(int id)
        {

            try
            {
                using (MySqlConnection conn = conexion.GetConnection())
                {


                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();
                    comm.CommandText = "DELETE FROM tb_vendedor WHERE id_ven = @id_ven";
                    comm.Parameters.AddWithValue("@id_ven", id);
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

    }

    public class VendedorBD
    {
        public string id_ven { get; set; }
        public string ven_nombre { get; set; }

        public string ven_password { get; set; }
        public string ven_telefono { get; set; }
        public string ven_sal { get; set; }
        public string persona_id { get; set; }
    } 
    public class VendedorBD_Editar
    {
        public string id_ven { get; set; }
        public string ven_nombre { get; set; }
        public string telefono { get; set; }

    }

}
