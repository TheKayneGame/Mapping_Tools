# Context Details — PatternGallery

Purpose
PatternGallery stores, manages and inserts reusable mapping patterns (OsuPattern). It enables export/import, grouping and quick placement of user-defined patterns into beatmaps.

```mermaid
%% mermaid
classDiagram
  class PatternGalleryAggregate {
    +SavePattern(pattern)
    +PlacePattern(beatmap, pattern, position)
  }
  class OsuPattern {
    int Id
    string Name
    string PatternData
  }
  class PatternRepository {
    +Save(pattern)
    +Load(id)
    +FindByName(name)
  }
  class PatternProject {
    int Id
    string Name
  }
  class PatternPlaced <<event>>

  PatternGalleryAggregate "1" -- "0..*" OsuPattern : stores
  PatternGalleryAggregate ..> PatternRepository : <<uses>>
  PatternGalleryAggregate --> PatternPlaced
```

Assumptions
- OsuPattern and file handlers exist under PatternGallery code; repository interfaces are inferred for persistence.
- Pattern placement interacts with BeatmapRepository/BeatmapEditor for writes.

Related links
- [`README.md`](./docs/ddd/README.md:1)
- [`Context_Details_Sliderator.md`](./docs/ddd/Context_Details_Sliderator.md:1)