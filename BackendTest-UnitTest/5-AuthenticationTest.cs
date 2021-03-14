using BackendTestWebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTestUnitTest
{
    [TestClass]
    public class E_AuthenticationTest : _BaseClass
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            BaseInitialize();
        }

        [TestMethod]
        public async Task PostSaltRequest()
        {
            var param = new AuthenticationParam
            {
                Command = "loginSalt",
                Username = "johndoe"
            };

            var client = _factory.CreateClient();
            var response = await client.PostAsync($"api/Authenticate", ObjectToJsonContent(param));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(CONTENT_TYPE_JSON, response.Content.Headers.ContentType?.ToString());

            var strResult = await response.Content.ReadAsStringAsync();
            var objResult = JsonSerializer.Deserialize<AuthenticationResponse>(strResult, _jsonOptions);
            var expResult = "cbb4a64006378ec261840d39ab6cc76048f3dad16e19b7db508fb11ba4594c51"; //salt

            Assert.AreEqual(expResult, objResult.Salt, "Wrong Salt value!");
        }

        [TestMethod]
        public async Task PostSigninRequest()
        {
            var param = new LoginParam
            {
                Command = "login",
                UsernameOrEmail = "johndoe",
                Challenge = "02b78364fee0f76cdfb64c17b6a919b2940198742cb3f989af9271a81e9471c8"
            };

            var client = _factory.CreateClient();
            var response = await client.PostAsync($"api/Login", ObjectToJsonContent(param));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(CONTENT_TYPE_JSON, response.Content.Headers.ContentType?.ToString());

            var strResult = await response.Content.ReadAsStringAsync();
            var objResult = JsonSerializer.Deserialize<LoginResponse>(strResult, _jsonOptions);

            Assert.IsNotNull(objResult.SessionId, "Session Id is empty!");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            BaseDispose();
        }
    }
}
