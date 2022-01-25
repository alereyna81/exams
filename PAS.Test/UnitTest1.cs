using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using PAS.Controllers;

namespace PAS.Test
{
    public class Tests
    {
        private readonly IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void getactivities_whenloadpage_shouldbereturnlist()
        {
            ActivityController pobj = new ActivityController(_configuration);
            var activities = pobj.Get();
            foreach (var a in activities)
            {
                Assert.AreEqual(a.id, 100);
                Assert.AreEqual(a.title, "Prueba");
            }
        }
    }
}