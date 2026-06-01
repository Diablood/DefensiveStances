# Defensive Stances 0.6 design note

## Goal

Version 0.6 makes evacuation a maintained state instead of a one-shot move order. A pawn using **Flee to safe area** must remain contained inside the global shelter layer until danger has remained absent for the existing grace period.

## Existing foundation

Earlier versions already stored the pawn's previous allowed area, temporarily assigned the global safe-area layer and restored the previous restriction after danger passed. Version 0.6 keeps that foundation and adds active containment recovery.

## Containment loop

Every 60 ticks, `DefensiveStancesGameComponent` reevaluates active evacuations:

1. invalid pawns or pawns no longer using **Flee to safe area** are restored;
2. a manual allowed-area change wins and stops automated restoration;
3. active danger refreshes the last-danger tick;
4. danger-free time beyond the grace period restores the previous allowed area;
5. while evacuation remains active, `DefensiveEvacuationUtility.MaintainSafeAreaContainment` keeps the pawn inside shelter.

If an automatic job carries the pawn outside the refuge, the mod starts a `Goto` job toward the nearest reachable safe cell. If an automatic movement job is already taking a sheltered pawn out of the safe layer, the job is interrupted so the normal restricted think tree can choose an in-shelter activity.

## Player precedence

Containment does not override direct player control. Drafted pawns and pawns with a player-forced job are left alone temporarily. Their safe-area restriction remains active, and containment resumes after drafted control or the forced order ends.

This preserves the existing regression requirement that forced orders and drafted pawns remain under player control.

## Failure handling

If the safe-area layer becomes empty or unreachable during an active evacuation, the previous allowed area is restored and the existing throttled warning path is reused. Safe-area painting tools notify the game component immediately after each edited cell, so a shelter `Goto` aimed at a deleted cell is cancelled without waiting for the periodic 60-tick containment pass.

Containment-recovery log messages are also throttled to one entry per pawn every 600 ticks.

Vanilla does not expose a direct undrafted move command. Player-precedence testing should therefore use a drafted movement order or an undrafted player-forced priority job outside shelter.
