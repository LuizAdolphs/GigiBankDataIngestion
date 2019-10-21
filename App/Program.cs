namespace GigiBankDataIngestion
{
    using System.IO;
    
    class Program
    {
        private static string defaultInFolder = Path.Combine("data","in");
        private static string defatulOutFolder = Path.Combine("data", "out");

        static void Main(string[] args)
        {
            var inFolder = args != null && args.Length == 2 ? args[0] : defaultInFolder;
            var outFolder = args != null && args.Length == 2 ? args[1] : defatulOutFolder;

            var generateOutput = new GenerateOutput(outFolder);

            Watcher.WatchFiles(inFolder, generateOutput.HandleEvent);
        }
    }
}
