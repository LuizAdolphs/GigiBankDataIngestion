namespace GigiBankDataIngestion.Tests
{
    using GigiBankDataIngestion.Models;
    using Xunit;

    public class ClientDataIngestionFacts
    {
        [Fact]
        public void IngestAValidClientData()
        {
            var clientData = new ClientData();

            var clientIngestData = "002ç2345675434544345çJose da SilvaçRural";

            var response = clientData.Parse(clientIngestData);

            Assert.True(response.IsValid);
        }

        [Fact]
        public void IngestAValidClientWithCedilNameData()
        {
            var clientData = new ClientData();

            var clientIngestData = "002ç2345675434544345çJose do Açai SilvaçRural";

            var response = clientData.Parse(clientIngestData);

            Assert.True(response.IsValid);

            Assert.True(response.IsValid);

            Assert.IsType<ClientData>(response.Model);

            Assert.Equal("Jose do Açai Silva", response.Model?.Name);
        }
    }
}