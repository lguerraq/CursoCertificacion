using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    public class Program
    {
        static void Main(string[] args)
        {
        }

        public static bool ingresoAlsistema(String usuario, string password) =>

            usuario == "admin" && password == "Diego@123" ? true : false;

        public static string EnviarCertificadoEmail(string correo)
        {

            correo = "jhuamanb10@gmail.com";
            string mensaje = "";
            if (correo.Length < 0)
            {
                return mensaje = "Correo no ingresado";
            }
            else { return mensaje = "Enviado"; }

            

        }

        public static string consultaCertificado(string usuario, string curso) {



            if (usuario.Equals("47204456") && curso.Equals("MODULO 1 - DISEÑO DEL PLAN DE SALUD MENTAL"))
            {

                return "Consulta correcta";

            }
            else {
                return "Error";
            }
        }

        public static string descargaCertificado(string usuario, string curso)
        {



            if (usuario.Equals("47204456") && curso.Equals("MODULO 1 - DISEÑO DEL PLAN DE SALUD MENTAL"))
            {

                return "Descarga correcta";

            }
            else
            {
                return "Error";
            }
        }

    }



    



}
