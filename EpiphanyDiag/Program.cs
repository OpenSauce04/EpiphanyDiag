using System;
using System.Formats.Tar;
using System.IO;
using System.IO.Compression;

namespace EpiphanyDiag
{
	internal class Program
	{
        static string ErrorString = "";
		static void Main(string[] args)
		{
            Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.Write("Epiphany ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Diagnostics Tool v1.0 | ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Written by OpenSauce\nSource available at: https://github.com/OpenSauce04/EpiphanyDiag\n");
            
			string[] logFiles = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\My Games\\Binding of Isaac Repentance", "*.txt", SearchOption.TopDirectoryOnly);

			bool validFolder = false;
            foreach (string filePath in logFiles) {
                if (filePath.Contains("log.txt")) {
                    validFolder = true;
                    break;
                } else {
                    ErrorString = "ERROR 1: Could not find Isaac Repentance log folder!";
                }
            }

			if (new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Name != "mods")
			{
				validFolder = false;
				ErrorString = "ERROR 3: This mod is not placed in the Isaac mods folder.\nPlease place Epiphany in the same folder as the rest of your mods";
			}

			if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\metadata.xml"))
            {
                validFolder = false;
                ErrorString = "ERROR 2: This program is being run outside of the Epiphany folder.";
            }

			if (validFolder) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Isaac Repentance log folder found.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Generating diagnostics package...");
			} else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ErrorString);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            // --- PACKAGE LOGS --- //
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  Grabbing logs...");
            if (Directory.Exists(Strings.tempDir)) Directory.Delete(Strings.tempDir, true);
            Directory.CreateDirectory(Strings.tempDir);
            Directory.CreateDirectory(Strings.tempDir + "\\" + Strings.logDir);

            foreach (string logFile in logFiles)
            {
                File.Copy(logFile, Strings.tempDir + "\\" + Strings.logDir + "\\" + Path.GetFileName(logFile));
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("done");

            // --- GET MOD LIST --- //
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  Getting mod list...");
            string[] modList = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "..\\"); // From the Epiphany folder, this should be the mods folder
            for (int i = 0; i < modList.Length; i++)
            {
                modList[i] = Path.GetFileName(modList[i]);
            }
                File.WriteAllLines(Strings.tempDir + "\\" + Strings.modList, modList);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("done");

            // --- PACKAGE FILES --- //
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  Packaging files...");
            if (File.Exists(Strings.tempDir + "\\..\\" + Strings.tarFile)) File.Delete(Strings.tempDir + "\\..\\" + Strings.tarFile);
            TarFile.CreateFromDirectory(Strings.tempDir, Strings.tempDir + "\\..\\" + Strings.tarFile, false);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("done");

            // --- COMPRESS FILES --- //
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  Compressing files...");
            using FileStream tarFile = File.Open(Strings.tempDir + "\\..\\" + Strings.tarFile, FileMode.Open);
            using FileStream gzipFile = File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\" + Strings.gzipFile);
            using var compressor = new GZipStream(gzipFile, CompressionMode.Compress);
            tarFile.CopyTo(compressor);
            tarFile.Close();
            compressor.Flush();
            compressor.Close();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("done");

            // --- CLEAN UP --- //
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  Cleaning up...");
            Directory.Delete(Strings.tempDir, true);
            File.Delete(Strings.tempDir + "\\..\\" + Strings.tarFile);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("done");

            Console.WriteLine("Finished!\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Diagnostic information saved to " + Strings.gzipFile);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}