# Mapping Tools - Domain Model & Data Ownership Analysis

This document identifies aggregates, their boundaries, invariants, and ownership patterns based on codebase analysis.

## Aggregate Identification

### 1. **Beatmap Aggregate** 🔴 CORE AGGREGATE ROOT

**Aggregate Root**: [`Beatmap`](../../Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs)

**Owned Entities**:
- `List<HitObject>` - Gameplay objects
- `Timing` (contains `List<TimingPoint>`) - All timing points
- `Storyboard` - Visual effects and events
- `List<ComboColour>` - Combo colors
- `Dictionary<string, ComboColour>` - Special colors
- Metadata dictionaries (General, Editor, Metadata, Difficulty)

**Invariants**:
1. HitObjects must be sorted by time
2. Timing points must be sorted by offset
3. At least one uninherited timing point (redline) must exist
4. Slider end times must be calculated before use
5. Combo indices and colors must be consistent
6. All sliders must have valid greenlines assigned

**Consistency Rules**:
- When HitObjects added/removed → Must call `SortHitObjects()`
- When HitObjects modified → Must call `CalculateSliderEndTimes()`, `GiveObjectsGreenlines()`, `CalculateHitObjectComboStuff()`
- When Timing modified → Must call `beatmap.BeatmapTiming.Sort()`
- File format version determines serialization precision

**Modification Methods** (Who can write):
- `BeatmapEditor.SaveFile()` - Persists to disk
- All 20+ tools read and modify through `BeatmapEditor`
- `EditorReaderStuff.UpdateBeatmap()` - Merges memory state from osu! editor

**Source of Truth**:
- **Primary**: `.osu` file on disk
- **Secondary**: osu! editor memory (when editor open)
- **Merge Strategy**: `EditorReaderStuff.GetNewestVersionOrNot()` combines both

---

### 2. **HitObject Entity** (Part of Beatmap Aggregate)

**Not an Aggregate Root** - Always accessed through Beatmap

**Internal Structure**:
- Position, time, type (Circle/Slider/Spinner/HoldNote)
- Hitsounds (Normal/Whistle/Finish/Clap)
- Sample sets and volumes
- Slider-specific: `SliderPath`, curve points, repeats, pixel length
- Timing references: `TimingPoint`, `UnInheritedTimingPoint`, `SliderVelocity`
- Timeline objects (visual representation)
- Body hitsounds (for sliders)

**Invariants**:
1. IsCircle XOR IsSlider XOR IsSpinner XOR IsHoldNote (exactly one true)
2. If IsSlider → Must have valid `SliderPath`, `CurvePoints`, `Repeat >= 1`
3. EndTime >= Time
4. TemporalLength must be calculated via Timing system
5. StackCount must be >= 0

**Owned By**: Beatmap aggregate
**Modified Through**: Beatmap operations only

---

### 3. **Timing Value Object** (Part of Beatmap Aggregate)

**Not an Aggregate Root** - Managed by Beatmap

**Internal Structure**:
- `List<TimingPoint>` - All timing points (sorted)
- `List<TimingPoint>` Redlines - Cached uninherited points
- `List<TimingPoint>` Greenlines - Cached inherited points  
- `double SliderMultiplier` - Global SV multiplier

**Invariants**:
1. All three lists must stay synchronized
2. All lists must remain sorted by offset
3. Binary search optimizations depend on sorted state
4. At least one redline must exist (or synthetic first TP)
5. Redlines list contains only uninherited=true TPs
6. Greenlines list contains only uninherited=false TPs

**Modification Methods**:
- `Add()` - Inserts maintaining sorted order in all 3 lists
- `Remove()` - Removes from all 3 lists
- `SetTimingPoints()` - Replaces all, rebuilds redlines/greenlines
- `Sort()` - Re-sorts all 3 lists

**Critical Operation**: Binary search for timing lookups (`GetRedlineAtTime`, `GetGreenlineAtTime`)

---

### 4. **Hitsound Layer Aggregate** 🟠 HITSOUND DOMAIN

**Aggregate Root**: [`HitsoundLayer`](../../Mapping_Tools/Classes/HitsoundStuff/HitsoundLayer.cs)

**Owned Entities**:
- `List<double> Times` - All times this sound plays
- `SampleGeneratingArgs` - How to generate the sound
- `LayerImportArgs` - Source information for reloading
- `Priority`, `SampleSet`, `Hitsound` - Classification

**Invariants**:
1. Times list should be sorted
2. No duplicate times (within precision)
3. Priority determines mix order
4. ImportArgs must allow reload from same source

