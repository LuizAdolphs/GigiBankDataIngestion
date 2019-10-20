namespace GigiBankDataIngestion.Models
{
    using GigiBankDataIngestion.Common;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class SalesData : DataParseBase<SalesData>
    {
        public const string Identifier = "003";
        public int? SaleId { get; set; }
        public IEnumerable<Item?>? Items { get; set; }
        public string? SalesPerson { get; set; }

        private static Regex itemDataRegex = new Regex(@"(\d+)-(\d+)-(\d+(\.\d+)?)", RegexOptions.Compiled);

        public class Item
        {
            public int? ItemId { get; set; }
            public int? ItemQuantity { get; set; }
            public decimal? ItemPrice { get; set;}
        }

        protected override Response<SalesData> ParseEngine(string data)
        {
            if (data.Substring(0, 3).Equals(SalesData.Identifier))
            {
                var splittedData = data.Split(separator);

                var salesData = new SalesData();

                if(int.TryParse(splittedData[1], out int salesId))
                {
                    salesData.SaleId = salesId;
                }
                else
                {
                    return new Response<SalesData>(false)
                        .AddErrorMessage($"Error while parsing {nameof(salesData.SaleId)}");
                }

                var matches = itemDataRegex.Matches(splittedData[2]);

                if (matches.Count == 0)
                {
                    return new Response<SalesData>(false)
                        .AddErrorMessage($"Error while parsing {nameof(salesData.SaleId)}");
                }

                salesData.Items = matches.Select(item =>
                {
                    if (int.TryParse(item.Groups[1].Value, out int itemId) &&
                        int.TryParse(item.Groups[2].Value, out int itemQuantity) &&
                        decimal.TryParse(item.Groups[3].Value, out decimal itemPrice))
                    {
                        return new Item
                        {
                            ItemId = itemId,
                            ItemQuantity = itemQuantity,
                            ItemPrice = itemPrice
                        };
                    }

                    return null;
                });

                if(salesData.Items.Any(x => x == null))
                {
                    return new Response<SalesData>(false)
                        .AddErrorMessage($"One of the items couldn't be processed");
                }

                salesData.SalesPerson = string.Join(separator, splittedData.Skip(3));

                return new Response<SalesData>(true)
                    .AddObjectModel(salesData);
            }

            return new Response<SalesData>(false)
                .AddErrorMessage("This is not a sales data");
        }
    }
}