using System;
using System.Formats.Tar;
using System.IO;
using System.IO.Compression;

namespace EpiphanyDiag
{
	internal class Program
	{
		static void Main(string[] args)
		{
            Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.Write("Epiphany ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Diagnostics Tool v0.1 | ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Written by OpenSauce\nSource available at: https://github.com/OpenSauce04/EpiphanyDiag\n");
            
			string[] logFiles = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\My Games\\Binding of Isaac Repentance", "*.txt", SearchOption.TopDirectoryOnly);

			bool validFolder = false;
            foreach (string filePath in logFiles)
            {
                if (filePath.Contains("log.txt")) {
                    validFolder = true;
                }
            }

            if (validFolder) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Isaac Repentance log folder found.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Generating diagnostics package...");
			} else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Could not find Isaac Repentance log folder!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            // --- PACKAGE LOGS --- //
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  Grabbing logs...");
            string tempPath = Path.GetTempPath() + "\\EpiphanyDiag";
            if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
            Directory.CreateDirectory(tempPath);
            Directory.CreateDirectory(tempPath + "\\logs");

            foreach (string logFile in logFiles)
            {
                File.Copy(logFile, tempPath + "\\logs\\" + Path.GetFileName(logFile));
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
                File.WriteAllLines(tempPath + "\\modlist.txt", modList);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("done");

            // --- PACKAGE FILES --- //
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  Packaging files...");
            if (File.Exists(tempPath + "\\..\\EpiphanyDiagnostics.tar")) File.Delete(tempPath + "\\..\\EpiphanyDiagnostics.tar");
            TarFile.CreateFromDirectory(tempPath, tempPath + "\\..\\EpiphanyDiagnostics.tar", false);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("done");

            // --- COMPRESS FILES --- //
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  Compressing files...");
            using FileStream tarFile = File.Open(tempPath + "\\..\\EpiphanyDiagnostics.tar", FileMode.Open);
            using FileStream gzipFile = File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\EpiphanyDiagnostics.tar.gz");
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
            Directory.Delete(tempPath, true);
            File.Delete(tempPath + "\\..\\EpiphanyDiagnostics.tar");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("done");

            Console.WriteLine("Finished!\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Diagnostic information saved to EpiphanyDiagnostics.tar.gz");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}