
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
	}
}
