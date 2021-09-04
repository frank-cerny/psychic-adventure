using NUnit.Framework;
using System.Threading.Tasks;
using System.IO;

namespace bike_selling_app.Application.IntegrationTests
{
    using static Testing;

    public class TestBase
    {
        [SetUp]
        public void Setup()
        {
            // Ensure the database is deleted before each test so each test gets a fresh copy
            if (File.Exists("test-database.db"))
            {
                File.Delete("test-database.db");
            }
            Testing.EnsureDatabase();
        }
    }
}
