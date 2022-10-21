using System;
using System.Formats.Tar;
using System.IO;
using System.Linq;

namespace EpiphanyDiag
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Epiphany Diagmostics Tool v0.1 - Written by OpenSauce");
            
			string[] logFiles = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\My Games\\Binding of Isaac Repentance", "*.txt", SearchOption.TopDirectoryOnly);

			bool validFolder = false;
            foreach (string filePath in logFiles)
            {
                if (filePath.Contains("log.txt")) {
                    validFolder = true;
                }
            }

            if (validFolder) {
                Console.WriteLine("Isaac Repentance log folder found.");
			} else {
                Console.WriteLine("ERROR: Could not find Isaac Repentance log folder!\nPress any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }

        }
    }
}