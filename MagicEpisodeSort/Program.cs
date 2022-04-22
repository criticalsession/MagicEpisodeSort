using System;
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
    Manager.MagicSort(System.AppContext.BaseDirectory);
    Console.WriteLine("Sorting done.");
    Console.ReadLine();
}