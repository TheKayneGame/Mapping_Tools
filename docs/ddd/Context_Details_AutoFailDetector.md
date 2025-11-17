# Context Details — AutoFailDetector

Purpose
The AutoFailDetector tool analyses a beatmap to detect patterns that would cause automatic failure or are structurally invalid (e.g., unreachable sequences, impossible timings). It is a QA/analysis tool used by mappers to flag issues before saving or publishing.

```mermaid
%% mermaid
classDiagram
  class AutoFailDetectorAggregate {
    +RunAnalysis(Beatmap)
    +GetReport()
  }
  class FailureFinding {
    int Id
    string Description
    int Time
    string Severity
  }
  class Rule {
    string Id
    string Name
    string Predicate
  }
  class HitObjectSnapshot {
    int Time
    string Type
    string Position
  }
  class ReportRepository {
    +Save(Report)
    +Get(id)
  }
  class BeatmapRepository {
    +Get(path)
    +Save(beatmap)
  }
  class FailureDetected <<event>>
  class ReportGenerated <<event>>
  class RunAnalysis <<command>>

  AutoFailDetectorAggregate "1" -- "0..*" FailureFinding : contains
  AutoFailDetectorAggregate "1" -- "0..*" Rule : evaluates
  FailureFinding "1" o-- "1" HitObjectSnapshot : evidence
  AutoFailDetectorAggregate ..> ReportRepository : <<uses>>
  AutoFailDetectorAggregate ..> BeatmapRepository : <<reads>>
  RunAnalysis --> AutoFailDetectorAggregate
  AutoFailDetectorAggregate --> FailureDetected
  AutoFailDetectorAggregate --> ReportGenerated
```

Assumptions
- There is no explicit AutoFail domain class in memory bank; the aggregate and entities are inferred from tool name and typical analyzer patterns.
- Repository interfaces (BeatmapRepository, ReportRepository) are inferred and should map to existing BeatmapEditor / ProjectManager responsibilities.
- Events (FailureDetected, ReportGenerated) are inferred to support UI updates and background processing notifications.

Related links
- [`README.md`](./docs/ddd/README.md:1)
- [`Context_Details_MapCleaner.md`](./docs/ddd/Context_Details_MapCleaner.md:1) — related cleanup tooling