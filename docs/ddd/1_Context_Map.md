# Context Map

The context map below shows inferred bounded contexts for Mapping Tools and their relationships. Names are taken from the project's memory bank and inferred bounded contexts where a dedicated file was not present.

```mermaid
%% mermaid
graph TD
  BE[Beatmap Editing]
  HM[Hitsound Management]
  GS[Geometry / Snapping]
  SG[Slider Generation]
  PS[Project / Storage]
  UI[Presentation / UI]

  UI -->|Customer/Supplier| BE
  UI -->|Customer/Supplier| HM
  BE -->|Conformist| PS
  HM -->|Shared Kernel| PS
  GS -->|Customer/Supplier| BE
  SG -->|Customer/Supplier| BE
  BE -->|Anticorruption Layer| EditorReader[EditorReader Integration]
  EditorReader -->|Supplier| BE
  BackupManager[BackupManager] -->|Supplier| BE
```

Assumptions
- Bounded context names were inferred from memory-bank files (`brief.md`, `architecture.md`, `product.md`) where `bounded-contexts.md` was not present.
- `EditorReader Integration` and `BackupManager` are shown as external/system contexts rather than full bounded contexts.
- Relationship labels chosen by common DDD patterns inferred from architecture (e.g., BE uses EditorReader via an ACL).
- Use [`README.md`](./docs/ddd/README.md:1) for navigation.