using BackendTestWebSocket.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendTestUnitTest
{
    [TestClass]
    public class A_Sha256HmacTest : _BaseClass
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            BaseInitialize();
        }

        [TestMethod]
        public void GetPasswordHash()
        {
            var username = "johndoe";
            var password = "somePassword";
            var expectedResult = "8437ae0231129d7038809d7aa68e89430b73e245b99b9cc662cbc0bd9cc6f6da";

            var client = new HashController();
            var actualResult = client.Get(username, password);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetPasswordHash2()
        {
            var email = "john.doe@mail.com";
            var password = "somePassword";
            var expectedResult = "55212a9a47b566ca3aa4ab24a00a0d1579d47cc9367ed15c1be61aa05c3467c3"; //actual result

            var client = new HashController();
            var actualResult = client.Get(email, password);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetPasswordHashWithKey()
        {
            var hashedPassword = "8437ae0231129d7038809d7aa68e89430b73e245b99b9cc662cbc0bd9cc6f6da";
            var superSecretKey = "superSecretKey";
            var expectedResult = "92d55f54ca872dc20c2f882b22e152f9c82ff62c66e1b9461e9f80011b3255c6"; //hmacForSaving

            var client = new HashController();
            var actualResult = client.Get(hashedPassword, superSecretKey);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            BaseDispose();
        }
    }
}
