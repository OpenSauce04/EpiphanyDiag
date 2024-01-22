namespace EpiphanyDiag

open System
open System.IO
open System.Reflection

module Strings =

    let private version = Assembly.GetExecutingAssembly().GetName().Version
    let VersionNumber = $"rev. {version.Major}"
    let TempDir = Path.GetTempPath() + "\\EpiphanyDiag"
    let TarFile = "EpiphanyDiagnostics.tar"
    let GzipFile = "EpiphanyDiagnostics.tar.gz"
    let ModDirList = "modDirList.txt"
    let ModList = "modNameList.txt"
    let ModListEnabled = "modNameListEnabled.txt"
    let IsaacLogDirs = [
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\My Games\\Binding of Isaac Repentance"; // Steam log directory
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\My Games\\Binding of Isaac Repentance (Epic)"; // EGS log directory
                        "..\\..\\Documents\\My Games\\Binding of Isaac Repentance"; // Fallback log directory (used in case the usual directory is unaccessible)
                       ] 
    let LogDir = "logs"
    let DataDir = "modData"
    let EpiphanyManifest = "epiphanyManifest.xml"
    let WarningLog = "warningLog.txt"

    module Error =
        let E1 = "ERROR 1: Could not find Isaac Repentance log folder!"
        let E2 = "ERROR 2: This program is being run outside of the Epiphany folder.\nPlease move EpiphanyDiag.exe into the same folder as Epiphany's 'metadata.xml' file and try again."
        let E3 = "ERROR 3: This mod is not placed in the Isaac mods folder.\nPlease place Epiphany in the 'mods' folder under your Isaac installation."
        let E4 modIndex = "ERROR 4: An XML exception has occurred. Name #" + string(modIndex + 1) + " has been skipped."