# Critical Flow — Save Beatmap (Apply Changes with Editor Merge)

This document describes the single most important cross-context operation: "Save Beatmap (Apply Changes with Editor Merge)". It explains the coordination between UI, tool ViewModel, BeatmapEditor, EditorReaderStuff, BackupManager, ProjectManager and the file system when saving while the osu! editor may hold a newer in-memory version.

Sequence diagram

```mermaid
sequenceDiagram
  participant UI as UI/View
  participant VM as UI/ViewModel
  participant MT as MappingTool
  participant BE as BeatmapEditor
  participant ER as EditorReaderStuff
  participant BM as BackupManager
  participant PM as ProjectManager
  participant FS as FileSystem

  UI->>VM: SaveRequested (SaveCommand)
  VM->>MT: ValidateSaveRequest() 
  alt Validation failed
    MT-->>VM: ValidationFailed(reason)
    VM-->>UI: SaveFailed(ValidationFailed)
  else Validation succeeded
    VM-->>BM: BackupRequested(beatmapId) 
    BM-->>VM: BackupCreated(backupId) /* BackupCreated */
    VM->>ER: CheckEditorState(beatmapPath)
    ER-->>VM: EditorState(open, versionToken)
    alt Editor open and newer
      VM->>BE: MergeRequested(editorVersion)
      BE-->>VM: MergePerformed(mergedBeatmap) /* MergePerformed */
      VM->>BE: Persist(mergedBeatmap)
      BE->>FS: WriteFile(mergedBeatmap)
      FS-->>BE: WriteSuccess
      BE-->>PM: ProjectUpdated(beatmapId) /* ProjectUpdated */
      PM-->>VM: ProjectStateUpdated
      VM-->>UI: SaveSucceeded(merged=true) /* SaveSucceeded */
    else Editor closed or same version
      VM->>BE: Persist(diskBeatmap)
      BE->>FS: WriteFile(diskBeatmap)
      FS-->>BE: WriteSuccess
      BE-->>PM: ProjectUpdated(beatmapId)
      PM-->>VM: ProjectStateUpdated
      VM-->>UI: SaveSucceeded(merged=false)
    end
  end

  %% Error path
  BE-->>VM: SaveFailed(reason) /* SaveFailed */
  VM->>BM: RestoreRequested(backupId)
  BM-->>VM: RestoreCompleted(status)
  VM-->>UI: SaveFailed(reason) 
``` 

Determine Save Strategy flowchart

```mermaid
graph TD
  A[User issues Save] --> B{Is map open in editor?}
  B -- Yes --> C{Is merge required?}
  B -- No --> I[Save to disk only]
  C -- Yes --> D[Create backup]
  C -- No --> I
  D --> E{Backup successful?}
  E -- Yes --> F[Perform Merge and Save] 
  E -- No --> H[Abort + Notify]
  F --> G[Write merged file to disk]
  G --> J[SaveSucceeded (merged=true)]
  I --> K[Write disk version to disk]
  K --> L[SaveSucceeded (merged=false)]
  G -- fail --> M[SaveFailed]
  K -- fail --> M
  M --> N[Rollback from Backup]
  N --> O[Notify user + Offer restore]
```

Notes
- Domain events used: SaveRequested, BackupCreated, MergePerformed, SaveSucceeded, SaveFailed, ProjectUpdated.
- Anti-Corruption Layer: EditorReaderStuff should expose a minimal contract (GetNewestVersionOrNot) to keep domain logic isolated.
- Related: [`1_Context_Map.md`](./docs/ddd/1_Context_Map.md:1)