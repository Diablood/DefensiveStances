# Defensive Stances — Workshop description draft

**Defensive Stances** adds new defensive behavior choices for your colonists while keeping RimWorld's vanilla hostility responses intact.

## Additional pawn doctrines

### Flee to safe area

Paint dedicated shelter cells from **Architect** → **Zone**, then select **Flee to safe area** in a pawn's existing hostility-response dropdown.

When danger reaches that pawn, ordinary automatic work is interrupted and the pawn heads toward an accessible shelter. The pawn remains contained while hostile threats remain active on the map, then returns to the previous allowed area after a configurable grace period.

### Self-defense only

The pawn ignores nearby hostiles until directly attacked, then retaliates against the aggressor. Direct ranged misses, shots intercepted by cover and missed or dodged melee attacks count as attacks. Automatic melee retaliation respects the pawn's vanilla allowed area; ranged retaliation can fire at an aggressor outside that area only when the pawn can shoot from its current position.

## Dedicated safe areas

Safe areas are independent from stockpiles, growing zones and normal allowed areas. They can overlap ordinary zones, and one map can contain several disconnected shelters.

A bottom-right toggle displays or hides the shelter overlay.

## Global emergency evacuation

Use the bottom-right siren to send every undrafted controllable pawn on the current map to shelter immediately, regardless of individual doctrine.

- Drafted pawns remain under player control.
- Undrafted pawns return to shelter while the alarm remains active.
- Unreachable pawns generate clickable warnings.
- The alarm state persists in saved games and is stored independently for each map.

## Settings

Open **Options** → **Mod settings** → **Defensive Stances** to configure:

- restoration grace period;
- containment check interval;
- transient safe-area warnings;
- vanilla fleeing fallback when no usable shelter exists.

## Requirements

- RimWorld 1.6
- Harmony

## Languages

- English
- French

## Notes

This is a stable public release. Reports about Combat Extended, multiplayer and large mod-list compatibility remain welcome.
