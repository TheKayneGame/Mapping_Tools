# Data Schema — Core Domain Tables

The diagram models core domain tables inferred from the memory bank and code structure: Beatmap, HitObject, TimingPoint, Sample, Project and Backup.

```mermaid
erDiagram
  BEATMAP {
    int beatmap_id PK
    string file_path
    string title
    string artist
    int project_id FK
    int version_token
    datetime last_modified
  }
  HITOBJECT {
    int hitobject_id PK
    int beatmap_id FK
    int time
    string type
    string path_data "slider anchor/curve data"
    int combo
  }
  TIMINGPOINT {
    int timingpoint_id PK
    int beatmap_id FK
    int time
    float bpm
    bool is_redline
    float sv_multiplier
  }
  SAMPLE {
    int sample_id PK
    int beatmap_id FK
    string file_name
    string hash
    int layer_index
    float volume
  }
  PROJECT {
    int project_id PK
    string name
    string file_path
    datetime last_saved
  }
  BACKUP {
    int backup_id PK
    int beatmap_id FK
    string file_path
    datetime created_at
    string reason
  }

  BEATMAP ||--o{ HITOBJECT : contains
  BEATMAP ||--o{ TIMINGPOINT : defines
  BEATMAP ||--o{ SAMPLE : references
  PROJECT ||--o{ BEATMAP : contains
  BEATMAP ||--o{ BACKUP : has

```

## Assumptions
- Attribute names use snake_case identifiers to make PK/FK roles explicit; this is an inferred mapping to a relational schema.
- Version comparison uses a numeric version_token on Beatmap (inferred from GetNewestVersionOrNot) to drive merge decisions.
- Backup is modelled as a first-class entity (backup_id, created_at) referenced by BackupManager.

## Mapping notes
- Beatmap (beatmap_id) is the aggregate root coordinating HitObject, TimingPoint, Sample and Backup entities.
- HitObject and TimingPoint are strongly owned by Beatmap; their lifecycle follows Beatmap persistence operations.
- Backup entries are created before destructive saves and retained until save completes or cleanup runs.
- Project groups beatmaps and stores per-tool project state (ISavable), patterns and project-level configuration.

Related docs: [`1_Context_Map.md`](./docs/ddd/1_Context_Map.md:1) — [`2_Ubiquitous_Language.md`](./docs/ddd/2_Ubiquitous_Language.md:1)