using BackendTestWebSocket.Controllers;
using BackendTestWebSocket.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            GetContext();

            var param = new VerificationParam
            {
                Command = "emailVerification",
                Email = "john.doe@mail.com",
                Username = "johndoe"
            };

            var client = new VerifyController(_config, _context);
            var response = await client.Post(param);

            Assert.IsNotNull(response.Success, response.Remarks);
        }

        [TestMethod]
        public async Task VerificationCodeGet()
        {
            GetContext();

            var param = new VerificationParam
            {
                Command = "getVerificationCode",
                Username = "johndoe"
            };

            var client = new VerifyController(_config, _context);
            var response = await client.Get(param);

            Assert.IsTrue(response.Success, response.Remarks);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            BaseDispose();
        }
    }
}
