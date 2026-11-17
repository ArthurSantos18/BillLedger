namespace BillLedger.IntegrationTests.Base;

[CollectionDefinition(nameof(ControllerTestCollection))]
public sealed class ControllerTestCollection : ICollectionFixture<WebApplicationFixture>
{
}