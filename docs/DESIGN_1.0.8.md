# Defensive Stances 1.0.8 self-defense allowed-area hotfix

## Problem

A pawn using **Self-defense only** could receive a direct ranged attack from an aggressor outside the pawn's vanilla allowed area. Once the aggressor was recorded, the automatic retaliation job could select melee and send the pawn across the allowed-area boundary to pursue the attacker.

That contradicts the player's vanilla area restriction and can move a supposedly restricted colonist into an unsafe part of the map.

## Rule

Automatic self-defense must respect the pawn's current vanilla allowed area.

### Melee retaliation

If the remembered aggressor is outside the pawn's vanilla allowed area, Defensive Stances does not create an automatic melee attack job.

If the aggressor later enters the allowed area while the self-defense incident is still recent, normal retaliation may resume.

### Ranged retaliation

A ranged pawn may retaliate against an aggressor outside the vanilla allowed area only when the current attack verb can hit the target from the pawn's present position.

Defensive Stances does not move the pawn outside the allowed area to obtain range or line of sight.

### Unrestricted pawns

If the pawn has no vanilla allowed-area restriction, the existing self-defense behavior remains unchanged.

## Scope

This hotfix does not change:

- how direct aggressors are recorded;
- the self-defense retaliation timeout;
- the downed-target handling introduced in 1.0.2;
- the drafted disengagement behavior introduced in 1.0.3;
- safe-area evacuation or global emergency containment.

Player drafting remains the explicit manual override.