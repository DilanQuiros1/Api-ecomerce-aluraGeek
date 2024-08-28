using ApiProyecto.Modelos;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using static ApiProyecto.Modelos.Usuario;

namespace ApiProyecto.Controllers
{
    public class Token_passwordController : Controller
    {
        private readonly ConexionBD conexion;

        public Token_passwordController(ConexionBD cbd)
        {
            this.conexion = cbd;
        }


        [HttpPost]
        [Route("[controller]/modificar_password")]//insertar en tbl_usario LISTO https://localhost:7262/Token_password/modificar_password
        public IActionResult modificar_password([FromBody] TokenBD tok)//j9kKs9Uaoq
        {
            try
            {
                Mensaje msj;
                Token token = new Token(conexion);
                //token.validarToken(tok.usu_correo, tok.newPassword);
                switch (token.validarToken(tok.usu_correo, tok.newPassword, tok.tok_token))
                {

                    case 0:
                        msj = new Mensaje("0", "Token no valido", "info");
                        return BadRequest(new { msj });
                    case 1:
                        token.deshabilitar_token(tok.tok_token);
                        msj = new Mensaje("1", "Se edito de forma correcta su password", "succ");
                        return Ok(new { msj });

                    default:
                        msj = new Mensaje("-1", "Error desconocido al validar token", "error");
                        return BadRequest(new { msj });

                }

            }
            catch (Exception e)
            {
                Mensaje msj = new Mensaje("-1", e.Message, "error");
                return BadRequest(new { msj });
            }
        }


    }

    public class TokenBD
    {
        public string usu_correo { get; set; }
        public string newPassword { get; set; }
        public string tok_token { get; set; }
    }
}
