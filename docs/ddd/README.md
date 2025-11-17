# Domain-Driven Design (DDD) Docs for Mapping Tools

This folder contains a focused DDD documentation set for Mapping Tools. It captures contexts, ubiquitous language, critical flows, the primary aggregate state model, and a core data schema. The goal is to make domain boundaries, models, and refactor priorities explicit to guide architecture and development.

Core files
- [`0_Context_Domain_Map.md`](./docs/ddd/0_Context_Domain_Map.md:1)
- [`1_Context_Map.md`](./docs/ddd/1_Context_Map.md:1)
- [`2_Ubiquitous_Language.md`](./docs/ddd/2_Ubiquitous_Language.md:1)
- [`3_Critical_Flow.md`](./docs/ddd/3_Critical_Flow.md:1)
- [`4_State_Model.md`](./docs/ddd/4_State_Model.md:1)
- [`5_Data_Schema.md`](./docs/ddd/5_Data_Schema.md:1)

Per-tool context details
- [`Context_Details_AutoFailDetector.md`](./docs/ddd/Context_Details_AutoFailDetector.md:1)
- [`Context_Details_RhythmGuide.md`](./docs/ddd/Context_Details_RhythmGuide.md:1)
- [`Context_Details_ComboColourStudio.md`](./docs/ddd/Context_Details_ComboColourStudio.md:1)
- [`Context_Details_MapCleaner.md`](./docs/ddd/Context_Details_MapCleaner.md:1)
- [`Context_Details_PatternGallery.md`](./docs/ddd/Context_Details_PatternGallery.md:1)
- [`Context_Details_Sliderator.md`](./docs/ddd/Context_Details_Sliderator.md:1)
- [`Context_Details_SnappingTools.md`](./docs/ddd/Context_Details_SnappingTools.md:1)
- [`Context_Details_TumourGenerator.md`](./docs/ddd/Context_Details_TumourGenerator.md:1)

Strategic Summary
- Establish explicit bounded contexts: Beatmap Editing, Hitsound Management, Geometry/Snapping, Slider Generation, and Project/Storage.
- Extract the Beatmap aggregate and define repository interfaces (BeatmapRepository, ProjectRepository, SampleRepository); implement concrete filesystem adapters behind these interfaces.
- Implement an Anti-Corruption Layer for EditorReaderStuff exposing a minimal contract (GetNewestVersionOrNot, EditorState) and a MergeAdapter to protect domain invariants.
- Formalize BackupManager as a domain service with explicit contracts (CreateBackup, RestoreBackup, Cleanup) and transactional guarantees for save operations.
- Add tests: unit tests for Timing binary-search invariants and repository contract tests; an integration test for "Save Beatmap with Editor Merge" covering backup→merge→persist and failure/rollback paths.
- Encapsulate SnappingGenerators and SliderGenerators behind domain service APIs to isolate geometry algorithms and enable independent refactoring.
- Follow an incremental migration plan: start by introducing repository interfaces and one integration test, then iteratively extract domain behaviours and anti-corruption adapters.
