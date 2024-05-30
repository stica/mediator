using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocky.Contract.Models;
using MockyProject;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace Mocky.Tests.BDDTests
{
    [TestClass]
    public class BddTests
    {
        private HttpClient _client;
        private TestServer _server;
        private HttpResponseMessage _response;
        private List<Product> _result;

        [TestInitialize]
        public void Setup()
        {
            _server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            _client = _server.CreateClient();
            _response = _client.GetAsync("/api/mocky/filter?maxprice=20&size=medium&highlight=green,blue").Result;

            var jsonContent = _response.Content.ReadAsStringAsync().Result;
            _result = JsonSerializer.Deserialize<List<Product>>(jsonContent);
        }

        [TestMethod]
        public void Response_Status_is_Ok()
        {
            Assert.AreEqual("OK", _response.StatusCode.ToString());
        }

        [TestMethod]
        public void Number_Of_Products_Is_Expected_One()
        {
            Assert.AreEqual(12, _result.Count);
        }

        [TestMethod]
        public void There_Is_Expected_Product_In_List()
        {
            Assert.IsTrue(_result.Any(x => x.description == "This trouser perfectly pairs with a green shirt."));
        }
    }
}
