using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace projectservice.integrationtests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async void DefaultRoute_ReturnsHelloWorld()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync("");
            var stringResult = await response.Content.ReadAsStringAsync();

            //Assert.AreEqual("Hello World!")
        }
    }
}