using Microsoft.Win32.SafeHandles;
using MySql.Data.MySqlClient;
using System.Web.Http;

namespace ApiProyecto.Modelos
{
    public class carrito
    {
        private ConexionBD conexion;
        public carrito(ConexionBD cBD)
        {
            this.conexion = cBD;
        }

        public int insertar_pro_carrito(CarritoBD carrito)//LISTO
        {

            try
            {//binding de parametros

                using (MySqlConnection conn = conexion.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM tb_carrito WHERE id_persona = " +
                        carrito.id_persona+" AND id_producto = " + carrito.id_producto + ";", conn);
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        //tiene filas, tiene producto
                        conn.Close();
                        return -1;

                    }
                    else
                    {
                        conn.Close();
                        conn.Open();
                        MySqlCommand comm = conn.CreateCommand();

                        comm.CommandText = "INSERT INTO tb_carrito (id_persona,id_producto,id_vendedor,cantidad) VALUES (@id_persona,  @id_producto, @id_vendedor, @cantidad);";
                        comm.Parameters.AddWithValue("@id_persona", carrito.id_persona);
                        comm.Parameters.AddWithValue("@id_producto", carrito.id_producto);
                        comm.Parameters.AddWithValue("@id_vendedor", carrito.id_vendedor);
                        comm.Parameters.AddWithValue("@cantidad", carrito.cantidad_producto);

                        comm.ExecuteNonQuery();
                        conn.Close();
                        return 1;
                    }
                   

                    /*conn.Open();
                    MySqlCommand comm = conn.CreateCommand();
                   
                    comm.CommandText = "INSERT INTO tb_carrito (id_persona,id_producto,id_vendedor,cantidad) VALUES (@id_persona,  @id_producto, @id_vendedor, @cantidad);";
                    comm.Parameters.AddWithValue("@id_persona", carrito.id_persona);
                    comm.Parameters.AddWithValue("@id_producto", carrito.id_producto);
                    comm.Parameters.AddWithValue("@id_vendedor", carrito.id_vendedor);
                    comm.Parameters.AddWithValue("@cantidad", carrito.cantidad_producto);

                    comm.ExecuteNonQuery();
                    conn.Close();
                    return 1;*/
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public int vaciarCarrito(int per)//LISTO
        {

            try
            {//binding de parametros

                using (MySqlConnection conn = conexion.GetConnection())
                {
                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();

                    comm.CommandText = "DELETE FROM tb_carrito WHERE id_persona = @per;";
                    comm.Parameters.AddWithValue("@per", per);
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


        public int borrarProdCarrito(int id, int per)//LISTO
        {

            try
            {//binding de parametros

                using (MySqlConnection conn = conexion.GetConnection())
                {
                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();

                    comm.CommandText = "DELETE FROM tb_carrito WHERE id_producto = @id AND id_persona = @per;";
                    comm.Parameters.AddWithValue("@id", id);
                    comm.Parameters.AddWithValue("@per", per);
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

    public class CarritoBD
    {
        
        public string id_persona { get; set; }
        public int id_vendedor { get; set; }
        public int id_producto { get; set; }
     
        public int cantidad_producto { get; set; }
    }
    
    public class CarritovalidaBD
    {
        
        public int id_producto { get; set; }

    }

}
