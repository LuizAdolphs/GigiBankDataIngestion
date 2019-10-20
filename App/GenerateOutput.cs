
namespace GigiBankDataIngestion
{
    using System;
    using System.IO;
    using System.Linq;

    public class GenerateOutput
    {
        private const string fileContent = "Quantidade de clientes no arquivo de entrada: {0}\n" +
            "Quantidade de vendedores no arquivo de entrada: {1}\n" +
            "ID da venda mais cara: {2}\n" +
            "O pior vendedor: {3}";

        public string FilePath { get; private set; }

        public GenerateOutput(string filePath)
        {
            this.FilePath = filePath;
        }

        public void HandleEvent(object source, FileSystemEventArgs e)
        {
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType} found");

            var integrationEngine = new IntegrationEngine();

            var fileResponse = integrationEngine
                .ProcessFileAsync(e.FullPath)
                .GetAwaiter()
                .GetResult();

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(FilePath, e.Name)))
            {

                if (fileResponse.IsValid)
                {
                    var data = fileResponse?.Model;

                    var totalClients = data?.Client?.Count;
                    var totalSalesPerson = data?.SalesPerson?.Count;

                    var mostExpensiveSaleId = data?
                        .Sales?
                        .GroupBy(x => x?.Model?.SaleId)
                        .Select(k => new { SaleId = k.Key, TotalPrice = k.Sum(s => s?.Model?.Items?.Sum(n => n?.ItemPrice ?? 0)) ?? 0})
                        .OrderByDescending(x => x.TotalPrice)
                        .FirstOrDefault()
                        .SaleId;

                    var worstSelesPerson = data?
                        .Sales?
                        .GroupBy(x => new { x?.Model?.SaleId, x?.Model?.SalesPerson })
                        .Select(k => new { k.Key.SaleId, k.Key.SalesPerson, TotalPrice = k.Sum(s => s?.Model?.Items?.Sum(n => n?.ItemPrice ?? 0)) ?? 0})
                        .OrderBy(x => x.TotalPrice)
                        .FirstOrDefault()
                        .SalesPerson;

                    outputFile.Write(string.Format(fileContent, totalClients, totalSalesPerson, mostExpensiveSaleId, worstSelesPerson));
                }
                else
                {
                    outputFile.Write($"The follwing errors were found in file {e.Name}: \n{string.Join("\n", fileResponse.MessageList)}");
                }
            }
        }
    }
}