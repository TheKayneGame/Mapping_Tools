[Memory Bank: Active]

# ColourHaxStudio — Domain Model

Purpose: define the ubiquitous language, core domain types, aggregates, invariants and candidate exceptions for the ColourHax (formerly ComboColourStudio) domain based on actual implementation.

Ubiquitous language
- Beatmap — a single .osu beatmap (timing, hit objects, combos and combo colours).
- Combo — a contiguous sequence of hit objects that share a combo index (between NewCombo markers).
- ComboColour — named RGB colour in the beatmap palette (e.g., "Combo1"). Implementation uses `SpecialColour` class from [`Mapping_Tools/Classes/BeatmapHelper/SpecialColour.cs`](Mapping_Tools/Classes/BeatmapHelper/SpecialColour.cs:1).
- ColourPoint — time-indexed marker defining which colour sequence applies at/after that time. Has Mode (Normal or Burst).
- ColourPointMode — enum: Normal (persistent) or Burst (single-use, only for short combos).
- ColourSequence — ordered list of ComboColours referenced by a ColourPoint (defines the repeating pattern).
- Palette — ordered collection of ComboColours in project (max 8 in osu!). Stored as `ComboColours` in current implementation.
- ComboColourProject — serialisable project containing ColourPoints, Palette (ComboColours), and MaxBurstLength configuration.
- MaxBurstLength — integer: combos with length ≤ this value can match Burst ColourPoints. Default is 1 (single hit objects only).

Domain types (actual implementation signatures from [`Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs`](Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs:1))

```csharp
// Value object (existing: SpecialColour)
public class ComboColour
{
    public string Name { get; }           // e.g., "Combo1", "Combo2"
    public Color Color { get; }           // System.Windows.Media.Color (RGBA)
}

// Enum for ColourPoint behavior
public enum ColourPointMode
{
    Normal,  // Persistent: applies to all subsequent combos until next ColourPoint
    Burst    // Single-use: applies once if combo length ≤ MaxBurstLength, then becomes exception
}

// Time-indexed mapping point (existing: ColourPoint)
public class ColourPoint
{
    public double Time { get; }                              // time in milliseconds
    public ColourPointMode Mode { get; }                     // Normal or Burst
    public IReadOnlyList<ComboColour> ColourSequence { get; } // sequence of colours to apply
    public bool IsSelected { get; }                          // UI selection state
    public ComboColourProject ParentProject { get; set; }    // back-reference for name matching
}

// Aggregate root (project-level) - existing: ComboColourProject
public class ComboColourProject
{
    public ObservableCollection<ColourPoint> ColourPoints { get; }    // timeline points
    public ObservableCollection<SpecialColour> ComboColours { get; }  // palette (max 8)
    public int MaxBurstLength { get; set; }                           // burst detection threshold
    
    // Import methods (reverse-engineer from beatmap)
    void ImportColourHaxFromBeatmap(string path);
    void ImportComboColoursFromBeatmap(string path);
    
    // Helper methods
    void MatchComboColourReferences();  // sync ColourSequence references with ComboColours
}

// Beatmap aggregate (existing: Beatmap from BeatmapHelper)
public class Beatmap
{
    public string Path { get; }
    public List<ComboColour> ComboColours { get; set; }      // beatmap's colour palette
    public List<HitObject> HitObjects { get; }               // ordered hit objects
    
    // Domain methods (to be added during DDD refactor)
    void ApplyColourProject(ComboColourProject project);
    void SetComboColours(IReadOnlyList<ComboColour> colours);
}

// HitObject (existing - relevant fields for colour hax)
public class HitObject
{
    public double Time { get; }
    public bool ActualNewCombo { get; }    // computed: is this actually a new combo?
    public bool NewCombo { get; set; }     // user-set new combo flag
    public int ComboSkip { get; set; }     // combo skip value (0-7)
    public int ColourIndex { get; }        // current colour index in palette
    public bool IsSpinner { get; }         // spinners don't use combo colours
}
```

