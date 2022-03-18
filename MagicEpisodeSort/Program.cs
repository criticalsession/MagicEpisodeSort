using System;
using TheBrain;

Console.WriteLine("Reading: " + System.AppContext.BaseDirectory);
Manager.MagicSort(System.AppContext.BaseDirectory);
Console.WriteLine("Done");
Console.ReadLine();