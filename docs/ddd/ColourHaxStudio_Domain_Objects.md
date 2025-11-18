[Memory Bank: Active]

# ColourHaxStudio — Domain objects (draft)

Purpose: define the core domain objects and minimal method signatures to implement first when moving domain logic out of the UI into MappingTools.Domain.

References
- Project model: [`Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs`](Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs:1)
- Export/orchestration: [`Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs`](Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs:61)
- Beatmap model: [`Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs`](Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs:1)
- HitObject model: [`Mapping_Tools/Classes/BeatmapHelper/HitObject.cs`](Mapping_Tools/Classes/BeatmapHelper/HitObject.cs:1)

Domain entities & value objects — minimal C#-style signatures

```csharp
// Aggregate root: Beatmap (maps to existing Beatmap)
public class Beatmap {
    public string Path { get; }
    public IList<ComboColour> ComboColours { get; }   // palette
    public IList<HitObject> HitObjects { get; }       // ordered by Time

    // Mutating domain APIs (to be implemented inside aggregate)
    void SetComboColours(IReadOnlyList<ComboColour> colours);
    void ApplyColourProject(ComboColourProject project);
    void RecalculateComboIndices(); // recompute NewCombo/ComboSkip/ColourIndex deterministically
}
```

```csharp
// Value object: ComboColour (maps to SpecialColour)
public class ComboColour {
    public string Name { get; }           // "Combo1", "Combo2"
    public Color Color { get; }           // System.Windows.Media.Color (RGBA)
}
```

```csharp
// Entity: HitObject (existing shape; important fields shown)
public class HitObject {
    public double Time { get; }
    public bool ActualNewCombo { get; }   // computed from beatmap state
    public bool NewCombo { get; set; }    // persisted flag
    public int ComboSkip { get; set; }    // 0..(paletteCount-1)
    public int ColourIndex { get; set; }  // index into ComboColours
    public bool IsSpinner { get; }        // ignore spinners for colour hax
}
```

```csharp
// Value object: ColourPoint (maps to existing ColourPoint)
public class ColourPoint {
    public double Time { get; }                               // ms
    public ColourPointMode Mode { get; }                      // Normal | Burst
    public IReadOnlyList<ComboColour> ColourSequence { get; } // repeating pattern
}
```

```csharp
// Aggregate: ComboColourProject (maps to existing ComboColourProject)
public class ComboColourProject {
    public IList<ColourPoint> ColourPoints { get; }   // timeline points
    public IList<ComboColour> ComboColours { get; }   // project palette
    public int MaxBurstLength { get; set; }           // configuration (default 1)

    // Pure-domain helpers
    ComboColourProject Clone();
    void MatchComboColourReferences(); // sync sequence entries to palette instances
}
```

Important invariants (the Beatmap and Project must enforce)
- HitObjects list remains strictly time-ordered.
- ComboColours.Count ≤ 8 (osu! palette limit).
- ComboSkip ∈ [0, ComboColours.Count).
- ColourSequence entries reference palette items by Name; missing names are a domain error.
- MaxBurstLength > 0.

Key domain helper signatures (pure functions to implement and test first)

```csharp
// Calculate skip used by current algorithm
public static int CalculateComboSkip(int lastColourIndex, int targetColourIndex, int paletteCount);

// Select the ColourPoint that applies to the given hit object
public static ColourPoint SelectColourPointForHit(HitObject hit, IReadOnlyList<ColourPoint> points, int maxBurstLength);

// Compute combo length starting at a given hit object
public static int GetComboLength(IList<HitObject> hitObjects, HitObject start);

// Resolve colour index for a new combo based on a ColourPoint and project palette
public static int ResolveColourIndex(ColourPoint point, int lastColourIndex, IReadOnlyList<ComboColour> palette);
```

Mapping notes (where logic currently lives)
- Sequence detection / import heuristics: [`Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs`](Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs:113)
- Export algorithm (selection, skip calc, saving): [`Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs`](Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs:61)
- Editor-memory pref / save: [`Mapping_Tools/Classes/ToolHelpers/EditorReaderStuff.cs`](Mapping_Tools/Classes/ToolHelpers/EditorReaderStuff.cs:1) and [`Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs`](Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs:1)

Next steps (recommended implementation order)
1. Implement and unit-test pure helpers above in MappingTools.Domain (CalculateComboSkip, SelectColourPointForHit, GetComboLength, ResolveColourIndex).
2. Create immutable/explicit domain types (ComboColour, ColourPoint, ComboColourProject DTOs) and tests for MatchComboColourReferences behaviour.
3. Implement Beatmap.ApplyColourProject as an orchestration that uses the pure helpers (but keep repository persistence in adapters).
4. Replace calls in [`Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs`](Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs:61) with a thin adapter that invokes MappingTools.Domain functions.

Verification criteria for domain objects
- Unit tests for pure helpers must pass and match existing behaviour (use sample beatmaps in tests resources).
- No file I/O in domain tests (use in-memory Beatmap representations).
- Behaviour parity: applying domain helpers to existing UI data yields same ComboSkip and ComboColours as current export.
