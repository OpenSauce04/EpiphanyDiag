namespace EpiphanyDiag

open System.Collections.Generic

module BadMod =

    type Severity =
        | None
        | Low
        | High

    let Check modName =
        try
            ([ 

                "Godhead", Severity.High;

            ]|>Map.ofList)[modName]

        with
            | :? KeyNotFoundException -> Severity.None