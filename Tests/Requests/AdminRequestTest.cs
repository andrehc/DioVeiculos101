using System.Text;
using System.Net;
using System.Text.Json;
using DioVeiculos101.Domain.DTO;
using DioVeiculos101.Domain.ModelViews;
using Test.Helpers;

namespace Tests.Requests
{
    [TestClass]
    public class AdminRequestTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup.ClassInit(context);
        }
        [ClassCleanup]
        public static void ClassCleanup()
        {
            Setup.ClassCleanup();
        }
        [TestMethod]
        public async Task TestGetSetProperties()
        {
            //arrange
            var loginDto = new LoginDTO
            {
                Email = "sanctum@mail.com",
                Password = "102030"
            };
            var content = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");

            //act
            var response = await Setup.client.PostAsync("/login", content);

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();

            var loggedAdmin = JsonSerializer.Deserialize<Logged>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(loggedAdmin);
            Assert.IsNotNull(loggedAdmin.Token);
        }
    }
}
