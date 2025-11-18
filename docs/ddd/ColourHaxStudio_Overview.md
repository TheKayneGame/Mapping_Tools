[Memory Bank: Active]

# ColourHaxStudio — Executive summary

Purpose: document the migration of the legacy ComboColourStudio tool (misnomer) into a focused ColourHax domain and describe the minimal, safe path to extract domain logic.

What the tool currently does
- Colour editing and combo colour manipulation for osu! beatmaps is exposed through a UI at [`Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs`](Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs:1) and orchestrated by the viewmodel at [`Mapping_Tools/Viewmodels/ComboColourStudioVm.cs`](Mapping_Tools/Viewmodels/ComboColourStudioVm.cs:1). Core project model lives in [`Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs`](Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs:1); further context in [`docs/ddd/Context_Details_ComboColourStudio.md`](docs/ddd/Context_Details_ComboColourStudio.md:1).

High-level DDD goal
- Replace ad-hoc UI-coupled logic with a small, well-defined ColourHax domain:
  - Isolate colour policy and palette transformations from UI and file I/O.
  - Define clear aggregates (Beatmap, ComboColourProject) and domain invariants.
  - Provide application services for preview, apply, import and export.

Recommended iterative approach
1. Ports & Adapters (safe): introduce interfaces (ports) for beatmap persistence and colour application; add infrastructure adapters that delegate to existing code paths. Wire the view to the new service surface to confirm parity.
2. Domain extraction (pure): move deterministic colour-transformation logic into a new domain project (MappingTools.Domain) as pure classes with no framework dependencies.
3. Full aggregate (complete): define Beatmap aggregate roots, enforce invariants, replace in-place file edits with domain-driven operations and add integration adapters in MappingTools.Infrastructure.

Migration principle: small, verifiable steps that keep the UI behaviour unchanged while progressively shifting logic into testable domain code.