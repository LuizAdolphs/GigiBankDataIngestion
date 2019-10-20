namespace GigiBankDataIngestion.Tests
{
    using GigiBankDataIngestion.Models;
    using Xunit;

    public class SalesPersonDataIngestionFacts
    {
        [Fact]
        public void IngestAValidSalesData()
        {
            var salesPersonData = new SalesPersonData();

            var salesmanData = "001ç1234567891234çPedroç50000";

            var response = salesPersonData.Parse(salesmanData);

            Assert.True(response.IsValid);
        }

        [Fact]
        public void IngestAValidSalesDataWithPlusCedilInName()
        {
            var salesPersonData = new SalesPersonData();

            var salesmanData = "001ç1234567891234çAçaíç50000";

            var response = salesPersonData.Parse(salesmanData);

            Assert.True(response.IsValid);

            Assert.IsType<SalesPersonData>(response.Model);

            Assert.Equal("Açaí", response.Model?.Name);
        }

        [Fact]
        public void IngestAValidSalesDataWithSequenceCedilInName()
        {
            var salesPersonData = new SalesPersonData();

            var salesmanData = "001ç1234567891234çAççaíç50000";

            var response = salesPersonData.Parse(salesmanData);

            Assert.True(response.IsValid);

            Assert.IsType<SalesPersonData>(response.Model);

            Assert.Equal("Aççaí", response.Model?.Name);
        }

        [Fact]
        public void IngestAValidSalesDataWithSeparatedCedilInName()
        {
            var salesPersonData = new SalesPersonData();

            var salesmanData = "001ç1234567891234çAçaíComAçaíç50000";

            var response = salesPersonData.Parse(salesmanData);

            Assert.True(response.IsValid);

            Assert.IsType<SalesPersonData>(response.Model);

            Assert.Equal("AçaíComAçaí", response.Model?.Name);
        }

        [Fact]
        public void IngestANullSalesData()
        {
            var salesPersonData = new SalesPersonData();

            var response = salesPersonData.Parse(null);

            Assert.False(response.IsValid);
        }

        [Fact]
        public void IngestAInvalidSalesData()
        {
            var salesPersonData = new SalesPersonData();

            var salesmanData = "002ç1234567891234çAçaíComAçaíç50000";

            var response = salesPersonData.Parse(salesmanData);

            Assert.False(response.IsValid);
        }

        [Fact]
        public void IngestAInvalidCPFSalesData()
        {
            var salesPersonData = new SalesPersonData();

            var salesmanData = "001ç123567891234çAçaíComAçaíç50000";

            var response = salesPersonData.Parse(salesmanData);

            Assert.False(response.IsValid);
        }
    }
}
