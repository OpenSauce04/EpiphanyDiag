
using System.IO;

namespace EpiphanyDiag
{
	public static class Strings
	{
		public static string tempDir = Path.GetTempPath() + "\\EpiphanyDiag";
		public static string tarFile = "EpiphanyDiagnostics.tar";
		public static string gzipFile = "EpiphanyDiagnostics.tar.gz";
		public static string modList = "modlist.txt";
		public static string logDir = "logs";
	}
}
