using BackendTestWebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTestUnitTest
{
    [TestClass]
    public class C_AvailabilityTest : _BaseClass
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            BaseInitialize();
        }

        [TestMethod]
        public async Task PostCheckUsername()
        {
            var param = new AvailabilityParam
            {
                Command = "checkUsername",
                Username = "johndoe"
            };

            var client = _factory.CreateClient();
            var response = await client.PostAsync($"api/Available", ObjectToJsonContent(param));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(CONTENT_TYPE_JSON, response.Content.Headers.ContentType?.ToString());

            var strResult = await response.Content.ReadAsStringAsync();
            var objResult = JsonSerializer.Deserialize<AvailabilityResponse>(strResult, _jsonOptions);

            Assert.IsNotNull(objResult.Available);
        }

        [TestMethod]
        public async Task PostCheckEmail()
        {
            var param = new AvailabilityParam
            {
                Command = "checkEmail",
                Email = "john.doe@mail.com"
            };

            var client = _factory.CreateClient();
            var response = await client.PostAsync($"api/Available", ObjectToJsonContent(param));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(CONTENT_TYPE_JSON, response.Content.Headers.ContentType?.ToString());

            var strResult = await response.Content.ReadAsStringAsync();
            var objResult = JsonSerializer.Deserialize<AvailabilityResponse>(strResult, _jsonOptions);

            Assert.IsNotNull(objResult.Available);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            BaseDispose();
        }
    }
}
