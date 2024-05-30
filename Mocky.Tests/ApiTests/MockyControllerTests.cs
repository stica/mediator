using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocky.Contract.Models;
using MockyProject;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace Mocky.Tests.ApiTests
{
    [TestClass]
    public class MockyControllerTests
    {
        protected HttpClient _client;

        private TestServer _server;

        [TestInitialize]
        public void Setup()
        {
            _server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [TestMethod]
        public void ReturnProducts_Works_Ok()
        {
            var response = _client.GetAsync("/api/mocky/filter?maxprice=20&size=medium&highlight=green,blue").Result;
            Assert.IsNotNull(response);
            Assert.AreEqual("OK", response.StatusCode.ToString());

            var jsonContent = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<List<Product>>(jsonContent);

            Assert.AreEqual(12, result.Count);
        }
    }
}
