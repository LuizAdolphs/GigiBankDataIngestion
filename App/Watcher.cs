namespace GigiBankDataIngestion
{
    using System;
    using System.IO;

    public static class Watcher
    {
        public static void WatchFiles(string path, FileSystemEventHandler handler)
        {
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {

                path = Path.Combine(Directory.GetCurrentDirectory(), path);

                Console.WriteLine($"Watching directory {path}");

                watcher.Path = path;

                watcher.NotifyFilter = NotifyFilters.LastWrite
                                    | NotifyFilters.FileName;

                watcher.Filter = "*.*";

                watcher.Changed += handler ?? OnChanged;
                watcher.Created += handler ?? OnChanged;

                watcher.EnableRaisingEvents = true;

                Console.WriteLine("Press 'q' to quit.");
                while (Console.Read() != 'q') ;
            }
        }
        private static void OnChanged(object source, FileSystemEventArgs e) =>
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
    }
}
