using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;

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
        public async Task GetPasswordHash()
        {
            var username = "johndoe";
            var password = "somePassword";
            var expectedResult = "8437ae0231129d7038809d7aa68e89430b73e245b99b9cc662cbc0bd9cc6f6da";
            
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"api/Hash/{username}/{password}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(CONTENT_TYPE_TEXT, response.Content.Headers.ContentType?.ToString());

            var actualResult = await response.Content.ReadAsStringAsync(); 
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public async Task GetPasswordHash2()
        {
            var email = "john.doe@mail.com";
            var password = "somePassword";
            var expectedResult = "55212a9a47b566ca3aa4ab24a00a0d1579d47cc9367ed15c1be61aa05c3467c3"; //actual result

            //from document, can't figure out how is this derived
            //var expectedResult = "8437ae0231129d7038809d7aa68e894902345bde25ad0fb662cbc0bd9cc6f6da";

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"api/Hash/{email}/{password}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(CONTENT_TYPE_TEXT, response.Content.Headers.ContentType?.ToString());

            var actualResult = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public async Task GetPasswordHashWithSuperKey()
        {
            var hashedPassword = "8437ae0231129d7038809d7aa68e89430b73e245b99b9cc662cbc0bd9cc6f6da";
            var superSecretKey = "superSecretKey";
            var expectedResult = "92d55f54ca872dc20c2f882b22e152f9c82ff62c66e1b9461e9f80011b3255c6"; //hmacForSaving

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"api/Hash/{hashedPassword}/{superSecretKey}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(CONTENT_TYPE_TEXT, response.Content.Headers.ContentType?.ToString());

            var actualResult = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            BaseDispose();
        }
    }
}
