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

            // --- PACKAGE LOGS --- //
            Console.Write("  Grabbing logs...");
            string tempPath = Path.GetTempPath() + "\\EpiphanyDiag";
            if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
            Directory.CreateDirectory(tempPath);
            Directory.CreateDirectory(tempPath + "\\logs");

            foreach (string logFile in logFiles)
            {
                File.Copy(logFile, tempPath + "\\logs\\" + Path.GetFileName(logFile));
            }

            Console.WriteLine("done");

            // --- GET MOD LIST --- //
            Console.Write("  Getting mod list...");
            string[] modList = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "..\\"); // From the Epiphany folder, this should be the mods folder
            File.WriteAllLines(tempPath + "\\modlist.txt", modList);
            Console.WriteLine("done");

            // --- PACKAGE FILES --- //
            Console.Write("  Packaging files...");
            if (File.Exists(tempPath + "\\..\\EpiphanyDiagnostics.tar")) File.Delete(tempPath + "\\..\\EpiphanyDiagnostics.tar");
            TarFile.CreateFromDirectory(tempPath, tempPath + "\\..\\EpiphanyDiagnostics.tar", false);
            Console.WriteLine("done");

            // --- COMPRESS FILES --- //
            Console.Write("  Compressing files...");
            using FileStream tarFile = File.Open(tempPath + "\\..\\EpiphanyDiagnostics.tar", FileMode.Open);
            using FileStream gzipFile = File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\EpiphanyDiagnostics.tar.gz");
            using var compressor = new GZipStream(gzipFile, CompressionMode.Compress);
            tarFile.CopyTo(compressor);
            tarFile.Close();
            Console.WriteLine("done");

            // --- CLEAN UP --- //
            Console.Write("  Cleaning up...");
            Directory.Delete(tempPath, true);
            File.Delete(tempPath + "\\..\\EpiphanyDiagnostics.tar");
            Console.WriteLine("done");

            Console.WriteLine("Finished!\nDiagnostic information saved to EpiphanyDiagnostics.tar.gz\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}