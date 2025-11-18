[Memory Bank: Active]

# ColourHaxStudio — Domain API (Signatures)

Purpose: definitive C#-style API for core domain objects and pure helpers to implement first when extracting domain logic from [`Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs`](Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs:61) and [`Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs`](Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs:1).

Overview
- Implement pure, well-tested helper functions before aggregate mutations.
- Keep domain free of UI types (no ObservableCollection, no ICommand) and no I/O.

Domain API — core class signatures

```csharp
// Beatmap aggregate (domain-level surface)
public class Beatmap {
    public string Path { get; }
    public IReadOnlyList<ComboColour> ComboColours { get; }
    public IReadOnlyList<HitObject> HitObjects { get; }

    // Replace palette (in-memory)
    public void SetComboColours(IReadOnlyList<ComboColour> colours);

    // Apply a ComboColourProject mutation (in-memory). Throws domain exceptions on invariant violation.
    public ApplyResult ApplyColourProject(ComboColourProject project);

    // Recalculate NewCombo and ComboSkip for the current HitObjects based on ComboColours
    public void RecalculateComboSkips();
}
```

```csharp
// Result returned by domain apply operation
public sealed class ApplyResult {
    public bool Success { get; }
    public IReadOnlyList<string> Errors { get; } // domain-level error messages
    public int MapsMutatedCount { get; } // for batch ops (0 or 1 at domain level)
}
```

Pure helper signatures (implement and unit-test first)

```csharp
// Returns the number of hits in the combo starting at 'start'
public static int GetComboLength(IReadOnlyList<HitObject> hitObjects, int startIndex);

// Select the ColourPoint to use for a new combo at the given hit index.
// Must follow selection rules in [`Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs`](Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs:82)
public static ColourPoint SelectColourPointForHit(IReadOnlyList<ColourPoint> points, IReadOnlyList<HitObject> hitObjects, int hitIndex, int maxBurstLength);

// Calculate combo skip using current algorithm
public static int CalculateComboSkip(int lastColourIndex, int targetColourIndex, int paletteCount);

// Map ColourPoint sequence and last index to a resolved target colour index in the palette
public static int ResolveColourIndex(ColourPoint point, int lastColourIndex, IReadOnlyList<ComboColour> palette);
```

Contracts & exceptions (domain)
- Methods must not perform I/O or reference UI types.
- ApplyColourProject throws:
  - ColourNotFoundException when a ColourSequence references a palette name not present in project.
  - InvalidComboSkipException when calculated skip would be out of range.
  - DomainInvariantException for other invariant violations.

Batch/apply usage (application orchestration)
- Application layer will:
  1. Load Beatmap via `IBeatmapRepository.LoadNewestVersion(path)` (infra).
  2. Run `beatmap.SetComboColours(project.ComboColours)` and `beatmap.ApplyColourProject(project)`.
  3. On success, persist via `IBeatmapRepository.Save(beatmap, path)` and create backups via `IBackupAdapter`.

Example minimal method contract

```csharp
public ApplyResult Beatmap.ApplyColourProject(ComboColourProject project) {
    // validate project.ComboColours.Count <= 8
    // for each hitObject where ActualNewCombo && !IsSpinner:
    //   select point = SelectColourPointForHit(...)
    //   targetIndex = ResolveColourIndex(point, lastIndex, project.ComboColours)
    //   comboSkip = CalculateComboSkip(lastIndex, targetIndex, project.ComboColours.Count)
    //   set hitObject.ComboSkip = comboSkip; if !hitObject.NewCombo && comboSkip != 0 => hitObject.NewCombo = true
    //   update lastIndex, lastPoint usage/exception handling for Burst
    // return ApplyResult.Success on no errors
}
```

Testing recommendations
- Write unit tests for each pure helper using sample beatmaps in [`Mapping_Tools_Tests/Resources/`](Mapping_Tools_Tests/Resources:1).
- Domain tests must avoid I/O; use in-memory Beatmap/HitObject instances.

References
- Export algorithm/source: [`Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs`](Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs:61)
- Import heuristics: [`Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs`](Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs:113)
- Beatmap model: [`Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs`](Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs:1)

Notes
- Implement helpers first, then aggregate orchestration; preserve exact semantics to maintain parity with existing behavior.
- After domain unit tests pass, proceed to adapter and application wiring.