References
- UI and orchestration: [`Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs`](Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs:1)
- Existing project model: [`Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs`](Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs:1)
- ViewModel: [`Mapping_Tools/Viewmodels/ComboColourStudioVm.cs`](Mapping_Tools/Viewmodels/ComboColourStudioVm.cs:1)
- Beatmap model: [`Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs`](Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs:1)
- HitObject model: [`Mapping_Tools/Classes/BeatmapHelper/HitObject.cs`](Mapping_Tools/Classes/BeatmapHelper/HitObject.cs:1)

Aggregate boundaries and invariants

Beatmap aggregate (aggregate root = Beatmap)
- Owns:
  - All HitObjects and their combo assignments (ComboSkip, NewCombo, ColourIndex).
  - The beatmap's ComboColours palette.
- Invariants:
  - HitObjects remain ordered by Time after colour application.
  - ComboColours.Count ≤ 8 (osu! limit).
  - ComboSkip ∈ [0, ComboColours.Count).
  - ColourIndex computation is deterministic based on NewCombo and ComboSkip values.
  - Spinners never trigger new combos for colour purposes.

ComboColourProject aggregate (aggregate root = ComboColourProject)
- Owns:
  - ColourPoints (timeline markers).
  - ComboColours (the project's palette).
  - MaxBurstLength configuration.
- Invariants:
  - ComboColours.Count ≤ 8.
  - Each ColourSequence references ComboColours by Name; all names must exist in ComboColours.
  - MaxBurstLength > 0.
  - ColourPoints are processed in time order (via OrderBy at application time, not enforced in storage).
  - MatchComboColourReferences maintains object identity: ColourSequence items reference same instances as ComboColours.

Domain rules (actual implementation from [`Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs`](Mapping_Tools/Views/ComboColourStudio/ComboColourStudioView.xaml.cs:82))

ColourPoint selection for a new combo:
- Choose last ColourPoint with Time ≤ hitObject.Time + 5ms.
- Exclude Burst points unless hitObject.Time ≥ ColourPoint.Time - 5ms AND combo length ≤ MaxBurstLength.
- Fallback hierarchy: first non-Burst ColourPoint → first ColourPoint overall.

Burst handling:
- Burst ColourPoints are added to exceptions list after first use (single-use within export session).
- Only applied when combo length ≤ MaxBurstLength.
- After burst applied, previous Normal ColourPoint continues.

ComboSkip calculation (lines 145-153 in ComboColourStudioView):
- `comboIncrease = (targetColourIndex - lastColourIndex) mod ComboColours.Count`
- `ComboSkip = (comboIncrease - 1) mod ComboColours.Count`
- If object was not NewCombo and ComboSkip ≠ 0, set NewCombo = true.

ColourSequence matching:
- Each colour in ColourPoint.ColourSequence matched by Name to project's ComboColours.
- Throws ArgumentException if colour Name not found in ComboColours.
- Sequence index cycles: `sequenceIndex = (lastIndex + 1) mod ColourSequence.Count`.

Import heuristics (from [`Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs`](Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs:113)):
- Reverse-engineers colour sequences from existing beatmap combo patterns.
- Tries sequence lengths from 1 to `ComboColours.Count * 2 + 2`.
- Scores sequences using recursive depth-3 search minimizing (sequence length / contribution).
- Detects Burst vs Normal based on contribution = 1 and combo length ≤ MaxBurstLength.
- Optimizes: suppresses redundant ColourPoints when burst follows normal with equivalent subsequence.

Candidate domain exceptions
- ArgumentException: thrown when ColourSequence references colour Name not in ComboColours (current implementation line 140).
- ColourNotFoundException: domain exception for missing palette reference (future).
- InvalidComboSkipException: ComboSkip out of valid range [0, ComboColours.Count).

Notes
- Current implementation mixes UI concerns (ObservableCollection, IsSelected) with domain logic.
- DDD refactor should separate:
  - Pure domain model (immutable or with clear mutation APIs).
  - Application DTOs for UI binding.
  - Infrastructure adapters for persistence and editor integration.
- MaxBurstLength is the ONLY configuration parameter; there is no separate "ColourHaxPolicy" object.