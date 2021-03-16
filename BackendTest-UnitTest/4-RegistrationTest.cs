using BackendTestWebSocket.Controllers;
using BackendTestWebSocket.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var paramVerify = new VerificationParam
            {
                Command = "getVerificationCode",
                Username = "johndoe"
            };

            var clientVerify = new VerifyController(_config, _context);
            var responseVerify = await clientVerify.Get(paramVerify);

            Assert.IsTrue(responseVerify.Success, "No valid verification code found!");

            var param = new RegistrationParam
            {
                Command = "register",
                Username = paramVerify.Username,
                DisplayName = "Bigjohndoe",
                Password = "8437ae0231129d7038809d7aa68e89430b73e245b99b9cc662cbc0bd9cc6f6da",
                Password2 = "55212a9a47b566ca3aa4ab24a00a0d1579d47cc9367ed15c1be61aa05c3467c3",
                Password3 = "cdd3485891de29fb1f242676bdb6d8b515a619b5ecfceae651bd01340b00a778",
                Email = "john.doe@mail.com",
                VerificationCode = responseVerify.Remarks //verificationCode
            };

            var client = new RegisterController(_config, _context);
            var response = await client.Post(param);

            Assert.IsNotNull(response.Success, response.Remarks);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            BaseDispose();
        }
    }
}
