namespace Testing1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestIngresoAlsistemaOK()
        {
            bool result = Testing.Program.ingresoAlsistema("adminx", "Diego@123");
            Assert.AreEqual(true, result);

        }

        [TestMethod]
        public void TestIngresoAlsistemaError() {

            bool result = Testing.Program.ingresoAlsistema("adminx", "Diego@123");
            Assert.AreEqual(false, result);
        }


        [TestMethod]
        public void TestenviarCertificado()
        {

            string result = Testing.Program.EnviarCertificadoEmail("jhuamanb10@gmail.com");
            Assert.AreEqual("Enviado", result);
        }

        [TestMethod]
        public void TestenviarCertificadoError()
        {

            string result = Testing.Program.EnviarCertificadoEmail("");
            Assert.AreEqual("Con error", result);
        }

        [TestMethod]
        public void TestconsultarCertificado()
        {

            string result = Testing.Program.consultaCertificado("47204456xx", "MODULO 1 - DISEÑO DEL PLAN DE SALUD MENTAL");
            Assert.AreEqual("Consulta correcta", result);
        }

        [TestMethod]
        public void TestdescargaCertificado()
        {

            string result = Testing.Program.descargaCertificado("47204456", "MODULOxx 1 - DISEÑO DEL PLAN DE SALUD MENTAL");
            Assert.AreEqual("Descarga correcta", result);
        }

    }
}