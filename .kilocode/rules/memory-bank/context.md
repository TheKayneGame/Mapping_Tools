# Mapping Tools - Current Context

## Project Status

**Current Version**: 1.12.27
**Status**: Active maintenance and development
**Primary Platform**: Windows Desktop (.NET 5.0)
**License**: MIT (Open Source)

## Recent State

The project is a mature, actively maintained desktop application with:
- 20+ specialized tools for osu! beatmap creation
- Stable architecture based on WPF and MVVM pattern
- Well-established user base in osu! mapping community
- Regular updates and bug fixes

## Active Components

### Core Functionality
- All 20+ tools are fully functional and production-ready
- Main tools include: Map Cleaner, Hitsound Studio, Geometry Dashboard, Slider Tools Suite
- Automatic backup system operational
- Editor integration via EditorReader working

### Key Features
- Real-time osu! editor memory reading
- Direct .osu file manipulation
- Graph-based value editors
- Timeline visualizations
- Project save/load system
- Automatic update system via Onova

## Technology Stack Summary

**Framework**: .NET 5.0 with WPF
**Language**: C# 9.0
**UI**: MaterialDesignThemes + Extended WPF Toolkit
**Audio**: NAudio + Vorbis support
**Integration**: OsuMemoryDataProvider + custom EditorReader
**Updates**: Onova with GitHub releases

## Code Organization

### Main Projects
- **Mapping_Tools** - Main application (WPF desktop app)
- **Mapping_Tools_Tests** - xUnit test suite
- **lib/** - External dependencies (EditorReader.dll, NonInvasiveKeyboardHookLibrary.Core.dll)

### Critical Paths
- [`Mapping_Tools/App.xaml.cs`](Mapping_Tools/App.xaml.cs) - Entry point
- [`Mapping_Tools/MainWindow.xaml.cs`](Mapping_Tools/MainWindow.xaml.cs) - Main UI
- [`Mapping_Tools/Classes/BeatmapHelper/`](Mapping_Tools/Classes/BeatmapHelper/) - Core beatmap logic
- [`Mapping_Tools/Views/`](Mapping_Tools/Views/) - All tool implementations
- [`Mapping_Tools/Viewmodels/`](Mapping_Tools/Viewmodels/) - MVVM ViewModels

## Architecture Patterns

**Primary Patterns in Use**:
1. **MVVM** - Separation of UI and logic
2. **Plugin Architecture** - Each tool is self-contained
3. **Repository Pattern** - BeatmapEditor for file access
4. **Observer Pattern** - INotifyPropertyChanged for UI updates
5. **Command Pattern** - User actions via ICommand
6. **Template Method** - Tool execution workflow

## Current Work Areas

### Stable Features
- Beatmap parsing and serialization (fully working)
- Timing calculations with binary search optimization
- File I/O with automatic backups
- Editor memory reading with validation
- All 20+ tools operational

### Key Classes
- **Timing** - Advanced timing point management with binary search
- **Beatmap** - Complete .osu file representation
- **BeatmapEditor** - High-level file operations
- **EditorReaderStuff** - osu! editor integration wrapper
- **BackupManager** - Automatic backup system
- **ProjectManager** - Tool state persistence

## Development Workflow

### Tool Development Pattern
1. Create View (XAML UserControl)
2. Create ViewModel (inherits BindableBase)
3. Implement tool logic in BackgroundWorker_DoWork
4. Add to navigation via reflection
5. Optional: Implement ISavable<T> for project persistence

### File Operations Pattern
1. Get current beatmap paths
2. Create backup via BackupManager
3. Load beatmap with EditorReaderStuff (merges memory + disk)
4. Process beatmap
5. Save via BeatmapEditor
6. Display results

## Known Patterns

### Binary Search Usage
- Extensively used in [`Timing.cs`](Mapping_Tools/Classes/BeatmapHelper/Timing.cs) for efficient timing point lookups
- Separate sorted lists for redlines and greenlines
- BinarySearchUtil helper for consistent implementation

### Thread Safety
- EditorReaderLock for synchronizing editor access
- BackgroundWorker for long-running operations
- Dispatcher for UI thread marshaling

### Error Handling
- Custom exceptions: BeatmapIncompatibleException, EditorReaderDisabledException
- Validation before editor reading
- Graceful degradation to disk-only reading
- Crash log generation

## Integration Points

### osu! Client Integration
- **Non-invasive**: Reads memory without modifying client
- **EditorReader.dll**: External library for memory access
- **OsuMemoryDataProvider**: Real-time state monitoring
- **Registry detection**: Auto-finds osu! installation

### File System Integration
- **Direct .osu manipulation**: Custom parser/serializer
- **Automatic backups**: Timestamped with cleanup
- **Dual backup**: Disk version + memory version
- **Temp files**: For MD5 validation

## Next Steps for Contributors

When working on this project:

1. **Adding a new tool**: Inherit from MappingTool or SingleRunMappingTool, implement BackgroundWorker_DoWork
2. **Modifying beatmap logic**: Work in Classes/BeatmapHelper/, use BeatmapEditor for file ops
3. **UI changes**: Follow Material Design patterns, use data binding
4. **Testing**: Add xUnit tests in Mapping_Tools_Tests project
5. **Editor integration**: Use EditorReaderStuff wrapper, handle exceptions gracefully

## Important Notes

- Always create backups before modifying beatmaps
- Use EditorReaderStuff.GetNewestVersionOrNot() for safe editor integration
- Binary search is critical for performance in Timing class
- Thread safety required for EditorReader operations
- Custom DLLs in lib/ must be distributed with application