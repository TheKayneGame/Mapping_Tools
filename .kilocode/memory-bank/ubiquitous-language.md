# Mapping Tools - Ubiquitous Language

This document identifies business terms used across the Mapping Tools codebase, highlighting semantic conflicts (polysemes) and context-specific meanings.

## Core Domain Terms

### Beatmap Domain

#### Beatmap
**Primary Context**: Core beatmap file representation
- **In [`Beatmap.cs`](../../Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs)**: Complete .osu file data structure containing metadata, timing, hit objects, and storyboard
- **Semantic Unity**: Single, consistent meaning across all contexts
- **Aggregate Root**: Yes - owns HitObjects, TimingPoints, Storyboard

#### HitObject
**Primary Context**: Gameplay objects
- **In [`HitObject.cs`](../../Mapping_Tools/Classes/BeatmapHelper/HitObject.cs)**: Individual gameplay element (Circle, Slider, Spinner, HoldNote)
- **Related Terms**: 
  - "Object" (shortened form in code)
  - "Selected objects" (in editor context)
  - "Timeline objects" (visual representation)
- **Semantic Unity**: Consistent meaning - always refers to gameplay objects

#### TimingPoint
**Primary Context**: Timing system
- **In [`TimingPoint.cs`](../../Mapping_Tools/Classes/BeatmapHelper/TimingPoint.cs)**: Control point defining BPM, meter, samples, or slider velocity
- **Polyseme Alert**: 
  - "Uninherited timing point" = Redline = BPM-defining point
  - "Inherited timing point" = Greenline = SV/volume/sample modifier
- **Semantic Conflict**: "Timing point" can mean either redline OR greenline depending on context

#### Timing
**Primary Context**: Timing calculation system
- **In [`Timing.cs`](../../Mapping_Tools/Classes/BeatmapHelper/Timing.cs)**: Complete timing system managing all timing points with binary search optimization
- **Distinct from**: Individual "TimingPoint" - this is the collection manager
- **Semantic Unity**: Clear distinction from TimingPoint

### Hitsound Domain

#### Sample
**Primary Context**: Audio sample representation
- **In [`Sample.cs`](../../Mapping_Tools/Classes/HitsoundStuff/Sample.cs)**: Hitsound sample with generating args, priority, volume
- **Polyseme Alert**: Multiple meanings exist
  1. Audio sample file (physical .wav file)
  2. Sample object (in-memory representation)
  3. SampleSet enum (Normal/Soft/Drum)
- **Semantic Conflict**: HIGH - "Sample" used for 3 different concepts

#### Layer
**Primary Context**: Hitsound composition
- **In [`HitsoundLayer.cs`](../../Mapping_Tools/Classes/HitsoundStuff/HitsoundLayer.cs)**: Single hitsound and all times it plays
- **Polyseme Alert**:
  1. Hitsound layer (audio composition)
  2. Storyboard layer (visual layer: Background/Fail/Pass/Foreground/Overlay)
  3. Tumour layer (pattern generation layer)
  4. Relevant object layer (snapping tool layer)
- **Semantic Conflict**: CRITICAL - "Layer" means completely different things in 4 contexts

#### Package
**Primary Context**: Hitsound bundling
- **In [`SamplePackage.cs`](../../Mapping_Tools/Classes/HitsoundStuff/SamplePackage.cs)**: Collection of samples that play at same time
- **Semantic Unity**: Consistent within hitsound domain
- **No Conflict**: Term not overloaded

### Editor Integration Domain

#### Editor
**Primary Context**: File I/O wrapper
- **In [`Editor.cs`](../../Mapping_Tools/Classes/BeatmapHelper/Editor.cs)**: Base class for file operations (save/load)
- **Polyseme Alert**:
  1. File editor class (internal system)
  2. osu! game editor (external system being read)
  3. BeatmapEditor/StoryboardEditor (specialized editors)
- **Semantic Conflict**: MEDIUM - "Editor" refers to both internal tool and external osu! editor

#### Reader
**Primary Context**: Memory reading from osu! client
- **In [`EditorReaderStuff.cs`](../../Mapping_Tools/Classes/ToolHelpers/EditorReaderStuff.cs)**: Wrapper around EditorReader.dll for reading osu! editor memory
- **Semantic Unity**: Consistent - always means reading from external process
- **No Major Conflict**: Clear context separation

### Slider Domain

#### Slider
**Primary Context**: Slider-type hit object
- **In HitObject.cs**: When [`IsSlider`](../../Mapping_Tools/Classes/BeatmapHelper/HitObject.cs:117) is true
- **Components**:
  - SliderPath: Geometric path representation
  - SliderType: PathType (Linear, Bezier, PerfectCurve, Catmull, BSpline)
  - PixelLength: Spatial length
  - TemporalLength: Duration
  - Repeat: Number of slide repeats
- **Semantic Unity**: Consistent meaning
- **Related Terms**: "SliderVelocity" (SV), "SliderMultiplier" (global SV)