**Modification Methods**:
- `Reload()` - Rebuilds times from source layers
- `RemoveDuplicates()` - Deduplicates time list
- Direct manipulation via `Times` property

**Owned By**: `HitsoundStudioVm` - User's hitsound project
**Source of Truth**: User's saved hitsound layers project

---

### 5. **SamplePackage Value Object** 🟠 HITSOUND DOMAIN

**Not an Aggregate Root** - Transient conversion object

**Structure**:
- `double Time` - When all samples play
- `HashSet<Sample> Samples` - All samples at this time

**Purpose**: Groups samples that play simultaneously for export

**Created By**: `HitsoundConverter.ZipLayers()`
**Consumed By**: `HitsoundExporter` and `HitsoundConverter.GetCompleteHitsounds()`
**Lifetime**: Temporary during hitsound generation process

**Invariants**:
1. All samples in set must have same time (within leniency)
2. If multiple samples → need CustomIndex management
3. Volume balancing required before export

---

### 6. **CustomIndex Aggregate** 🟠 HITSOUND DOMAIN

**Aggregate Root**: [`CustomIndex`](../../Mapping_Tools/Classes/HitsoundStuff/CustomIndex.cs)

**Structure**:
- `int Index` - The custom sample index number (1-99)
- `Dictionary<SampleSet, Dictionary<Hitsound, SampleGeneratingArgs>>` - Sample definitions

**Invariants**:
1. Index must be unique within beatmap
2. Can represent up to 4 SampleSets × 4 Hitsounds = 16 samples
3. Must "fit" all required SamplePackages
4. Samples must be valid/loadable

**Modification Methods**:
- `Fits()` - Checks if can represent a package
- `MergeWith()` - Combines with another CustomIndex
- `CanMerge()` - Validates merge possibility
- `CleanInvalids()` - Removes unloadable samples

**Created By**: `HitsoundConverter.GetCustomIndices()`
**Optimized By**: `HitsoundConverter.OptimizeCustomIndices()`

**Owned By**: Hitsound Studio export process
**Lifetime**: Duration of hitsound export operation

---

### 7. **SnappingToolsProject Aggregate** 🟢 SNAPPING DOMAIN

**Aggregate Root**: [`SnappingToolsProject`](../../Mapping_Tools/Classes/Tools/SnappingTools/Serialization/SnappingToolsProject.cs)

**Owned Entities**:
- `SnappingToolsPreferences` - Current settings
- `List<SnappingToolsSaveSlot>` - Saved configurations
- `IEnumerable<RelevantObjectsGenerator>` - Active generators (40+ types)

**Invariants**:
1. CurrentPreferences must be valid
2. Generators must have settings applied from preferences
3. Save slots must have unique names/hotkeys
4. Preferences and generators stay synchronized

**Modification Methods**:
- `SetGenerators()` - Updates generator list and applies settings
- `SetCurrentPreferences()` - Updates prefs and applies to generators
- `SaveToSlot()` / `LoadFromSlot()` - Persists/restores state

**Source of Truth**: User's project file (JSON serialized)
**Owned By**: Snapping Tools UI

---

### 8. **RelevantObjectLayer Value Object** 🟢 SNAPPING DOMAIN

**Not an Aggregate Root** - Transient computation layer

**Structure**:
- `RelevantObjectCollection` - Generated virtual objects
- Reference to `PreviousLayer` - Layered generation
- Generator settings applied during creation

**Invariants**:
1. Objects generated from previous layer + generators
2. Inception level limits recursion depth
3. Must maintain relevancy scores
4. Time-based sorting for efficiency

**Lifetime**: Exists only during snapping overlay rendering
**Created By**: `LayerCollection.Update()`
**Consumed By**: Snapping overlay for visual display

---

### 9. **Pattern Aggregate** 🔵 PATTERN GALLERY DOMAIN

**Aggregate Root**: Pattern (file-based, not explicit class)

**Structure**:
- Subset of Beatmap (filtered HitObjects)
- Associated Timing points
- Pattern metadata (name, collection, parts)

**Invariants**:
1. Pattern is self-contained (all needed timing included)
2. Relative timing preserved
3. Can be placed at different times/BPMs
4. Parts define where pattern inserts

**Modification Methods**:
- `OsuPatternMaker.MakePattern()` - Extracts pattern from beatmap
- `OsuPatternPlacer.PlacePattern()` - Inserts pattern into beatmap

**Source of Truth**: Pattern files on disk (`.osu` format)
**Owned By**: Pattern Gallery user storage

