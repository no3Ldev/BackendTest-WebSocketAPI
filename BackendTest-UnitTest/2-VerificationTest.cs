using BackendTestWebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTestUnitTest
{
    [TestClass]
    public class B_VerificationTest : _BaseClass
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            BaseInitialize();
        }

        [TestMethod]
        public async Task PostVerification()
        {
            var param = new VerificationParam
            {
                Command = "emailVerification",
                Email = "john.doe@mail.com",
                Username = "johndoe"
            };

            var client = _factory.CreateClient();
            var response = await client.PostAsync($"api/Verify", ObjectToJsonContent(param));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(CONTENT_TYPE_JSON, response.Content.Headers.ContentType?.ToString());

            var strResult = await response.Content.ReadAsStringAsync();
            var objResult = JsonSerializer.Deserialize<VerificationResponse>(strResult, _jsonOptions);

            Assert.IsNotNull(objResult.Success, "Verification failed!");
        }

        [TestMethod]
        public async Task VerificationCodeGet()
        {
            var username = "johndoe";

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"api/Verify/{username}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(CONTENT_TYPE_TEXT, response.Content.Headers.ContentType?.ToString());

            var actualResult = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(actualResult, "Verification code not found");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            BaseDispose();
        }
    }
}
