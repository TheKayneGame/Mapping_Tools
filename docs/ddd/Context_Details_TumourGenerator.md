# Context Details — TumourGenerator

Purpose
TumourGenerator is an algorithmic pattern generator that creates complex, repeatable structures (tumour patterns) using template-based generators (circle, parabola, square, triangle). It is used for creative pattern creation and can output patterns into beatmaps as sequences of HitObjects.

```mermaid
%% mermaid
classDiagram
  class TumourAggregate {
    +Generate(template, options)
    +ExportToBeatmap(beatmap)
  }
  class TumourTemplate {
    string Id
    string Type
    string Parameters
  }
  class TumourLayer {
    string Name
    int Priority
  }
  class TemplateRepository {
    +Get(id)
    +Save(template)
  }
  class BeatmapRepository {
    +Get(path)
    +Save(beatmap)
  }
  class PatternExported <<event>>

  TumourAggregate "1" -- "0..*" TumourLayer : contains
  TumourAggregate ..> TemplateRepository : <<uses>>
  TumourAggregate ..> BeatmapRepository : <<modifies>>
  TumourAggregate --> PatternExported
```

Assumptions
- Tumour templates and layers exist under `TumourGenerating/Options/TumourTemplates/`; repository abstraction is inferred for reuse and persistence.
- Export mechanics use BeatmapRepository/BeatmapEditor for writing into beatmaps.

Related links
- [`README.md`](./docs/ddd/README.md:1)
- [`Context_Details_Sliderator.md`](./docs/ddd/Context_Details_Sliderator.md:1)