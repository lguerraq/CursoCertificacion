using System;
using System.Configuration;

namespace DBR.Eventos.Presentacion
{
    public class Configuracion
    {
        public static string NameEmailRobot
        {
            get
            {
                return Convert.ToString(ConfigurationManager.AppSettings.Get("NameEmailRobot"));
            }
        }
        public static string EmailRobot
        {
            get
            {
                return Convert.ToString(ConfigurationManager.AppSettings.Get("EmailRobot"));
            }
        }
        public static string NameEmail
        {
            get
            {
                return Convert.ToString(ConfigurationManager.AppSettings.Get("NameEmail"));
            }
        }
        public static string Email
        {
            get
            {
                return Convert.ToString(ConfigurationManager.AppSettings.Get("Email"));
            }
        }
        public static string userName
        {
            get
            {
                return Convert.ToString(ConfigurationManager.AppSettings.Get("userName"));
            }
        }
        public static string password
        {
            get
            {
                return Convert.ToString(ConfigurationManager.AppSettings.Get("password"));
            }
        }
        public static string host
        {
            get
            {
                return Convert.ToString(ConfigurationManager.AppSettings.Get("host"));
            }
        }
        public static int port
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("port"));
            }
        }
        public static bool EnableSsl
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EnableSsl"));
            }
        }
        public static string NumeroRelease
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("NumeroRelease").ToString();
            }
        }
        public static double DiferenciaZona
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("DiferenciaZona").ToString());
            }
        }
        public static int CodigoEmpresa
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("CodigoEmpresa").ToString());
            }
        }
        public static string urlFileServer
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("urlFileServer").ToString();
            }
        }
        public static string urlFileServerVer
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("urlFileServerVer").ToString();
            }
        }
        public static string reCapchaKeyWeb
        {
            get
            {
                return Convert.ToString(ConfigurationManager.AppSettings.Get("reCapchaKeyWeb"));
            }
        }
        public static string reCapchaKeySecret
        {
            get
            {
                return Convert.ToString(ConfigurationManager.AppSettings.Get("reCapchaKeySecret"));
            }
        }
        public static string TokenEncriptado
        {
            get
            {
                return "7kjr14c47f104415";
            }

        }
    }
}