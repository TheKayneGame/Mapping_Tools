[Memory Bank: Active]

# ColourHaxStudio — Migration plan (iterative)

Purpose: step-by-step plan to migrate the existing ComboColourStudio functionality into a DDD-aligned ColourHax domain with minimal risk.

Summary of safe-first steps
- Step 1 — Ports & Adapters (safe, reversible)
  - Introduce interface ports for persistence and service orchestration.
  - Provide infrastructure adapters that delegate to the current implementation.
  - Wire UI to the new port surface without changing existing behaviour.
- Step 2 — Extract pure domain logic (test-first)
  - Move deterministic colour-transformation logic into a new domain project.
  - Keep all adapters and UI calls stable; swap implementations to domain gradually.
- Step 3 — Full aggregate and invariants (final)
  - Model Beatmap and ComboColourProject as aggregate roots in the domain project.
  - Replace in-place file mutations with domain operations and infrastructure persistence adapters.

Concrete file-level actions
- Create project placeholders (empty class library projects)
  - MappingTools.Domain (new)
    - Create folders: /Ports, /Model, /Services
  - MappingTools.Application (new)
    - Create folders: /Services, /DTOs
  - MappingTools.Infrastructure (new)
    - Create folders: /Repositories, /Adapters

- Step 1 actions (Ports & Adapters)
  - Add port interfaces in MappingTools.Domain/Ports:
    - IBeatmapRepository.cs (Load/Save), IColourHaxService.cs (Preview/Apply/Import).
  - Add lightweight adapters in MappingTools.Infrastructure/Repositories that call existing code:
    - BeatmapFileRepository.cs → uses [`Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs`](Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs:1) and [`Mapping_Tools/Classes/BeatmapHelper/BeatmapEditor.cs`](Mapping_Tools/Classes/BeatmapHelper/BeatmapEditor.cs:1).
  - Add an adapter/service implementation in MappingTools.Infrastructure that wraps the current project model:
    - LegacyColourHaxAdapter.cs → delegates to [`Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs`](Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs:1).
  - Wire view to port:
    - Introduce constructor overload or service locator call in [`Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs`](Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs:1) to resolve `IColourHaxService` instead of directly calling VM helpers.

- Step 2 actions (Domain extraction)
  - Move pure algorithms into MappingTools.Domain/Model (no framework refs):
    - Colour resolver, Policy implementations (GradientPolicy, PaletteMapPolicy, ReplacePolicy).
  - Replace LegacyColourHaxAdapter internals to call domain services instead of current model.
  - Add unit tests in MappingTools.Domain.Tests for deterministic behaviour; keep existing [`Mapping_Tools_Tests/Classes/ComboColourStudio/ComboColourProjectTests.cs`](Mapping_Tools_Tests/Classes/ComboColourStudio/ComboColourProjectTests.cs:1) running against a compatibility shim.

- Step 3 actions (Full aggregate)
  - Implement Beatmap aggregate and domain-level Save/Apply operations in MappingTools.Domain.
  - Archive or remove legacy direct-edit paths in favour of domain-engineered operations.
  - Add integration adapters in MappingTools.Infrastructure that translate domain aggregates to file-based persistence.

Change gating checklist (run after each step)
- All unit tests pass (Mapping_Tools_Tests and new domain tests).
- Behavioural parity smoke tests:
  - Create a backup and run an ApplyColourHax on a sample beatmap; compare resulting file MD5 against legacy run (when deterministic) or validate object-level changes via a diff tool.
  - Manual UI preview matches expected combo-colour mapping for sample projects (use UI & Preview).
- No breaking public API changes for other tools.
- Code review completed for the adapter boundary and domain extraction.

Testing & CI
- Add conditional CI job to run MappingTools.Domain tests and the existing Mapping_Tools_Tests.
- After Step 1: run full test suite to ensure adapters preserve behaviour.
- After Step 2: run domain unit tests + adapter integration tests.
- After Step 3: run full integration tests including sample beatmap processing.

Rollback & verification steps
- Rollback plan (per step)
  - Step 1 rollback:
    - Revert constructor changes in [`Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs`](Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs:1) to original wiring.
    - Replace MappingTools.Infrastructure adapters with the previous direct calls.
  - Step 2 rollback:
    - Reintroduce legacy implementation in MappingTools.Infrastructure and mark domain code as experimental.
  - Step 3 rollback:
    - Restore prior beatmap save path from source control; re-enable previous direct file edits.

- Verification checklist after rollback
  - Run the same smoke tests described in "Change gating checklist".
  - Verify backups were created and can be restored.
  - Confirm unit tests pass in previous commit.

Notes on minimal invasiveness
- Keep adapters thin and test-covered.
- Do not modify [`Mapping_Tools/Viewmodels/ComboColourStudioVm.cs`](Mapping_Tools/Viewmodels/ComboColourStudioVm.cs:1) beyond dependency injection unless necessary; prefer adapter facades to preserve viewmodel behaviour.
- Prefer constructor injection where feasible; where not possible, use a service locator with a clear migration plan to constructor injection.

Deliverables by step
- Step 1: New projects scaffolded + ports + infrastructure adapters + wiring in View (small change set).
- Step 2: Pure domain classes + unit tests + adapter changes.
- Step 3: Aggregate implementations + migration of persistence + full integration tests.

Acceptance criteria
- UI behaviour unchanged for preview and apply operations.
- Deterministic domain unit tests added and passing.
- No regressions in other tools; CI green.