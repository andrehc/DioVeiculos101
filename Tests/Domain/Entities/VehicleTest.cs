using DioVeiculos101.Domain.Entity;

namespace Tests.Domain.Entities
{
    public class VehicleTest
    {
        public void TestGetSetVehicleProperties()
        {
            var v = new Vehicle();

            v.Id = 1;
            v.Brand = "Lorem";
            v.Name = "Ipsum";
            v.Year = 12;

            Assert.AreEqual(1, v.Id);
            Assert.AreEqual("Lorem", v.Brand);
            Assert.AreEqual("Ipsum", v.Name);
            Assert.AreEqual(12, v.Year);

        }
    }
}
