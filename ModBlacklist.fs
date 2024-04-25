namespace EpiphanyDiag

open System.Collections.Generic

module ModBlacklist =

    type Severity =
        | None
        | Low
        | Medium
        | High
        | Inconsistent

    let CheckMod modName =
        try
            ([ 

                "[REP] Co-op plus", Severity.High;
                "Mei", Severity.Medium;
                "Maid in the Mist, Playable Character (Rep)", Severity.Inconsistent;
                "Heart tokens for the Lost!", Severity.High;
                "Siren, Playable Character (Rep/AB+)", Severity.Inconsistent;
                "Wario's Cap", Severity.High;
                "Quality Control", Severity.High;
                "!!Mod Config Menu - Continued", Severity.High;
                "!!(REP)Music Mod Callback", Severity.Low;
                "!!!Mod Compatibility Hack", Severity.High;
                "The Philosopher's Stone", Severity.Inconsistent;
                "Beggars Purgatory", Severity.Inconsistent;
                "Better Coop Item Pedestals", Severity.Medium;
                "The Specialist for Good Items", Severity.Medium;
                "Stats+", Severity.Low;
                "Godhead", Severity.Inconsistent;
                "Fancy Costumes", Severity.Medium;
                "Nemesis", Severity.High;
                "Co-op Fixes", Severity.High;
                "Belson", Severity.High;

            ]|>Map.ofList)[modName]

        with
            | :? KeyNotFoundException -> Severity.None
