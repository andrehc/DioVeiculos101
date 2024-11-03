
using DioVeiculos101.Domain.Entity;

namespace Tests.Domain.Entities
{
    public class AdminTest
    {
        [TestMethod]
        public void TestGetSetProperties()
        {
            var admin = new Admin();

            admin.Id = 1;
            admin.Email = "lorem@ipsum.com";
            admin.Name = "lorem Ipsum";
            admin.Profile = "admin";
            admin.Password = "102030";

            Assert.AreEqual(1, admin.Id);
            Assert.AreEqual("lorem@ipsum", admin.Email);
            Assert.AreEqual("lorem ipsum", admin.Name);
            Assert.AreEqual("admin", admin.Profile);
            Assert.AreEqual("102030", admin.Password);
        }
    }
}
