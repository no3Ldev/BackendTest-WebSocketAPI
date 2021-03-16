using BackendTestWebSocket.Controllers;
using BackendTestWebSocket.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            var client = new AvailableController(_config, _context);
            var response = await client.Post(param);

            Assert.IsNotNull(response.Available);
        }

        [TestMethod]
        public async Task PostCheckEmail()
        {
            var param = new AvailabilityParam
            {
                Command = "checkEmail",
                Email = "john.doe@mail.com"
            };

            var client = new AvailableController(_config, _context);
            var response = await client.Post(param);

            Assert.IsNotNull(response.Available);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            BaseDispose();
        }
    }
}
