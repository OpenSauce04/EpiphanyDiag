﻿namespace EpiphanyDiag

open System.Collections.Generic

module BadMod =

    type Severity =
        | None
        | Low
        | Medium
        | High
        | Inconsistent

    let Check modName =
        try
            ([ 

                "Godhead", Severity.High;
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

            ]|>Map.ofList)[modName]

        with
            | :? KeyNotFoundException -> Severity.None