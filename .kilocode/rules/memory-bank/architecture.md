# Mapping Tools - Architecture Documentation

## System Architecture

Mapping Tools follows a **layered desktop application architecture** with clear separation between UI, business logic, and data access layers.

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    Presentation Layer                    │
│  (WPF Views + ViewModels + Material Design Components)  │
└───────────────────┬─────────────────────────────────────┘
                    │
┌───────────────────▼─────────────────────────────────────┐
│                   Application Layer                      │
│      (Tool Implementations + Workflow Orchestration)     │
└───────────────────┬─────────────────────────────────────┘
                    │
┌───────────────────▼─────────────────────────────────────┐
│                    Business Logic Layer                  │
│  (Beatmap Helpers + Algorithm Implementations + Math)   │
└───────────────────┬─────────────────────────────────────┘
                    │
┌───────────────────▼─────────────────────────────────────┐
│                   Data Access Layer                      │
│    (File I/O + Editor Reader + Serialization)           │
└─────────────────────────────────────────────────────────┘
```

## Source Code Structure

### Primary Directories

**`Mapping_Tools/`** - Main application project
- `Views/` - WPF UserControl implementations for each tool
- `Viewmodels/` - MVVM ViewModels with data binding logic
- `Classes/` - Business logic and data models
  - `BeatmapHelper/` - Core beatmap parsing and manipulation
  - `Tools/` - Tool-specific algorithms and logic
  - `ToolHelpers/` - Shared helper utilities
  - `SystemTools/` - Application infrastructure
  - `HitsoundStuff/` - Audio processing and hitsound management
  - `MathUtil/` - Mathematical utilities and algorithms
- `Components/` - Reusable UI components
  - `Graph/` - Graph-based value editors
  - `TimeLine/` - Timeline visualization
  - `Dialogs/` - Custom dialog windows

**`Mapping_Tools_Tests/`** - xUnit test project

### Key File Paths

**Application Entry Point**
- [`App.xaml.cs`](Mapping_Tools/App.xaml.cs) - Application initialization and global exception handling
- [`MainWindow.xaml.cs`](Mapping_Tools/MainWindow.xaml.cs) - Main window, tool navigation, beatmap selection

**Core Data Models**
- [`Classes/BeatmapHelper/Beatmap.cs`](Mapping_Tools/Classes/BeatmapHelper/Beatmap.cs) - Main beatmap representation
- [`Classes/BeatmapHelper/HitObject.cs`](Mapping_Tools/Classes/BeatmapHelper/HitObject.cs) - Individual hit objects
- [`Classes/BeatmapHelper/Timing.cs`](Mapping_Tools/Classes/BeatmapHelper/Timing.cs) - Timing point management
- [`Classes/BeatmapHelper/TimingPoint.cs`](Mapping_Tools/Classes/BeatmapHelper/TimingPoint.cs) - Individual timing points

**Core Infrastructure**
- [`Classes/SystemTools/SettingsManager.cs`](Mapping_Tools/Classes/SystemTools/SettingsManager.cs) - Application configuration
- [`Classes/SystemTools/ProjectManager.cs`](Mapping_Tools/Classes/SystemTools/ProjectManager.cs) - Project save/load
- [`Classes/SystemTools/BackupManager.cs`](Mapping_Tools/Classes/SystemTools/BackupManager.cs) - Automatic backups
- [`Classes/ToolHelpers/EditorReaderStuff.cs`](Mapping_Tools/Classes/ToolHelpers/EditorReaderStuff.cs) - osu! editor integration

**Tool Base Classes**
- [`Views/MappingTool.cs`](Mapping_Tools/Views/MappingTool.cs) - Base class for all tools
- [`Views/SingleRunMappingTool.cs`](Mapping_Tools/Views/SingleRunMappingTool.cs) - Tools with background worker execution

## Key Technical Decisions

### 1. MVVM Pattern with Data Binding

**Decision**: Use Model-View-ViewModel architecture with WPF data binding

**Rationale**:
- Separates UI from business logic
- Enables testability
- Leverages WPF's powerful binding system
- Reduces boilerplate code

**Implementation**:
- ViewModels inherit from `BindableBase` for INotifyPropertyChanged
- Views are UserControls with XAML markup
- Two-way binding for user inputs
- Command pattern for user actions

### 2. Plugin-Like Tool Architecture

**Decision**: Each tool is a self-contained view with its own ViewModel

**Rationale**:
- Enables independent tool development
- Easy to add new tools
- Clean separation of concerns
- Allows per-tool configuration persistence

**Implementation**:
- All tools inherit from [`MappingTool`](Mapping_Tools/Views/MappingTool.cs) base class
- Navigation system dynamically discovers tools via reflection
- Each tool has:
  - Dedicated View (XAML + code-behind)
  - ViewModel for state management
  - Optional project file format for persistence

### 3. Direct File Manipulation

**Decision**: Parse and modify .osu files directly rather than using an API

**Rationale**:
- osu! doesn't provide official API for beatmap editing
- Complete control over file format
- No dependency on external services
- Works offline

**Implementation**:
- Custom parsers for .osu file format
- Line-by-line reading with section detection
- Structured object model for manipulation
- Serialization back to .osu format

### 4. Non-Invasive Editor Reading

**Decision**: Read osu! editor memory without modifying the client

**Rationale**:
- Avoids modifying osu! executable
- No risk of breaking game functionality
- Real-time state access
- Safe for online use

**Implementation**:
- [`EditorReader.dll`](lib/EditorReader.dll) - External library for memory reading
- Process.NET for memory access
- Validation to prevent incorrect reads
- Graceful fallback when editor not available

### 5. Automatic Backup System

**Decision**: Create backups before every destructive operation

**Rationale**:
- Protects user work
- Enables easy undo
- Builds user confidence
- Minimal performance impact

**Implementation**:
- [`BackupManager`](Mapping_Tools/Classes/SystemTools/BackupManager.cs) handles all backup operations
- Timestamped backup files
- Automatic cleanup of old backups
- Optional dual-backup (disk + memory versions)

## Design Patterns

### 1. Repository Pattern

**Usage**: Beatmap file access

**Implementation**:
- `BeatmapEditor` provides CRUD operations for beatmaps
- `StoryboardEditor` for storyboard files
- Abstraction over file system details

### 2. Strategy Pattern

**Usage**: Different hitsound export formats

**Implementation**:
- `HitsoundExporter` with multiple export strategies
- `SampleExportFormat` enum defines available strategies
- Single interface, multiple implementations

### 3. Observer Pattern

**Usage**: UI updates and event handling

**Implementation**:
- INotifyPropertyChanged for data binding
- Custom events for cross-component communication
- `ListenerManager` for coordinating updates

### 4. Command Pattern

**Usage**: User actions in ViewModels

**Implementation**:
- `CommandImplementation` class wraps actions
- Bound to UI elements via ICommand interface
- Enables undo/redo potential

### 5. Factory Pattern

**Usage**: Creating timing points and hit objects

**Implementation**:
- Static factory methods on model classes
- Handles complex initialization logic
- Type-safe object creation

### 6. Template Method Pattern

**Usage**: Tool execution workflow

**Implementation**:
- `MappingTool` base class defines workflow
- `BackgroundWorker_DoWork` abstract method for tool logic
- Consistent execution pattern across tools

## Component Relationships

### Core Components

**MainWindow** → **ViewCollection** → **Individual Tool Views**
- MainWindow manages navigation
- ViewCollection maintains tool instances
- Lazy loading of tool views

**Tool View** → **ViewModel** → **Business Logic**
- View handles UI rendering
- ViewModel manages state
- Business logic performs operations

**EditorReaderStuff** → **EditorReader** → **osu! Process**
- Wrapper around external library
- Thread-safe access
- Validation and error handling

**BeatmapEditor** → **Beatmap** → **File System**
- Editor provides high-level operations
- Beatmap represents in-memory structure
- Automatic serialization

### Data Flow

1. **User Input** → View captures interaction
2. **Data Binding** → ViewModel property updated
3. **Validation** → Input validation in ViewModel
4. **Command Execution** → Business logic invoked
5. **File Operations** → BeatmapEditor reads/writes files
6. **Backup Creation** → BackupManager saves copy
7. **Result Display** → UI updates via data binding

## Critical Implementation Paths

### Beatmap Loading with Editor Integration

```
User selects beatmap
    ↓
