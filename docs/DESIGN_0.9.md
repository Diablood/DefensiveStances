# Design note — 0.9 release preparation

## Goal

Prepare the validated gameplay prototype for a first public alpha without changing runtime behavior.

## Metadata

`About/About.xml` now includes the GitHub repository URL and a concise user-facing description. `About/Preview.png` provides the mod-manager and Workshop preview image. `About/ModIcon.png` provides a compact loading-screen icon.

## Packaging boundary

The repository contains source code, development tools, design notes and regression documentation. Public release archives should contain only runtime and user-facing files.

The packaging helpers create this layout:

```text
DefensiveStances/
├── About/
├── 1.6/
│   ├── Assemblies/DefensiveStances.dll
│   ├── Languages/
│   ├── Patches/
│   └── Textures/
├── LoadFolders.xml
├── LICENSE
└── README.md
```

## Validation

The PowerShell package script checks that the compiled DLL exists and that its file version matches `About.xml`. This prevents publishing an archive with stale compiled code.

The Linux/macOS helper creates the same runtime layout after a successful local build.

## Deferred work before 1.0

- full regression testing with a clean generated distribution ZIP;
- Steam Workshop upload metadata and screenshots;
- compatibility testing with large mod lists, Combat Extended and multiplayer;
- final review of user-facing English and French wording;
- removal of obsolete migrations only after confirming that public saves no longer need them.
