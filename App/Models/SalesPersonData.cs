namespace GigiBankDataIngestion.Models
{
    using GigiBankDataIngestion.Common;
    using System.Linq;

    public class SalesPersonData : DataParseBase<SalesPersonData>
    {
        public const string Identifier = "001";
        private const int cpfSize = 13;

        public string? CPF { get; set; }
        public string? Name { get; set; }
        public decimal? Salary { get; set; }

        protected override Response<SalesPersonData> ParseEngine(string data)
        {
            if (data.Substring(0, 3).Equals(SalesPersonData.Identifier))
            {
                var splittedData = data.Split(separator);

                var salesPersonData = new SalesPersonData
                {
                    CPF = splittedData[1]
                };

                if (salesPersonData.CPF?.Length != cpfSize)
                {
                    //TODO: consider to use a validation library like Fluent validations
                    return new Response<SalesPersonData>(false)
                        .AddErrorMessage($"The {nameof(salesPersonData.CPF)} is invalid");
                }

                if (splittedData.Length == 4)
                {
                    salesPersonData.Name = splittedData[2];

                    if (decimal.TryParse(splittedData[3], out decimal salary))
                    {
                        salesPersonData.Salary = salary;
                    }
                }
                else if (splittedData.Length > 4)
                {
                    var name = string.Join(separator, splittedData.Skip(2).Take(splittedData.Length - 3));

                    salesPersonData.Name = name;

                    if (decimal.TryParse(splittedData[splittedData.Length - 1], out decimal salary))
                    {
                        salesPersonData.Salary = salary;
                    }
                }

                return new Response<SalesPersonData>(true)
                    .AddObjectModel(salesPersonData);
            }

            return new Response<SalesPersonData>(false)
                .AddErrorMessage("This is not a sales person data");
        }
    }
}