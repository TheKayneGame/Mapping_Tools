# Data Schema — Core ERD

Purpose: Provide a concise, implementation-focused ER view of core domain persistence. The diagrams show primary tables for the core domain and how they relate across bounded contexts.

## Core ER Diagram

```mermaid
erDiagram
    BEATMAP {
      int beatmap_id PK
      string artist
      string title
      string version
    }
    HITOBJECT {
      int hitobject_id PK
      int beatmap_id FK
      int time
      string type
    }
    TIMINGPOINT {
      int timingpoint_id PK
      int beatmap_id FK
      float bpm
      bool redline
    }
    SAMPLE {
      int sample_id PK
      int beatmap_id FK
      string filename
      int index
    }
    PROJECT {
      int project_id PK
      string name
    }
    BACKUP {
      int backup_id PK
      int beatmap_id FK
      datetime created_at
      string path
    }
    BEATMAP ||--o{ HITOBJECT : "has"
    BEATMAP ||--o{ TIMINGPOINT : "has"
    BEATMAP ||--o{ SAMPLE : "contains"
    BEATMAP ||--o{ BACKUP : "backed_by"
    PROJECT ||--o{ BEATMAP : "contains"
```

## Grouped ERD (by Bounded Contexts)

Beatmap Core context

```mermaid
erDiagram
    BEATMAP {
      int beatmap_id PK
      string title
    }
    HITOBJECT {
      int hitobject_id PK
      int beatmap_id FK
    }
    TIMINGPOINT {
      int timingpoint_id PK
      int beatmap_id FK
    }
    BEATMAP ||--o{ HITOBJECT : "has"
    BEATMAP ||--o{ TIMINGPOINT : "has"
```

Hitsound Studio context

```mermaid
erDiagram
    SAMPLE {
      int sample_id PK
      int beatmap_id FK
      string filename
    }
    SAMPLE }o--|| BEATMAP : "belongs_to"
```

Tools / Project & Backup context

```mermaid
erDiagram
    PROJECT {
      int project_id PK
      string name
    }
    BACKUP {
      int backup_id PK
      int beatmap_id FK
      datetime created_at
    }
    PROJECT ||--o{ BEATMAP : "contains"
    BEATMAP ||--o{ BACKUP : "backed_by"
```

## Combined Cross-Context ERD

```mermaid
erDiagram
    BEATMAP {
      int beatmap_id PK
    }
    HITOBJECT {
      int hitobject_id PK
      int beatmap_id FK
    }
    TIMINGPOINT {
      int timingpoint_id PK
      int beatmap_id FK
    }
    SAMPLE {
      int sample_id PK
      int beatmap_id FK
    }
    PROJECT {
      int project_id PK
    }
    BACKUP {
      int backup_id PK
      int beatmap_id FK
    }
    BEATMAP ||--o{ HITOBJECT : "has"
    BEATMAP ||--o{ TIMINGPOINT : "has"
    BEATMAP ||--o{ SAMPLE : "contains"
    PROJECT ||--o{ BEATMAP : "contains"
    BEATMAP ||--o{ BACKUP : "backed_by"
```

## Legend

- PK: Primary key. FK: Foreign key.
- Relationship labels:
  - has — aggregation (one-to-many within aggregate)
  - contains — composition or ownership
  - backed_by — backup / supplier relation
  - belongs_to — cross-context reference

## Assumptions

- PROJECT and BACKUP inferred from Memory Bank and BackupManager usage (Mapping_Tools/Classes/SystemTools/BackupManager.cs) — represent persistent project grouping and backup records for rollback/restore.
- SAMPLE inferred from Hitsound modules (Mapping_Tools/Classes/HitsoundStuff/) — represents persisted sample metadata referenced by Beatmap.
- TIMINGPOINT and HITOBJECT map directly to classes in Mapping_Tools/Classes/BeatmapHelper/ and are modeled as persisted entities owned by the Beatmap aggregate.
- Additional technical fields (artist, version, path) are illustrative and inferred from product.md and Beatmap.cs structure for clarity in ER diagrams.

## Mapping Notes

- Beatmap aggregate persists BEATMAP, HITOBJECT, TIMINGPOINT, and SAMPLE via BeatmapRepository; repository interfaces should align with these tables.
- BackupManager is a domain service that writes BACKUP records before destructive operations; consider transaction/compensating action patterns for rollback.
- PROJECT groups BEATMAPs for multi-difficulty mapset operations and can be the unit for exports and bulk operations.
