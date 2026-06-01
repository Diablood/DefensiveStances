# Defensive Stances 1.0 release design note

## Goal

Freeze the validated runtime behavior and prepare the first stable release candidate.

## Runtime policy

`1.0.0-rc1` contains no gameplay changes compared with `0.9.0`. Any issue discovered during release-candidate testing should be handled as a narrowly scoped corrective patch before the final `v1.0.0` tag.

## Branding policy

The repository uses the project-specific red-and-black demon icon supplied by the project owner at `About/ModIcon.png`. Future patches, archives and release tooling must preserve this file.

## Distribution policy

The runtime ZIP continues to contain only the mod metadata, runtime content, compiled DLL, license and user-facing README. Source code, development tools and design notes remain repository-only files.
