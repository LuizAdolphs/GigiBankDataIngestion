
namespace GigiBankDataIngestion.Common
{
    using System.Collections.Generic;
    using GigiBankDataIngestion.Models;

    public class FileResponse
    {
        public List<Response<SalesPersonData>>? SalesPerson { get; set; } = new List<Response<SalesPersonData>>();
        public List<Response<ClientData>>? Client{ get; set; } = new List<Response<ClientData>>();
        public List<Response<SalesData>>? Sales { get; set; } = new List<Response<SalesData>>();
    }
}