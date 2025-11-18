[Memory Bank: Active]

# ColourHaxStudio — Application Services

Purpose: define the application-layer API surface and DTOs that sit between presentation (UI) and the ColourHax domain.

Key references
- UI entry: [`Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs`](Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs:1)
- UI viewmodel: [`Mapping_Tools/Viewmodels/ComboColourStudioVm.cs`](Mapping_Tools/Viewmodels/ComboColourStudioVm.cs:1)
- Existing project model: [`Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs`](Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs:1)

Application ports and DTOs

```csharp
// Repository port used by application services
public interface IBeatmapRepository
{
    // Load beatmap by file path or id
    Beatmap Load(string path);
    // Persist changes to the beatmap (overwrites existing file)
    void Save(Beatmap beatmap, string path);
}

// Core application service for ColourHax operations
public interface IColourHaxService
{
    // Produce an in-memory preview: returns mapping of combo index -> resolved colour
    PreviewResult Preview(PreviewRequest request);

    // Apply project changes to a beatmap and return an export result
    ExportResult Apply(ApplyRequest request);

    // Import colour data from a beatmap into a new ComboColourProject DTO
    ComboColourProjectDto ImportFromBeatmap(ImportRequest request);
}

// Simple DTO for export outcomes
public sealed class ExportResult
{
    public bool Success { get; }
    public string Message { get; }
    public string? OutputPath { get; }
}
```

Primary use cases (inputs / outputs)
- PreviewColourHax
  - Input: PreviewRequest { BeatmapPath, ComboColourProject (or project id), TimeWindow? }
  - Output: PreviewResult { IReadOnlyDictionary<int, ComboColourDto> ComboColours, Diagnostics[] }
  - Intent: fast, non-mutating colour resolution for UI preview.

- ApplyColourHax
  - Input: ApplyRequest { BeatmapPath, ComboColourProject, Options (overwrite/backups) }
  - Output: ExportResult
  - Intent: apply resolved colours to beatmap objects and persist via IBeatmapRepository.Save.

- ImportColourHaxFromBeatmap
  - Input: ImportRequest { BeatmapPath }
  - Output: ComboColourProjectDto
  - Intent: extract existing combo colours and produce a serialisable project.

Example sequence (high-level)
- Presentation → calls viewmodel which constructs a PreviewRequest using [`Mapping_Tools/Viewmodels/ComboColourStudioVm.cs`](Mapping_Tools/Viewmodels/ComboColourStudioVm.cs:1).
- Application → `IColourHaxService.Preview()` resolves colour mapping using domain services.
- Domain → domain model (ComboColourProject with MaxBurstLength config) applies deterministic rules and returns a resolution.
- Infrastructure → `IBeatmapRepository` adapters load/save via existing beatmap parsing at [`Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs`](Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs:1).

Notes
- Keep DTOs simple and serialisable primitives only (ints, strings, arrays).
- Application services orchestrate transactions (backup → domain apply → persist) but contain no colour algorithm logic.