---

### 10. **TumourLayer Configuration** 🟣 PATTERN GENERATION DOMAIN

**Aggregate Root**: [`TumourLayer`](../../Mapping_Tools/Classes/Tools/TumourGenerating/Options/TumourLayer.cs)

**Owned Entities**:
- `ITumourTemplate` - Shape template (Circle/Parabola/Square/Triangle)
- Property configurations (offset, rotation, length, width)
- Enabled/visible state

**Invariants**:
1. Must have valid template
2. Properties must be within valid ranges
3. Template-specific constraints apply

**Modification Methods**:
- Direct property setters
- Template can be swapped

**Owned By**: `TumourGeneratorVm`
**Source of Truth**: User's tumour configuration

---

### 11. **ComboColourProject Aggregate** 🟣 COMBO COLOUR DOMAIN

**Aggregate Root**: [`ComboColourProject`](../../Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs)

**Owned Entities**:
- `List<ColourPoint>` - Timeline of color changes
- `List<ComboColour>` - Available combo colors
- Mode settings (sequence/gradient/interpolation)

**Invariants**:
1. ColourPoints must be sorted by time
2. Each ColourPoint has valid colour sequence
3. ComboColours count <= 8 (osu! limit)
4. Color indices must reference valid combo colors

**Modification Methods**:
- `AddColourPointCommand` / `RemoveColourPointCommand`
- `AddComboCommand` / `RemoveComboCommand`
- `Export()` - Applies to beatmap

**Source of Truth**: User's combo colour project file
**Owned By**: Combo Colour Studio UI

---

## Data Ownership Matrix

| Aggregate | Owner Context | Write Access | Read Access | Persistence |
|-----------|---------------|--------------|-------------|-------------|
| **Beatmap** | Core Beatmap Context | BeatmapEditor, All Tools | All Tools, EditorReader | `.osu` file + Editor memory |
| **HitObject** | Beatmap Aggregate | Via Beatmap only | Via Beatmap only | Part of `.osu` file |
| **Timing** | Beatmap Aggregate | Via Beatmap only | Via Beatmap only | Part of `.osu` file |
| **HitsoundLayer** | Hitsound Context | HitsoundStudio | HitsoundStudio | Hitsound project JSON |
| **CustomIndex** | Hitsound Context | HitsoundConverter | HitsoundExporter | Temporary (export time) |
| **SnappingProject** | Snapping Context | SnappingTools UI | SnappingTools | Snapping project JSON |
| **RelevantObjectLayer** | Snapping Context | LayerCollection | Snapping Overlay | Transient (not persisted) |
| **Pattern** | Pattern Context | PatternGallery | PatternGallery | Pattern files (`.osu`) |
| **TumourLayer** | Tumour Context | TumourGenerator UI | TumourGenerator | Tool settings |
| **ComboColourProject** | Combo Context | ComboColourStudio | ComboColourStudio | Combo project JSON |

---

## Aggregate Relationships & Dependencies

### Cross-Aggregate References

**Beatmap → External**:
- No outbound aggregate references
- Self-contained with all gameplay data
- **Pure aggregate** - others reference it

**HitsoundLayer → Beatmap**:
- Imports from: Beatmap hit objects, MIDI, Storyboard
- Exports to: Beatmap (via HitsoundConverter)
- **Dependency**: Read Beatmap, Write back transformed

**Pattern → Beatmap**:
- Extracted from: Beatmap
- Inserted into: Beatmap
- **Dependency**: Read/Write Beatmap subset

**SnappingProject → Beatmap**:
- Reads: Current beatmap hit objects
- Generates: Virtual objects for placement
- Does NOT modify beatmap directly
- **Dependency**: Read-only Beatmap reference

**TumourGenerator → Beatmap**:
- Reads: Base slider path
- Writes: Modified slider path back
- **Dependency**: Read/Transform/Write Beatmap

**ComboColourProject → Beatmap**:
- Reads: Beatmap hit objects timeline
- Writes: Combo colors to beatmap
- **Dependency**: Read timeline, Write colors

---

## Consistency Boundaries

### Transactional Boundaries

**Beatmap Modifications**:
```
1. BackupManager.SaveMapBackup() ← Create backup
2. Load beatmap via BeatmapEditor
3. Modify hit objects / timing
4. Call consistency methods:
   - SortHitObjects()
   - CalculateSliderEndTimes()
   - GiveObjectsGreenlines()
   - CalculateHitObjectComboStuff()
5. BeatmapEditor.SaveFile() ← Atomic write
```

