using MySql.Data.MySqlClient;

namespace ApiProyecto.Modelos
{
    public class ConexionBD
    {

        public string ConnectionString { get; set; }


        public ConexionBD(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

    }
}
