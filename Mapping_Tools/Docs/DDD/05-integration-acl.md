# Integration & Anti-Corruption Layers

This document identifies external integration boundaries and anti-corruption patterns required for the target architecture.

## Integration Categories

0. **osu! Beatmap File Integration (Stable/Lazer)**
   - Current responsibilities: reading/writing `.osu` files and handling format variation.
   - Future ACL: `IBeatmapFormatReader`, `IBeatmapFormatWriter`, `IBeatmapFormatVersionPolicy`.

1. **Editor/Mem Integration**
   - Current responsibilities: editor state read, selection/time access, save-protection behavior.
   - Future ACL: `IEditorReaderGateway`, `IEditorSelectionGateway`, `IEditorSavePolicyGateway`.

2. **File System & Asset Integration**
   - Current responsibilities: beatmap discovery, mapset traversal, sample file operations.
   - Future ACL: `IBeatmapFileStore`, `ISampleAssetStore`, `IPathPolicy`.

3. **Settings & Configuration Integration**
   - Current responsibilities: app/tool settings persistence and retrieval.
   - Future ACL: `ISettingsStore`, `IToolSettingsStore`.

4. **Update/Network Integration**
   - Current responsibilities: update checking/downloading.
   - Future ACL: `IUpdateFeedClient`, `IUpdatePackageInstaller`.

5. **Host Notifications / UX Feedback**
   - Current responsibilities: message queue, popups, host notifications.
   - Future ACL: `IUserNotifier`, `IProgressReporter`.

## ACL Mapping Rules

- Domain and application layers talk to interfaces only.
- Infrastructure adapters map external data formats to domain/application contracts.
- No external library types pass through domain boundaries.
- Translation logic belongs in ACL adapters, not in domain services.
- Tool assemblies should interface with external systems through framework-provided contracts first.

## Legacy Coupling Hotspots (to protect against)

- Static host state coupling (app globals).
- UI types in non-presentation logic.
- Direct use of dialog/process/memory APIs from tool logic.

## ACL Adapter Responsibilities

Each adapter must explicitly define:

- accepted input contract
- produced output contract
- error mapping policy (external exception -> application error)
- fallback behavior when integration is unavailable

For beatmap format adapters, also define:
- supported osu! format range (Stable/Lazer compatibility)
- lossy vs lossless mapping behavior
- unknown-field preservation policy

## Non-Goals for This Phase

- No runtime adapter implementation yet.
- No migration of existing code paths.
- Only communication architecture and contracts are defined.
