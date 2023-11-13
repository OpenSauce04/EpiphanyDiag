namespace EpiphanyDiag

open System
open System.IO
open System.Reflection

module Strings =

    let private version = Assembly.GetExecutingAssembly().GetName().Version
    let VersionNumber = $"{version.Major}.{version.Minor}.{version.Build}"
    let TempDir = Path.GetTempPath() + "\\EpiphanyDiag"
    let TarFile = "EpiphanyDiagnostics.tar"
    let GzipFile = "EpiphanyDiagnostics.tar.gz"
    let ModDirList = "modDirList.txt"
    let ModList = "modNameList.txt"
    let ModListEnabled = "modNameListEnabled.txt"
    let IsaacLogDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\My Games\\Binding of Isaac Repentance"
    let AltIsaacLogDir = "..\\..\\Documents\\My Games\\Binding of Isaac Repentance"
    let LogDir = "logs"
    let DataDir = "modData"
    let EpiphanyManifest = "epiphanyManifest.xml"
    let WarningLog = "warningLog.txt"

    module Error =
        let E1 = "ERROR 1: Could not find Isaac Repentance log folder!"
        let E2 = "ERROR 2: This program is being run outside of the Epiphany folder."
        let E3 = "ERROR 3: This mod is not placed in the Isaac mods folder.\nPlease place Epiphany in the same folder as the rest of your mods"
        let E4 modIndex = "ERROR 4: An XML exception has occurred. Name #" + string(modIndex + 1) + " has been skipped."