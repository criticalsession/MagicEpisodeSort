using System;
using System.Collections.Generic;
using System.IO;
using TheBrain;

bool running = true;
string root = System.AppContext.BaseDirectory;

while (running)
{
    Console.Clear();
    Console.Write("Press 'enter' to run, or 'm' for menu: ");
    ConsoleKeyInfo keyPress = Console.ReadKey();
    Console.WriteLine();

    if (keyPress.Key == ConsoleKey.Enter)
    {
        RunMagicSort();
        running = false;
    }
    else
    {
        Manager.ReadConfig(root);

        while (running)
        {
            Console.Clear();
            Console.WriteLine("Magic Sort Main Menu");
            Console.WriteLine("---");
            var menu = new EasyConsole.Menu()
                //.Add("Manage TV Show Titles", () => { })
                .Add("Set Sorted Target Directory", () =>
                {
                    Console.Clear();
                    Console.WriteLine("Current target directory: " + Manager.config.SortedDirectory);
                    Console.WriteLine();
                    Console.Write("New target directory name (leave empty to keep): ");
                    string newDirectory = Console.ReadLine();

                    if (!String.IsNullOrEmpty(newDirectory))
                    {
                        Manager.config.SortedDirectory = newDirectory;
                        Manager.UpdateConfig(root);
                        Console.WriteLine();
                        Console.WriteLine("Target directory changed. Press 'enter' to return to menu.");
                        Console.ReadLine();
                    }
                })
                .Add("Run Magic Sort", () =>
                {
                    RunMagicSort();
                    running = false;
                })
                .Add("Quit", () =>
                {
                    running = false;
                });
            menu.Display();
        }
    }
}

static void RunMagicSort()
{
    Console.Clear();
    string root = System.AppContext.BaseDirectory;

    Manager.ReadConfig(root);
    List<VideoFile> videoFiles = Manager.BuildVideoFiles(root);

    List<string[]> adjustedSeriesNames = new List<string[]>();
    int count = 0;
    List<string> newSeriesNames = Manager.GetNewSeriesNames(videoFiles);

    foreach (var seriesName in newSeriesNames) 
    {
        Console.Clear();

        Console.WriteLine("New Series Name Found (" + ++count + " of " + newSeriesNames.Count + ")");

        Console.Write("Enter custom name for \"" + seriesName + "\" (press 'Enter' to keep as is): ");
        string customName = Console.ReadLine().Trim();

        string[] newSeriesName = new string[] { seriesName, String.IsNullOrEmpty(customName) ? seriesName : customName };
        adjustedSeriesNames.Add(newSeriesName);
    }

    Console.Clear();

    Manager.UpdateConfig(root, adjustedSeriesNames);
    videoFiles = Manager.BuildVideoFiles(root);
    Manager.MoveFiles(root, videoFiles);

    Console.WriteLine("Sorting done.");
    Console.ReadLine();
}