#### Path
**Primary Context**: Slider geometry
- **In [`SliderPath.cs`](../../Mapping_Tools/Classes/BeatmapHelper/SliderPathStuff/SliderPath.cs)**: Geometric curve definition
- **Polyseme Alert**:
  1. Slider path (geometry)
  2. File path (directory path)
- **Semantic Conflict**: LOW - Usually clear from context, but could be confusing

### Snapping Tools Domain

#### Generator
**Primary Context**: Virtual object generation
- **In Snapping Tools**: [`RelevantObjectsGenerator`](../../Mapping_Tools/Classes/Tools/SnappingTools/DataStructure/RelevantObjectGenerators/RelevantObjectsGenerator.cs) - 40+ generator types
- **Polyseme Alert**:
  1. Snapping generator (geometric object generator)
  2. Sample generator (audio generator)
  3. Tumour generator (pattern generator)
  4. Rhythm guide generator
- **Semantic Conflict**: MEDIUM - "Generator" means different things in different tool contexts

#### RelevantObject
**Primary Context**: Snapping assistance
- **In Snapping Tools**: Virtual objects for geometric snapping (points, lines, circles)
- **Semantic Unity**: Specific to snapping tools domain
- **No Conflict**: Domain-specific term

### System/Infrastructure Domain

#### Manager
**Primary Context**: System-level coordination
- **Instances**:
  1. [`SettingsManager`](../../Mapping_Tools/Classes/SystemTools/SettingsManager.cs): Application configuration
  2. [`BackupManager`](../../Mapping_Tools/Classes/SystemTools/BackupManager.cs): Automatic backups
  3. [`ProjectManager`](../../Mapping_Tools/Classes/SystemTools/ProjectManager.cs): Tool state persistence
  4. [`ListenerManager`](../../Mapping_Tools/Classes/SystemTools/ListenerManager.cs): Editor monitoring
  5. `MetadataManager`: Metadata editing tool
- **Semantic Conflict**: MEDIUM - Both system services AND user-facing tools use "Manager"

#### Project
**Primary Context**: Tool configuration persistence
- **In [`ProjectManager.cs`](../../Mapping_Tools/Classes/SystemTools/ProjectManager.cs)**: Saved state for complex tools (Hitsound Studio, Combo Colour Studio, Snapping Tools)
- **Semantic Unity**: Consistent - always means saved tool configuration
- **No Conflict**: Clear meaning

### Time-Related Terms

#### Time
**Primary Context**: Millisecond timestamp
- **Universal**: Used everywhere in beatmap as double (milliseconds)
- **Related Terms**:
  - Offset: Timing point time
  - StartTime: Hit object start
  - EndTime: Hit object/spinner/holdnote end
  - TemporalLength: Duration
  - BeatTime: Time in beats (not milliseconds)
- **Semantic Conflict**: LOW - "Time" vs "BeatTime" distinction is critical but usually clear

#### Beat
**Primary Context**: Musical beat unit
- **Meanings**:
  1. Musical beat (quarter note)
  2. Beat divisor (1/4, 1/8, etc.)
  3. Milliseconds per beat (MpB)
  4. BPM (beats per minute)
- **Semantic Conflict**: LOW - Usually clear from context

### Tool-Specific Terms

#### Transform
**Primary Context**: Geometric transformation
- **In Property Transformer**: Modify object properties with interpolation
- **In Snapping Tools**: SameTransformGenerator applies transformations
- **Semantic Unity**: Mathematical transformation concept
- **No Major Conflict**: Consistent geometric meaning

#### Pattern
**Primary Context**: Reusable mapping patterns
- **In Pattern Gallery**: Saved hit object arrangements
- **In Tumour Generator**: Algorithmically generated patterns
- **Semantic Unity**: Arrangement of hit objects
- **Minor Conflict**: Manual vs algorithmic source

## Critical Polysemes Requiring Attention

### 🔴 HIGH PRIORITY: Layer
**Problem**: "Layer" has 4 completely different meanings
1. **HitsoundLayer**: Audio composition layer
2. **StoryboardLayer**: Visual layer (5 types: Background, Fail, Pass, Foreground, Overlay)
3. **TumourLayer**: Pattern generation configuration
4. **RelevantObjectLayer**: Snapping tool virtual objects

**Impact**: Code confusion, difficult to discuss cross-context
**Recommendation**: Rename to:
- `HitsoundTrack` or `AudioLayer`
- `StoryboardLayer` (keep)
- `TumourGenerationConfig` or `PatternLayer`
- `SnappingLayer` or `VirtualObjectLayer`

### 🟠 MEDIUM PRIORITY: Sample
**Problem**: "Sample" has 3 different meanings
1. **Sample object**: In-memory hitsound representation
2. **Sample file**: Physical audio file (.wav)
3. **SampleSet**: Enum (Normal, Soft, Drum)

