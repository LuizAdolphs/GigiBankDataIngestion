namespace GigiBankDataIngestion.Tests
{
    using Xunit;
    using GigiBankDataIngestion;
    using System.Threading.Tasks;

    public class IntegrationEngineFacts
    {
        [Fact]
        public async Task ProcessFileAnyDataAsync()
        {
            var integrationEngine = new IntegrationEngine();

            var filePath = "data/in/001";

            var response = await integrationEngine.ProcessFileAsync(filePath);

            Assert.NotEmpty(response.Model.SalesPerson);
        }

        [Fact]
        public async Task ProcessFileEmptyDataAsync()
        {
            var integrationEngine = new IntegrationEngine();

            var filePath = "data/in/Empty";

            var response = await integrationEngine.ProcessFileAsync(filePath);

            Assert.False(response.IsValid);
        }

        [Fact]
        public async Task ProcessFileErrorAsync()
        {
            var integrationEngine = new IntegrationEngine();

            var filePath = "data/in/Error";

            var response = await integrationEngine.ProcessFileAsync(filePath);

            Assert.False(response.IsValid);
            Assert.NotEmpty(response.MessageList);
        }
    }
}