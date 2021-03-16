using BackendTestWebSocket.Controllers;
using BackendTestWebSocket.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            var client = new AuthenticateController(_config, _context);
            var response = await client.Post(param);

            Assert.IsTrue(response.Success, response.Remarks);
        }

        [TestMethod]
        public async Task PostSigninRequest()
        {
            var paramAuth = new AuthenticationParam
            {
                Command = "loginSalt",
                Username = "johndoe"
            };

            var clientAuth = new AuthenticateController(_config, _context);
            var responseAuth = await clientAuth.Post(paramAuth);

            Assert.IsTrue(responseAuth.Success, responseAuth.Remarks);

            var hashedUsername = "92d55f54ca872dc20c2f882b22e152f9c82ff62c66e1b9461e9f80011b3255c6";
            
            var clientHash = new HashController();
            var hashOutput = clientHash.Get(hashedUsername, responseAuth.Salt);

            var param = new LoginParam
            {
                Command = "login",
                UsernameOrEmail = "johndoe",
                Challenge = hashOutput
            };

            var client = new LoginController(_config, _context);
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