**Impact**: Confusion in hitsound domain
**Recommendation**: Use:
- `HitsoundSample` for object
- `SampleFile` or `AudioFile` for files
- `SampleSet` (keep enum name)

### 🟠 MEDIUM PRIORITY: Editor
**Problem**: Refers to both internal system and external osu! editor
1. **Editor class**: Internal file I/O wrapper
2. **osu! editor**: External application being integrated with

**Impact**: Unclear which "editor" is being referenced
**Recommendation**: 
- Rename internal to `FileEditor` or `BeatmapFileHandler`
- Keep "osu! editor" or "game editor" for external

### 🟠 MEDIUM PRIORITY: Generator
**Problem**: Multiple generator types across different domains
1. **RelevantObjectsGenerator**: Geometric snapping generators (40+ types)
2. **SampleSoundGenerator**: Audio sample generation
3. **TumourGenerator**: Pattern generation
4. **RhythmGuide generator**: Rhythm visualization

**Impact**: Generic term used across unrelated contexts
**Recommendation**: More specific names:
- `GeometricGenerator` or `SnappingGenerator`
- `AudioGenerator` or `SoundSynthesizer`
- `PatternGenerator` (already specific)
- `RhythmAnalyzer`

### 🟡 LOW PRIORITY: TimingPoint Types
**Problem**: "TimingPoint" can be redline OR greenline
1. **Uninherited/Redline**: BPM-defining timing point
2. **Inherited/Greenline**: SV/volume modifier

**Impact**: Must check Uninherited property to know type
**Recommendation**: Consider:
- `BpmTimingPoint` and `ModifierTimingPoint`
- OR keep current with better documentation

## Terms with Consistent Meaning (No Conflicts)

✅ **Beatmap**: Always .osu file representation
✅ **HitObject**: Always gameplay object
✅ **Timing**: Always complete timing system
✅ **Bookmark**: Always timeline marker
✅ **Combo**: Always combo color grouping
✅ **Hitsound**: Always audio feedback (Normal/Whistle/Finish/Clap)
✅ **Storyboard**: Always visual effects/background
✅ **Metadata**: Always song/map information
✅ **Difficulty**: Always difficulty settings (HP/CS/OD/AR)
✅ **Backup**: Always saved copy of beatmap
✅ **Project**: Always saved tool configuration
✅ **Slider**: Always slider-type hit object
✅ **Spinner**: Always spinner-type hit object
✅ **Circle**: Always circle-type hit object

## Context-Specific Vocabulary

### Hitsound Studio Context
- Layer, Sample, Package, Zone, Schema, CustomIndex, ImportArgs, SampleGeneratingArgs
- Exporter, Importer, Converter

### Snapping Tools Context
- Generator, RelevantObject, VirtualObject, Layer, CoordinateConverter
- Intersection, Parallel, Perpendicular, Bisector, Symmetry, Tangent

### Slider Tools Context
- Sliderator, Completionator, Merger, Picturator
- PathGenerator, BezierConverter, Anchor, CurvePoint

### Timing Tools Context
- Redline, Greenline, MpB (milliseconds per beat), Meter, BPM, Resnap

### Pattern/Tumour Context
- Template, Layer, Property, Relative scaling, Absolute positioning

## Recommendations for DDD Migration

### 1. Bounded Context Separation
The polysemes indicate clear bounded context boundaries:
- **Hitsound Context**: Own definition of Layer, Sample, Package
- **Snapping Context**: Own definition of Layer, Generator, RelevantObject
- **Core Beatmap Context**: Own definitions of Beatmap, HitObject, Timing
- **Storyboard Context**: Own definition of Layer, Event

### 2. Anti-Corruption Layers
Need translation between contexts for shared terms:
- "Layer" must be translated when crossing contexts
- "Sample" needs clarification at boundaries
- "Editor" should specify internal vs external

### 3. Ubiquitous Language Per Context
Each bounded context should have its own ubiquitous language dictionary where polysemes have single, clear meanings within that context.

### 4. Shared Kernel
Some terms are truly shared and should remain consistent:
- Beatmap, HitObject, TimingPoint (in Core Context)
- Time-related terms (Time, Offset, Duration)
- Geometric primitives (Vector2, Position, Angle)

## Cross-Context Communication Patterns

### Current Implicit Translations
1. **Tool → Core Beatmap**: Tools read/modify beatmap via `BeatmapEditor`
2. **Core → Editor Integration**: `EditorReaderStuff` wraps external editor access
3. **Hitsound → Core**: Hitsounds converted to hit objects for export
4. **Snapping → Core**: Virtual objects guide placement of real hit objects

### Recommended Explicit Translations
Need clear adapters/translators at context boundaries to handle polyseme translation and prevent semantic leakage.

---

**Document Version**: 1.0
**Analysis Date**: 2025-11-17
**Scope**: Complete codebase analysis covering 20+ tools and all major subsystems