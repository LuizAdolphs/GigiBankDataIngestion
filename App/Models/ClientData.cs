namespace GigiBankDataIngestion.Models
{
    using GigiBankDataIngestion.Common;
    using System.Linq;

    public class ClientData : DataParseBase<ClientData>
    {
        public const string Identifier = "002";
        private const int cpnjSize = 16;

        public string? CNPJ { get; set; }
        public string? Name { get; set; }
        public string? BusinessArea { get; set; }

        protected override Response<ClientData> ParseEngine(string data)
        {
            if (data.Substring(0, 3).Equals(ClientData.Identifier))
            {
                var splittedData = data.Split(separator);

                var clientData = new ClientData
                {
                    CNPJ = splittedData[1]
                };

                if (clientData.CNPJ?.Length != cpnjSize)
                {
                    //TODO: consider to use a validation library like Fluent validations
                    return new Response<ClientData>(false)
                        .AddErrorMessage($"The {nameof(clientData.CNPJ)} is invalid");
                }

                if (splittedData.Length == 4)
                {
                    clientData.Name = splittedData[2];

                    clientData.BusinessArea = splittedData[2];
                }
                else if (splittedData.Length > 4)
                {
                    var name = string.Join(separator, splittedData.Skip(2).Take(splittedData.Length - 3));

                    clientData.Name = name;

                    clientData.BusinessArea = splittedData[splittedData.Length - 1];
                }

                return new Response<ClientData>(true)
                    .AddObjectModel(clientData);
            }

            return new Response<ClientData>(false)
                .AddErrorMessage("This is not a client data");
        }
    }
}