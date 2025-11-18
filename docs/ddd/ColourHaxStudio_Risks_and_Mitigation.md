[Memory Bank: Active]

# ColourHaxStudio — Risks and mitigation

Purpose: enumerate migration risks, mitigation strategies and estimated effort for Step 1 and Step 2.

Risks (high-level)
- Behavioural regression: UI preview/apply changes due to wiring or adapter bugs.
- Data loss: incorrect save operations or broken backup creation.
- Hidden coupling: viewmodels or other tools depending on legacy internals.
- Test fragility: existing brittle tests fail after refactor.
- Performance regressions: domain logic slower on large beatmaps.
- Release friction: packaging multi-project solution and CI changes.

Risk mitigations (concrete)
- Behavioural regression
  - Mitigation: Ports & Adapters first; adapters delegate to existing code paths. Validate with smoke tests and MD5/diff checks.
  - Verification: run UI preview and Apply against sample beatmaps; compare outputs to legacy results.
- Data loss
  - Mitigation: Ensure BackupManager invoked before any Save; require explicit backup flag in ApplyRequest.
  - Verification: automated test that applies changes to a copy and verifies originals unchanged.
- Hidden coupling
  - Mitigation: Add integration tests that exercise viewmodel → adapter boundary using fakes; keep viewmodel unchanged.
  - Verification: code search for direct references to legacy internal types and fix via adapter.
- Test fragility
  - Mitigation: Move pure logic tests into MappingTools.Domain.Tests; replace file I/O in tests with FakeBeatmapRepository.
  - Verification: CI runs domain tests in isolation and reports coverage.
- Performance regressions
  - Mitigation: Benchmark domain algorithms with large sample maps; keep previous algorithm where needed as fallback.
  - Verification: compare execution time before/after on worst-case resource files.
- Release friction
  - Mitigation: scaffold new projects as placeholders; add CI job incrementally and keep single-solution build green.

Estimated scope and effort
- Step 1: Ports & Adapters
  - LOC: ~120–300 lines across 3–5 files (IBeatmapRepository, IColourHaxService ports; BeatmapFileRepository adapter; LegacyColourHaxAdapter).
  - Tasks: create projects, add interfaces in MappingTools.Domain/Ports, implement adapters in MappingTools.Infrastructure/Repositories, wire view constructor.
  - Time: 1–2 developer days (including tests and CI tweak).

- Step 2: Domain extraction (pure)
  - LOC: ~800–1600 lines (domain models, policies, resolver, unit tests).
  - Tasks: implement ColourHax policies (GradientPolicy, PaletteMapPolicy, ReplacePolicy), move deterministic algorithms, add domain tests, replace adapter internals.
  - Time: 3–7 developer days (algorithm verification + test coverage).

Confidence and contingency
- Confidence: moderate — domain algorithms are deterministic but require careful test coverage.
- Contingency: reserve one additional sprint day for unexpected coupling or test maintenance.

Monitoring and verification checklist (post-change)
- All unit tests pass (legacy + new domain tests).
- Smoke test: ApplyColourHax on sample resource → validate output via diff/MD5.
- UI preview matches legacy view outputs for representative projects.
- Backups verified and restorable.
- CI green for multi-project solution.

Quick references
- UI entry: [`Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs`](Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs:1)
- Legacy project model: [`Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs`](Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs:1)
- ViewModel: [`Mapping_Tools/Viewmodels/ComboColourStudioVm.cs`](Mapping_Tools/Viewmodels/ComboColourStudioVm.cs:1)

End.