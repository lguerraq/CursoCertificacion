using System;
using System.IO;
using System.Text;

namespace DBR.Eventos.Presentacion
{
    public class CapturaErrores
    {
        public CapturaErrores()
        {

        }
        public void WriteLog(string msg, string rutaLog)
        {
            try
            {
                if (!System.IO.Directory.Exists(rutaLog))
                {
                    System.IO.Directory.CreateDirectory(rutaLog);
                }
                using (FileStream fs = new FileStream(rutaLog + "/" + DateTime.Now.ToString("ddMMyyy") + ".txt", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.BaseStream.Seek(0, SeekOrigin.End);
                        sw.WriteLine(msg);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }
        public void WriteLogRecapcha(string msg, string rutaLog)
        {
            try
            {
                if (!System.IO.Directory.Exists(rutaLog))
                {
                    System.IO.Directory.CreateDirectory(rutaLog);
                }
                using (FileStream fs = new FileStream(rutaLog + "/Recapcha" + DateTime.Now.ToString("ddMMyyy") + ".txt", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.BaseStream.Seek(0, SeekOrigin.End);
                        sw.WriteLine(msg);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        public void debugError(Exception ex)
        {
            var errorPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogError");
            if (!Directory.Exists(errorPath))
            {
                Directory.CreateDirectory(errorPath);
            }

            StringBuilder MensajeError = new StringBuilder();
            MensajeError.AppendLine("Fecha hora: " + DateTime.Now.ToString());
            MensajeError.AppendLine("Mensaje: " + ex.Message);
            MensajeError.AppendLine("StackTrace: " + ex.StackTrace.ToString());
            MensajeError.AppendLine("");
            MensajeError.AppendLine("");

            using (FileStream fs = new FileStream(errorPath + "/Log" + DateTime.Now.ToString("ddMMyyy") + ".txt", FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    sw.WriteLine(MensajeError);
                    sw.Flush();
                    sw.Close();
                }
            }
        }
    }
}