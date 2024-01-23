namespace EpiphanyDiag

open System
open System.Formats.Tar
open System.IO
open System.IO.Compression
open System.Xml
open System.Linq

open Helpers
open ModBlacklist

module Main =

    cprintf ConsoleColor.DarkRed "Epiphany "
    cprintf ConsoleColor.White "Diagnostics Tool"
    cprintf ConsoleColor.Gray $" ({Strings.VersionNumber}) | "
    cprintfn ConsoleColor.DarkGray "Written by OpenSauce\nSource available at: https://github.com/OpenSauce04/EpiphanyDiag\n"

    let mutable logFiles = Array.zeroCreate 0

    for path in Strings.IsaacLogDirs |> List.rev do
        try 
            logFiles <- Directory.GetFiles(path, "*.txt", SearchOption.TopDirectoryOnly)
        with | :? DirectoryNotFoundException -> 
            ()
            
    let mutable validFolder = false

    for filePath in logFiles do
        if filePath.Contains("log.txt") then
            validFolder <- true

    if validFolder = false then
        throw Strings.Error.E1

    if (new DirectoryInfo(Directory.GetCurrentDirectory())).Parent.Name <> "mods" then
        if not (File.Exists(Directory.GetCurrentDirectory() + "\\metadata.xml")) then
            throw Strings.Error.E2
        else
            throw Strings.Error.E3

    cprintfn ConsoleColor.Green "Isaac Repentance log folder found."
    cprintfn ConsoleColor.Yellow "Generating diagnostics package..."

    // --- COPY LOGS --- //
    cprintf ConsoleColor.Yellow "  Grabbing logs..."

    if (Directory.Exists(Strings.TempDir)) then Directory.Delete(Strings.TempDir, true)
    Directory.CreateDirectory(Strings.TempDir) |> ignore
    Directory.CreateDirectory(Strings.TempDir + "\\" + Strings.LogDir) |> ignore

    for logFile in logFiles do
        File.Copy(logFile, Strings.TempDir + "\\" + Strings.LogDir + "\\" + Path.GetFileName(logFile))

    cprintfn ConsoleColor.Green "done"


    // --- GRAB EPIPHANY MANIFEST --- //
    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\metadata.xml", Strings.TempDir + "\\" + Strings.EpiphanyManifest)


    // --- GET MOD LIST --- //
    cprintf ConsoleColor.Yellow "  Getting mod list..."
    let rawModList = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "..\\") // From the Epiphany folder, this should be the mods folder
    let modDirList = Array.create rawModList.Length ""

    for i = 0 to rawModList.Length-1 do
        modDirList[i] <- Path.GetFileName(rawModList[i])

    File.WriteAllLines(Strings.TempDir + "\\" + Strings.ModDirList, modDirList)

    let modList = Array.create rawModList.Length ""
    let modListEnabled = Array.create rawModList.Length ""
    let mutable warnList = ""

    for  i = 0 to rawModList.Length-1 do
        if File.Exists(rawModList[i] + "\\metadata.xml") then
            try
                let infodoc = new XmlDocument()
                infodoc.Load(rawModList[i] + "\\metadata.xml")

                modList[i] <- infodoc.GetElementsByTagName("name").[0].InnerXml
                if not (File.Exists(rawModList[i] + "\\disable.it")) then
                    modListEnabled[i] <- infodoc.GetElementsByTagName("name").[0].InnerXml
            with
                | :? XmlException -> printf "\n%s" (Strings.Error.E4(Path.GetFileName(rawModList[i])))
                                     warnList <- warnList + $"Metadata in mod directory '{Path.GetFileName(rawModList[i])}' caused an XML exception\n"

    let modListEnabledClean = modListEnabled.Where( fun x -> not (String.IsNullOrEmpty(x)) ).ToArray()

    Array.Sort(modDirList, StringComparer.Ordinal)
    Array.Sort(modListEnabledClean, StringComparer.Ordinal)

    File.WriteAllLines(Strings.TempDir + "\\" + Strings.ModList, modList)
    File.WriteAllLines(Strings.TempDir + "\\" + Strings.ModListEnabled, modListEnabledClean)

    cprintfn ConsoleColor.Green "done"


    // --- FIND AND SHOW WARNINGS --- //
    cprintfn ConsoleColor.Yellow "  Checking for problematic mods..."
    
    for modName in modList do
        let issueSeverity = CheckMod(modName)

        if issueSeverity = Severity.Low then
            cprintf ConsoleColor.Yellow $"    Warning Lv1: \"{modName}\" is known to cause minor gameplay issues."
            warnList <- warnList + modName + ": " + "Low Severity"

        elif issueSeverity = Severity.Medium then
            cprintf ConsoleColor.DarkYellow $"    Warning Lv3: \"{modName}\" is known to cause moderate gameplay issues."
            warnList <- warnList + modName + ": " + "Medium Severity"

        elif issueSeverity = Severity.High then
            cprintf ConsoleColor.Red $"    Warning Lv4: \"{modName}\" is known to cause severe gameplay issues! If you are experiencing issues, consider removing it."
            warnList <- warnList + modName + ": " + "High Severity"

        elif issueSeverity = Severity.Inconsistent then
            cprintf ConsoleColor.Magenta $"    Warning Lv2: \"{modName}\" has been observed to cause gameplay issues under specific circumstances."
            warnList <- warnList + modName + ": " + "Inconsistent"
        
        if issueSeverity <> Severity.None then
            if not (modListEnabled.Contains(modName)) then
                cprintf ConsoleColor.Gray " (this mod is disabled)"
                warnList <- warnList + " | DISABLED"
            printf "\n"
            warnList <- warnList + "\n"

    if not (modList.Contains("ModErrorContainer")) then
        cprintfn ConsoleColor.Cyan "    Mod Error Container is not installed. Usage of this mod improves the stability of the game when using mods, and is highly recommended."
        warnList <- warnList + "\nMod Error Container is missing."

    File.WriteAllText(Strings.TempDir + "\\" + Strings.WarningLog, warnList)


    // --- COPY MOD DATA --- //
    cprintf ConsoleColor.Yellow "  Copying mod data..."
    Directory.CreateDirectory(Strings.TempDir + "\\" + Strings.DataDir) |> ignore
    cloneDirectory (AppDomain.CurrentDomain.BaseDirectory + "..\\..\\data\\") (Strings.TempDir + "\\" + Strings.DataDir + "\\")
    cprintfn ConsoleColor.Green "done"


    // --- PACKAGE FILES --- //
    cprintf ConsoleColor.Yellow "  Packaging files..."

    if File.Exists(Strings.TempDir + "\\..\\" + Strings.TarFile) then
        File.Delete(Strings.TempDir + "\\..\\" + Strings.TarFile)

    TarFile.CreateFromDirectory(Strings.TempDir, Strings.TempDir + "\\..\\" + Strings.TarFile, false)
    cprintfn ConsoleColor.Green "done"


    // --- COMPRESS FILES --- //
    cprintf ConsoleColor.Yellow "  Compressing files..."
    let tarFile = File.Open(Strings.TempDir + "\\..\\" + Strings.TarFile, FileMode.Open)
    let gzipFile = File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\" + Strings.GzipFile)
    let compressor = new GZipStream(gzipFile, CompressionMode.Compress)
    tarFile.CopyTo(compressor)
    tarFile.Close()
    compressor.Flush()
    compressor.Close()
    cprintfn ConsoleColor.Green "done"


    // --- CLEAN UP --- //
    cprintf ConsoleColor.Yellow "  Cleaning up..."
    Directory.Delete(Strings.TempDir, true)
    File.Delete(Strings.TempDir + "\\..\\" + Strings.TarFile)
    cprintfn ConsoleColor.Green "done"

    cprintfn ConsoleColor.Green "Finished!"
    cprintfn ConsoleColor.Yellow $"Diagnostic information saved to {Strings.GzipFile}"
    cprintfn ConsoleColor.White "\nPress any key to exit."
    Console.ReadKey() |> ignore