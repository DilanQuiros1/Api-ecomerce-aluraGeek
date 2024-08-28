using MySql.Data.MySqlClient;

namespace ApiProyecto.Modelos
{
    public class Productos
    {

        private ConexionBD conexion;
        public Productos(ConexionBD cBD)
        {
            this.conexion = cBD;
        }

        public int insertarProducto(ProductoBD pro)//LISTO
        {

            try
            {//binding de parametros

                using (MySqlConnection conn = conexion.GetConnection())
                {
                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();//INSERT INTO tb_productos (id_producto, nombre_producto, url_producto, precio_producto, descri_producto,id_vendedor) VALUES (@id_producto, @nombre_producto,@url_producto, @precio_producto, @descri_producto, @id_vendedor);
                    //INSERT INTO tb_productos (id_producto,nombre_producto,url_producto,precio_producto,descri_producto,id_vendedor) VALUES (5,'NOM','IMG',100,'DES',1);
                    comm.CommandText = "INSERT INTO tb_productos (id_producto, nombre_producto, url_producto, precio_producto, descri_producto,id_vendedor, categoria, stock) VALUES (@id_producto, @nombre_producto,@url_producto, @precio_producto, @descri_producto, @id_vendedor, @categoria, @stock);";
                    comm.Parameters.AddWithValue("@id_producto", pro.id_producto);
                    comm.Parameters.AddWithValue("@nombre_producto", pro.nombre_producto);
                    comm.Parameters.AddWithValue("@url_producto", pro.url_producto);
                    comm.Parameters.AddWithValue("@precio_producto", pro.precio_producto);
                    comm.Parameters.AddWithValue("@descri_producto", pro.descri_producto);
                    comm.Parameters.AddWithValue("@id_vendedor", pro.id_vendedor);
                    comm.Parameters.AddWithValue("@categoria", pro.categoria);
                    comm.Parameters.AddWithValue("@stock", pro.stock);
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


        //********************
        //Eliminar Todos Productos Vendedor ----------------------------
        public int eliminarTodosProductosVend(int id)
        {


            try
            {
                using (MySqlConnection conn = conexion.GetConnection())
                {


                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();
                    comm.CommandText = "DELETE FROM tb_carrito WHERE id_vendedor = @id_vendedor";
                    comm.Parameters.AddWithValue("@id_vendedor", id);
                    comm.ExecuteNonQuery();
                    conn.Close();

                    conn.Open();
                    MySqlCommand commaux = conn.CreateCommand();
                    commaux.CommandText = "DELETE FROM tb_productos WHERE id_vendedor = @id_vendedor";
                    commaux.Parameters.AddWithValue("@id_vendedor", id);
                    commaux.ExecuteNonQuery();
                    conn.Close();
                    return 1;
                }
            }
            catch (Exception e)
            {

                return -1;
            }
        }


        public int eliminarProducto(int id)
        {


            try
            {
                using (MySqlConnection conn = conexion.GetConnection())
                {


                    conn.Open();
                    MySqlCommand comm = conn.CreateCommand();
                    comm.CommandText = "DELETE FROM tb_carrito WHERE id_producto = @id_producto";
                    comm.Parameters.AddWithValue("@id_producto", id);
                    comm.ExecuteNonQuery();
                    conn.Close();

                    conn.Open();
                    MySqlCommand comAux = conn.CreateCommand();
                    comAux.CommandText = "DELETE FROM tb_productos WHERE id_producto = @id_producto";
                    comAux.Parameters.AddWithValue("@id_producto", id);
                    comAux.ExecuteNonQuery();
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

    public class ProductoBD
    {
        public int id_producto { get; set; }
        public string nombre_producto { get; set; }
        public string url_producto { get; set; }

        public int precio_producto { get; set; }
        public string descri_producto { get; set; }
        public int id_vendedor { get; set; }
        public string categoria { get; set; }
        public int stock { get; set; }
    } 
    public class ProductoBDcarrito
    {
        public int id_producto { get; set; }
        public string nombre_producto { get; set; }
        public string url_producto { get; set; }
        public int precio_producto { get; set; }
        public string categoria { get; set; }
        public string telefono_persona { get; set; }
        public string ven_telefono { get; set; }
        public string cantidad { get; set; }
    }

}
