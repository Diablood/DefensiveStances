# Defensive Stances 1.0.2 self-defense target hotfix

## Problem

A pawn using **Self-defense only** could keep a recently recorded aggressor for the complete retaliation window after that enemy pawn had fallen down. The target was still spawned and hostile, so the mod could briefly issue new automatic attack jobs while the defender was attempting ordinary work such as hauling.

This produced a visible alternation between work and attack indicators against an incapacitated enemy, even though vanilla does not normally require an automatic follow-up attack against a downed pawn.

## Fix

`DefensivePawnState.TryGetRecentAggressor()` now clears the remembered aggressor when that target is a pawn and is dead or downed:

```csharp
Pawn aggressorPawn = aggressor as Pawn;
if (aggressorPawn != null && (aggressorPawn.Dead || aggressorPawn.Downed))
{
    ClearAggression();
    return false;
}
```

Non-pawn hostile targets, such as hostile buildings, continue to use the existing validation path.

## Compatibility

The change does not alter saved-data structure. A downed aggressor is simply discarded the next time self-defense evaluates the remembered target. If the same enemy later stands up, a new direct attack is required before retaliation resumes.
