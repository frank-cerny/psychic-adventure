using Xunit;

namespace bike_selling_app.WebUI.IntegrationTests
{
    // This class has no purpose except to define a Collection Fixture and explain what interfaces to include
    [CollectionDefinition("Test Collection")]
    public class TestCollection : ICollectionFixture<HttpServerFixture>
    {

    }
}