# Defensive Stances 1.0.3 drafted disengagement hotfix

## Problem

A pawn using **Self-defense only** remembered its last direct aggressor for the normal retaliation window. If the player drafted the pawn, retreated and later undrafted it after the enemy had stopped pursuing, the remembered aggressor could still be considered valid and the pawn could return to combat automatically.

## Rule

Drafting is an explicit manual takeover. It closes the current automatic self-defense incident by clearing the remembered aggressor immediately.

After undrafting, ordinary work resumes unless a hostile directly attacks the pawn again. A new direct attack records a new aggressor through the existing ranged, melee and damage hooks.

## Scope

This hotfix does not change safe-area evacuation, global emergency evacuation or the handling of downed targets introduced in 1.0.2.
