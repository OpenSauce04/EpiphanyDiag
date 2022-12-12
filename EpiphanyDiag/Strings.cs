
using System.IO;

namespace EpiphanyDiag
{
	public static class Strings
	{
		public static string versionNumber = "v1.3";

		public static string tempDir = Path.GetTempPath() + "\\EpiphanyDiag";
		public static string tarFile = "EpiphanyDiagnostics.tar";
		public static string gzipFile = "EpiphanyDiagnostics.tar.gz";
		public static string modDirList = "modDirList.txt";
		public static string modList = "modNameList.txt";
		public static string modListEnabled = "modNameListEnabled.txt";
		public static string logDir = "logs";
		public static string dataDir = "modData";
		public static string epiphanyManifest = "epiphanyManifest.xml";
		public static class Error
		{
			public static string e1 = "ERROR 1: Could not find Isaac Repentance log folder!";
			public static string e2 = "ERROR 2: This program is being run outside of the Epiphany folder.";
			public static string e3 = "ERROR 3: This mod is not placed in the Isaac mods folder.\nPlease place Epiphany in the same folder as the rest of your mods";
			public static string e4(int i) {return "ERROR 4: An XML exception has occurred. Name #"+(i + 1)+" has been skipped.";}
		}
	}
}
