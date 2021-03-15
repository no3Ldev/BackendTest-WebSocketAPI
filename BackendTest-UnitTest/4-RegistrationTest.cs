using BackendTestWebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTestUnitTest
{
    [TestClass]
    public class D_RegistrationTest : _BaseClass
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            BaseInitialize();
        }

        [TestMethod]
        public async Task PostRegistration()
        {
            var username = "johndoe";
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"api/Verify/{username}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(CONTENT_TYPE_TEXT, response.Content.Headers.ContentType?.ToString());

            var verificationCode = await response.Content.ReadAsStringAsync();

            Assert.IsNotNull(verificationCode, "No verification code found!");

            var param = new RegistrationParam
            {
                Command = "register",
                Username = username,
                DisplayName = "Bigjohndoe",
                Password = "8437ae0231129d7038809d7aa68e89430b73e245b99b9cc662cbc0bd9cc6f6da",
                Password2 = "55212a9a47b566ca3aa4ab24a00a0d1579d47cc9367ed15c1be61aa05c3467c3", //8437ae0231129d7038809d7aa68e894902345bde25ad0fb662cbc0bd9cc6f6da
                Password3 = "cdd3485891de29fb1f242676bdb6d8b515a619b5ecfceae651bd01340b00a778",
                Email = "john.doe@mail.com",
                VerificationCode = verificationCode
            };

            response = await client.PostAsync($"api/Register", ObjectToJsonContent(param));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(CONTENT_TYPE_JSON, response.Content.Headers.ContentType?.ToString());

            var strResult = await response.Content.ReadAsStringAsync();
            var objResult = JsonSerializer.Deserialize<RegistrationResponse>(strResult, _jsonOptions);

            Assert.IsNotNull(objResult.Success, "Registration failed!");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            BaseDispose();
        }
    }
}
