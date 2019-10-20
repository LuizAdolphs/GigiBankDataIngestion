namespace GigiBankDataIngestion.Tests
{
    using GigiBankDataIngestion.Models;
    using System.Linq;
    using Xunit;

    public class SalesDataIngestionFacts
    {
        [Fact]
        public void IngestAValidSalesData()
        {
            var salesData = new SalesData();

            var salesmanData = "003ç10ç[1-10-100,2-30-2.50,3-40-3.10]çPedro";

            var response = salesData.Parse(salesmanData);

            Assert.True(response.IsValid);
        }

        [Fact]
        public void IngestAValidSalesDataGetItem()
        {
            var salesData = new SalesData();

            var salesmanData = "003ç08ç[1-34-10,2-33-1.50,3-40-0.10]çPaulo";

            var response = salesData.Parse(salesmanData);

            Assert.True(response.IsValid);

            Assert.IsType<SalesData>(response.Model);

            Assert.Equal(1.5m, response.Model?.Items?.ToArray()[1].ItemPrice);
        }

        [Fact]
        public void IngestAValidSalesDataWithCedil()
        {
            var salesData = new SalesData();

            var salesmanData = "003ç08ç[1-34-10,2-33-1.50,3-40-0.10]çPaulo Açaí";

            var response = salesData.Parse(salesmanData);

            Assert.True(response.IsValid);

            Assert.IsType<SalesData>(response.Model);

            Assert.Equal("Paulo Açaí", response.Model?.SalesPerson, true,true,true);
        }


        [Fact]
        public void IngestAValidSalesDataVerifySalesId()
        {
            var salesData = new SalesData();

            var salesmanData = "003ç08ç[1-34-10,2-33-1.50,3-40-0.10]çPaulo Açaí";

            var response = salesData.Parse(salesmanData);

            Assert.True(response.IsValid);

            Assert.IsType<SalesData>(response.Model);

            Assert.Equal(8, response.Model?.SaleId);
        }

    }
}
