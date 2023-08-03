namespace EpiphanyDiag

open System.Collections.Generic

module BadMod =

    type Severity =
        | None
        | Low
        | Mid
        | High
        | Inconsistent

    let Check modName =
        try
            ([ 

                "Godhead", Severity.High;
                "[REP] Co-op plus", Severity.High;
                "Mei", Severity.Mid;
                "Maid in the Mist, Playable Character (Rep)", Severity.Inconsistent;
                "Heart tokens for the Lost!", Severity.High;
                "Siren, Playable Character (Rep/AB+)", Severity.Inconsistent;
                "Wario's Cap", Severity.High;
                "Quality Control", Severity.High;
                "!!Mod Config Menu - Continued", Severity.High;
                "!!(REP)Music Mod Callback", Severity.Low;

            ]|>Map.ofList)[modName]

        with
            | :? KeyNotFoundException -> Severity.None