**Hitsound Export**:
```
1. Collect all HitsoundLayers
2. HitsoundConverter.ZipLayers() → SamplePackages
3. HitsoundConverter.GetCustomIndices() → CustomIndex list
4. HitsoundConverter.OptimizeCustomIndices() → Minimize indices
5. Export samples + Write to beatmap
   ← All or nothing (samples + beatmap update together)
```

**Pattern Placement**:
```
1. Load target beatmap
2. Remove conflicting objects in time range
3. Load pattern beatmap
4. Transform pattern timing/properties
5. Insert pattern objects
6. Rebuild timing/hitsounds
   ← Must complete all steps for consistency
```

---

## Invariant Enforcement

### Beatmap Aggregate Invariants

**Enforced By**:
- `Beatmap.SortHitObjects()` - Ensures time ordering
- `Beatmap.CalculateHitObjectComboStuff()` - Ensures combo consistency
- `Beatmap.GiveObjectsGreenlines()` - Ensures timing references
- `Timing.Sort()` - Ensures timing point ordering
- `Timing.Add()` / `Remove()` - Maintains 3-list synchronization

**Validation Points**:
- Before save via `BeatmapEditor`
- After any tool modification
- On load from disk/editor

### Hitsound Layer Invariants

**Enforced By**:
- `HitsoundLayer.RemoveDuplicates()` - Ensures no duplicate times
- `HitsoundLayer.Reload()` - Rebuilds from source
- Property setters with validation

**Validation Points**:
- Before export
- After import from source
- During layer manipulation

### Timing Aggregate Invariants

**Enforced By**:
- `Timing.Add()` - Binary search insert maintaining order
- `Timing.SetTimingPoints()` - Rebuilds all caches
- `Timing.Sort()` - Re-establishes order

**Critical**: Binary search depends on sorted state - corruption causes incorrect lookups

---

## Shared Kernel vs Context-Specific

### Shared Kernel (Used Across Contexts)

**Core Value Objects**:
- `Vector2` - 2D position (universal)
- `Time` (double) - Millisecond timestamps (universal)
- `SampleSet` enum - Normal/Soft/Drum (cross-context)
- `Hitsound` enum - N/W/F/C flags (cross-context)
- `GameMode` enum - Std/Taiko/Catch/Mania (cross-context)

**Core Entities** (Read-only in other contexts):
- `Beatmap` - Read by all tools
- `HitObject` - Read by all tools
- `TimingPoint` - Read by timing-aware tools

### Context-Specific (Not Shared)

**Hitsound Context Only**:
- `HitsoundLayer`, `SamplePackage`, `CustomIndex`
- `HitsoundConverter`, `SampleImporter`, `HitsoundExporter`

**Snapping Context Only**:
- `RelevantObject` hierarchy
- `RelevantObjectsGenerator` types
- `SnappingToolsProject`, `RelevantObjectLayer`

**Pattern Context Only**:
- Pattern file management
- `OsuPatternMaker`, `OsuPatternPlacer`
- Pattern transformation logic

**Tumour Context Only**:
- `TumourTemplate` types
- `TumourLayer` configuration
- Path generation algorithms

---

## Repository Pattern (Implicit)

### BeatmapEditor as Repository

**Responsibilities**:
- Load beatmap from file
- Save beatmap to file
- Merge with editor memory state
- Manage file paths

**Interface** (implicit):
```csharp
class BeatmapEditor {
    Beatmap Beatmap { get; }
    void SaveFile()
    void SaveFile(string path)
    static Load(string path) : BeatmapEditor
}
```

**Used By**: All 20+ tools as primary beatmap access

### ProjectManager as Repository

**Responsibilities**:
- Load/Save tool projects
- Serialize to JSON
- Manage project file paths
- Generic for ISavable<T>

**Interface**:
```csharp
static class ProjectManager {
    void SaveProject<T>(ISavable<T> view, ...)
    void LoadProject<T>(ISavable<T> view, ...)
    bool IsSaved<T>(ISavable<T> view)
}
```

**Used By**: Complex tools (Hitsound Studio, Snapping Tools, Combo Colour Studio)

---

## Data Flow Patterns

### Tool Execution Pattern

```
1. User selects beatmap paths
2. Tool loads via BeatmapEditor
3. Optional: Merge with EditorReader state
4. Tool performs transformations
5. Consistency methods called
6. BeatmapEditor.SaveFile()
7. Optional: Reload in osu! editor
```

### Read-Modify-Write with Backup