EditorReaderStuff.GetNewestVersionOrNot()
    ↓
Check if beatmap open in editor
    ↓
If open: Read from editor memory + merge with disk version
If closed: Read from disk only
    ↓
BeatmapEditor wraps Beatmap object
    ↓
Return to tool for processing
```

### Tool Execution Workflow

```
User clicks "Run" button
    ↓
Validation in ViewModel
    ↓
BackupManager.SaveMapBackup() 
    ↓
BackgroundWorker.RunWorkerAsync()
    ↓
Tool-specific logic in BackgroundWorker_DoWork()
    ↓
Progress updates via UpdateProgressBar()
    ↓
BeatmapEditor.SaveFile()
    ↓
Result message displayed
    ↓
Optional: Open output folder
```

### Timing Calculation System

```
Request timing at specific time
    ↓
Timing.GetRedlineAtTime() - Binary search redlines
    ↓
Timing.GetGreenlineAtTime() - Binary search greenlines
    ↓
Calculate effective values (BPM, SV multiplier)
    ↓
Use for slider duration calculations
    ↓
Return calculated values
```

### Hitsound Layer Processing

```
Import layers from various sources (MIDI, beatmap, storyboard)
    ↓
HitsoundConverter.ZipLayers() - Merge layers by time
    ↓
HitsoundConverter.BalanceVolumes() - Normalize volumes
    ↓
SampleImporter.ImportSamples() - Load audio files
    ↓
HitsoundConverter.GetCompleteHitsounds() - Generate custom indices
    ↓
HitsoundExporter.ExportHitsounds() - Create beatmap
    ↓
HitsoundExporter.ExportCustomIndices() - Export samples
```

## Threading Model

### UI Thread
- All WPF UI operations
- Data binding updates
- User interaction handling

### Background Worker Threads
- Tool execution via `BackgroundWorker`
- Long-running operations
- Progress reporting via `ReportProgress()`
- File I/O operations

### Thread Safety
- `EditorReaderLock` for editor access synchronization
- Dispatcher for UI updates from background threads
- Immutable data structures where possible

## State Management

### Application-Level State
- `SettingsManager.Settings` - Global configuration
- `MainWindow.AppWindow` - Singleton main window reference
- Current beatmap selection in MainWindow

### Tool-Level State
- ViewModel properties (persisted via ProjectManager)
- ISavable<T> interface for project save/load
- AutoSave for preserving state between sessions

### Transient State
- BackgroundWorker progress
- Temporary file paths
- UI interaction state

## Error Handling Strategy

1. **Validation**: Prevent errors at input time
2. **Try-Catch**: Wrap risky operations
3. **Custom Exceptions**: Domain-specific error types
   - `BeatmapIncompatibleException`
   - `EditorReaderDisabledException`
   - `InvalidEditorReaderStateException`
4. **User Feedback**: Show() extension method for exceptions
5. **Logging**: Write to crash-log.txt for unhandled exceptions
6. **Graceful Degradation**: Fall back to disk reading if editor reading fails