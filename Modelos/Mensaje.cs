namespace ApiProyecto.Modelos
{
    public class Mensaje
    {

        public string id { get; set; }
        public string mensaje { get; set; }
        public string tipo { get; set; }

        public Mensaje(string idMensaje, string mensaje, string tipo)
        {
            this.id = idMensaje;
            this.mensaje = mensaje;
            this.tipo = tipo;
        }

    }
}
