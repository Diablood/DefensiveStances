# 0.4 design note: direct-attack self-defense

## Goal

Make **Self-defense only** react to an actual direct attack attempt instead of waiting exclusively for damage to be dealt.

## Ranged attacks

`Verb_LaunchProjectile.TryCastShot` is patched after a projectile has been launched successfully. When the current target is a pawn configured for self-defense only, the launcher is recorded as the direct aggressor immediately. This also covers shots that subsequently miss or hit cover.

## Melee attacks

`Verb_MeleeAttack.TryCastShot` is patched before the hit, miss and dodge roll. When a valid hostile pawn starts a melee attempt against a pawn configured for self-defense only, the attacker is recorded immediately. This covers successful hits, misses and dodges.

## Damage fallback

The existing `Thing.TakeDamage` patch remains active and now delegates to the shared `DefensiveAggressionUtility`. It continues to cover direct hostile damage sources that do not pass through the standard ranged or melee verbs.

## Scope boundary

The doctrine still responds only to a direct aggressor. Near misses aimed at another pawn, indirect area damage without a hostile instigator and attacks against nearby allies are intentionally outside the 0.4 scope.