```
1. BackupManager.SaveMapBackup(path)
2. BeatmapEditor editor = EditorReaderStuff.GetNewestVersionOrNot(path)
3. Modify editor.Beatmap
4. editor.SaveFile()
```

### Hitsound Generation Flow

```
1. User creates HitsoundLayers (imports from various sources)
2. HitsoundConverter.ZipLayers() → SamplePackages
3. HitsoundConverter.GetCompleteHitsounds() → CustomIndices + Hitsounds
4. HitsoundExporter.ExportHitsounds() → Write to beatmap
5. HitsoundExporter.ExportCustomIndices() → Copy sample files
```

---

## Anti-Corruption Layers

### EditorReaderStuff (ACL)

**Purpose**: Protect domain from external osu! editor memory format

**Responsibilities**:
- Convert `Editor_Reader.HitObject` → `BeatmapHelper.HitObject`
- Convert `Editor_Reader.ControlPoint` → `TimingPoint`
- Validate and clean memory data
- Merge disk + memory versions

**Interface**:
```csharp
static class EditorReaderStuff {
    BeatmapEditor GetNewestVersionOrNot(string path, ...)
    List<HitObject> UpdateBeatmap(Beatmap, EditorReader)
    HitObject ConvertHitObject(Editor_Reader.HitObject)
}
```

### HitsoundConverter (ACL)

**Purpose**: Convert between HitsoundLayer domain and Beatmap domain

**Responsibilities**:
- Zip layers → packages
- Optimize custom indices
- Balance volumes
- Generate hit objects with proper hitsounds

**Interface**:
```csharp
class HitsoundConverter {
    static List<SamplePackage> ZipLayers(layers, ...)
    static CompleteHitsounds GetCompleteHitsounds(packages, ...)
    static List<CustomIndex> OptimizeCustomIndices(indices)
}
```

---

## Aggregate Design Recommendations

### 1. Beatmap Aggregate (Current: Good)

**Strengths**:
✅ Clear aggregate root
✅ Strong invariants enforced
✅ Consistency methods well-defined
✅ Single source of truth (with merge strategy)

**Improvements**:
- Consider splitting Storyboard into separate aggregate (different lifecycle)
- Add domain events for modifications (for undo/redo)
- Encapsulate consistency methods (currently public, should be automatic)

### 2. Hitsound Aggregates (Current: Mixed)

**Strengths**:
✅ HitsoundLayer is good aggregate
✅ CustomIndex has clear boundaries

**Issues**:
⚠️ SamplePackage should be value object (currently mutable)
⚠️ Tight coupling between layers → converter → exporter

**Improvements**:
- Make SamplePackage immutable
- Add HitsoundProject aggregate containing all layers
- Consider domain events for layer changes

### 3. Snapping Aggregates (Current: Good for transient data)

**Strengths**:
✅ SnappingToolsProject is proper aggregate
✅ RelevantObjectLayer correctly transient
✅ Clear generation pipeline

**No major issues** - Design fits use case (transient computational geometry)

### 4. Tool-Specific Aggregates (Current: Adequate)

**Pattern/Tumour/ComboColour**: Each has clear boundaries and ownership

**Improvement Opportunity**:
- Standardize project serialization
- Add common base for tool projects
- Consider tool workflow domain events

---

## Critical Findings for DDD Migration

### 1. Strong Aggregate: Beatmap

**Status**: ✅ Already well-designed aggregate
- Clear root (Beatmap)
- Strong invariants
- Consistency boundary enforcement
- Single source of truth with merge strategy

**Migration**: Minimal changes needed, mostly formalization

### 2. Hitsound Domain Needs Clarity

**Status**: ⚠️ Multiple aggregates with unclear boundaries
- HitsoundLayer → Should be aggregate root
- CustomIndex → Should be aggregate root  
- SamplePackage → Should be value object
- Need HitsoundProject aggregate

**Migration**: Requires refactoring and boundary clarification

### 3. Cross-Aggregate Transactions

**Status**: ⚠️ Tools perform multi-aggregate operations
- Pattern placement modifies beatmap in transaction
- Hitsound export writes samples + beatmap together
- No explicit transaction management

**Migration**: Need Unit of Work pattern or domain events

### 4. Implicit Repositories

**Status**: ⚠️ BeatmapEditor and ProjectManager act as repositories
- Not explicitly designed as repositories
- Mixed concerns (persistence + domain logic)

**Migration**: Extract proper repository interfaces

---

**Document Version**: 1.0  
**Analysis Date**: 2025-11-17  
**Scope**: All identified aggregates and their ownership patterns
