namespace GigiBankDataIngestion
{
    using GigiBankDataIngestion.Common;
    using GigiBankDataIngestion.Models;
    using System.IO;
    using System.Threading.Tasks;

    public class IntegrationEngine
    {
        private const int lineMinimumLength = 3;

        public async Task<Response<FileResponse>> ProcessFileAsync(string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                return new Response<FileResponse>(false)
                    .AddErrorMessage("You must provide filepath");
            }

            try
            {
                var lines = await File.ReadAllLinesAsync(filePath);

                if(lines.Length == 0)
                {
                    return new Response<FileResponse>(false)
                        .AddErrorMessage("The file seems empty.");
                }

                var fileResponse = new Response<FileResponse>(true)
                    .AddObjectModel(new FileResponse());

                foreach (var line in lines)
                {
                    if (fileResponse.IsValid)
                    {
                        fileResponse = ReadLine(fileResponse, line);

                        continue;
                    }

                    break;
                }

                return fileResponse;
            }
            catch (IOException e)
            {
                return new Response<FileResponse>(false)
                    .AddErrorMessage(e.Message);
            }
        }

        private Response<FileResponse> ReadLine(Response<FileResponse> response, string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return new Response<FileResponse>(false)
                    .AddErrorMessage("Line is empty");
            }

            if (line.Length <= lineMinimumLength)
            {
                return new Response<FileResponse>(false)
                    .AddErrorMessage("Line is too short");
            }

            var identifier = line.Substring(0,3);

            switch(identifier)
            {
                case SalesPersonData.Identifier:
                    var responseSalesPerson = new SalesPersonData().Parse(line);

                    if (!responseSalesPerson.IsValid)
                    {
                        foreach(var erorr in responseSalesPerson.MessageList)
                        {
                            response.AddErrorMessage(erorr);
                        }

                        break;
                    }

                    response?.Model?.SalesPerson?.Add(responseSalesPerson);

                    break;
                case ClientData.Identifier:
                    var responseClient = new ClientData().Parse(line);

                    if (!responseClient.IsValid)
                    {
                        foreach(var erorr in responseClient.MessageList)
                        {
                            response.AddErrorMessage(erorr);
                        }

                        break;
                    }

                    response?.Model?.Client?.Add(responseClient);

                    break;
                case SalesData.Identifier:
                    var responseSales = new SalesData().Parse(line);

                    if (!responseSales.IsValid)
                    {
                        foreach(var erorr in responseSales.MessageList)
                        {
                            response.AddErrorMessage(erorr);
                        }

                        break;
                    }

                    response?.Model?.Sales?.Add(responseSales);

                    break;
                default:
                    return new Response<FileResponse>(false)
                        .AddErrorMessage($"The line identificaror \"{identifier}\" is invalid");
            }

            return response ?? new Response<FileResponse>();
        }
    }

}
