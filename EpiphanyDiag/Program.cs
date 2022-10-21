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
                Console.WriteLine("Generating diagnostics package...");
			} else {
                Console.WriteLine("ERROR: Could not find Isaac Repentance log folder!\nPress any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            // --- PACKAGE LOGS --- ///
            Console.Write("  Packaging logs...");
            string tempPath = Path.GetTempPath() + "\\EpiphanyDiag";
            if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
            Directory.CreateDirectory(tempPath);
            Directory.CreateDirectory(tempPath + "\\logs");

            foreach (string logFile in logFiles)
            {
                File.Copy(logFile, tempPath + "\\logs\\" + Path.GetFileName(logFile));
            }

            TarFile.CreateFromDirectory(tempPath + "\\logs", tempPath + "\\logs.tar", false);
            Console.WriteLine("done");



        }
    }
}