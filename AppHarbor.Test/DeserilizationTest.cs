using System;
using System.IO;
using System.Text;
using AppHarbor.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace AppHarbor.Test
{
    [TestClass]
    public class DeserilizationTest
    {
        private string _DataPath = null;
        private Encoding _Encoding;

        [TestInitialize]
        public void InitTest()
        {
            _DataPath = Util.GetDataPath();
            _Encoding = Encoding.UTF8;
        }

        private byte[] GetBytes(string fileName)
        {
            return _Encoding.GetBytes(File.ReadAllText(Path.Combine(_DataPath, fileName), _Encoding));
        }


        [TestMethod]
        public void Can_Deserialize_Application()
        {
            var response = new RestResponse()
            {
                RawBytes = GetBytes("GetApplication.json"),
            };

            var json = new CustomJsonDeserializer();
            var item = json.Deserialize<Application>(response);
        }
    }
}
