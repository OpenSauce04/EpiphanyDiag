namespace EpiphanyDiag

open System
open System.IO

module Helpers =

    let consoleColor (fc : ConsoleColor) = 
        let current = Console.ForegroundColor
        Console.ForegroundColor <- fc
        { new IDisposable with
              member x.Dispose() = Console.ForegroundColor <- current }

    // printf statements that allow user to specify output color
    let cprintf color str = Printf.kprintf (fun s -> use c = consoleColor color in printf "%s" s) str
    let cprintfn color str = Printf.kprintf (fun s -> use c = consoleColor color in printfn "%s" s) str

    let throw error =
        cprintfn ConsoleColor.Red "%s" error
        cprintfn ConsoleColor.White "\nPress any key to exit."
        Console.ReadKey() |> ignore
        exit 1
        ()

    let rec cloneDirectory root dest =
        for directory in Directory.GetDirectories(root) do
            let dirName = Path.GetFileName(directory)
            if not (Directory.Exists(Path.Combine(dest, dirName))) then
                Directory.CreateDirectory(Path.Combine(dest, dirName)) |> ignore
            cloneDirectory directory (Path.Combine(dest, dirName))

        for file in Directory.GetFiles(root) do
           File.Copy(file, Path.Combine(dest, Path.GetFileName(file)));