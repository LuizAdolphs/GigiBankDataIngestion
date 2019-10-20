namespace GigiBankDataIngestion
{
    using System;
    using System.IO;
    using System.Security.Permissions;

    class Program
    {
        private const string defaultInFolder = "data/in";
        private const string defatulOutFolder = "data/out";

        static void Main(string[] args)
        {
            var inFolder = args != null && args.Length == 2 ? args[0] : defaultInFolder;
            var outFolder = args != null && args.Length == 2 ? args[1] : defatulOutFolder;

            var generateOutput = new GenerateOutput(outFolder);

            Watcher.WatchFiles(inFolder, generateOutput.HandleEvent);
        }
    }
}
