# Mapping Tools - Technology Stack

## Framework & Language

### Core Platform
- **.NET 5.0** (Windows Desktop Runtime)
- **C# 9.0** with modern language features
- **WPF** (Windows Presentation Foundation) for UI

### Build Configuration
- **Visual Studio 2022** solution
- **Multi-platform builds**: x86 and x64
- **Inno Setup** for installer creation
- **Assembly Version**: 1.12.27

## Key Dependencies

### UI & Design
- **MaterialDesignThemes** (4.7.0-ci390)
  - Material Design UI components
  - Icons and theming
  - Modern, consistent look
- **MaterialDesignColors** (2.1.0-ci390)
  - Color palette for Material Design
- **Extended.Wpf.Toolkit** (4.4.0)
  - Enhanced WPF controls
  - Advanced UI components
- **VirtualizingWrapPanel** (1.5.7)
  - Performance optimization for large lists

### Data & Serialization
- **Newtonsoft.Json** (13.0.2)
  - Beatmap serialization
  - Configuration management
  - Project file format
  - Custom converters for Vector2 and other types

### Audio Processing
- **NAudio** (2.0.0)
  - Audio playback and processing
  - Sample manipulation
- **NAudio.Vorbis** (1.3.1)
  - Vorbis audio codec support
- **NVorbis** (0.10.3)
  - Vorbis decoding
- **OggVorbisEncoder** (1.2.0)
  - Encoding audio to Ogg Vorbis

### osu! Integration
- **OsuMemoryDataProvider** (0.10.3)
  - Real-time memory reading from osu! client
  - Editor state monitoring
- **EditorReader.dll** (Custom, in lib/)
  - Core editor memory reading functionality
  - Hit object and timing point extraction
- **Process.NET** (transitive dependency)
  - Memory access and manipulation

### System Integration
- **Overlay.NET** (1.0.2)
  - In-game overlay rendering
  - Used by Geometry Dashboard for snapping visualization
- **NonInvasiveKeyboardHookLibrary.Core.dll** (Custom, in lib/)
  - Global hotkey support
  - Non-invasive keyboard monitoring

### Updates & Distribution
- **Onova** (2.6.2)
  - Automatic update system
  - GitHub release integration
  - Background update downloads

### File System
- **Microsoft-WindowsAPICodePack-Core** (1.1.4)
- **Microsoft-WindowsAPICodePack-Shell** (1.1.4)
  - Advanced file dialogs
  - Windows shell integration

## Development Setup

### Prerequisites
1. **Visual Studio 2022** (or later)
   - Workload: .NET desktop development
   - Component: .NET 5.0 Runtime
2. **Windows 10/11** (required for WPF)
3. **.NET 5.0 SDK** installed

### Build Process
```bash
# Restore NuGet packages
dotnet restore

# Build solution
dotnet build Mapping_Tools.sln --configuration Release

# Build for specific platform
dotnet build --configuration Release --runtime win-x64
dotnet build --configuration Release --runtime win-x86
```

### Project Structure
- **Mapping_Tools.csproj** - Main application project
- **Mapping_Tools_Tests.csproj** - xUnit test project
- **lib/** - External DLLs not available via NuGet

### Configuration Files
- **App.config** - Runtime configuration
- **config.json** - User settings (generated at runtime)
- **Properties/Settings.settings** - Application settings

## Technical Constraints

### Platform Limitations
- **Windows-only**: WPF requires Windows
- **.NET 5.0 requirement**: Users must have runtime installed
- **x86/x64 builds**: Separate builds for architecture support

### Memory Access
- **Admin rights**: Some features work better with elevated privileges
- **Process access**: Requires osu! to be running for editor integration
- **Thread safety**: EditorReader access must be synchronized

### File Format
- **.osu file format**: Custom parser implementation
  - Must handle all osu! file format versions
  - Line-based parsing with section detection
  - Precision handling for timing values

### Performance Considerations
- **Large beatmaps**: Handle maps with thousands of objects
- **Background processing**: Use BackgroundWorker for long operations
- **Memory management**: Dispose resources properly
- **Binary search optimization**: Used extensively in Timing class

## Testing Framework

### Unit Testing
- **xUnit** - Test framework
- **Test Project**: Mapping_Tools_Tests
- **Test Resources**: Sample beatmaps in Resources/

### Test Categories
- Beatmap parsing and serialization
- Timing calculations
- Slider path generation
- Tool-specific algorithms
- Type converters

## Build Artifacts

### Output Files
- **Mapping Tools.exe** - Main executable
- ***.dll** - Dependencies
- **Data/** - Embedded resources (icons, images)
- **config.json** - User configuration (runtime)

### Installers
- **Installer_Script_x64.iss** - 64-bit installer script
- **Installer_Script_x86.iss** - 32-bit installer script
- Built using Inno Setup

### Distribution
- **GitHub Releases** - Primary distribution method
- **Automatic updates** via Onova
- **Release assets**: release_x64.zip, release.zip

## Development Tools

### Code Organization
- **.editorconfig** - Consistent code style
- **.gitignore** - Git exclusions
- **.gitattributes** - Git line ending configuration

### IDE Integration
- **Mapping_Tools.sln.DotSettings** - ReSharper settings
- Full IntelliSense support in Visual Studio
- XAML designer for UI development

## Runtime Environment

### Application Data
- **AppData Path**: %LOCALAPPDATA%\Mapping Tools
- **Backups**: Timestamped beatmap backups
- **Projects**: Tool-specific project files
- **Exports**: Generated files and samples
- **Crash logs**: crash-log.txt for unhandled exceptions

### User Configuration
- **osu! path detection**: Registry-based auto-detection
- **Songs folder**: Configurable, defaults to osu!/Songs
- **Backups folder**: Configurable, defaults to AppData/Backups

## External Libraries (Custom)

### EditorReader.dll
- **Purpose**: Read osu! editor memory
- **Location**: lib/EditorReader.dll
- **Source**: External project by Karoo13
- **Functionality**:
  - Hit object extraction
  - Timing point reading
  - Editor state monitoring
  - Bookmark extraction

### NonInvasiveKeyboardHookLibrary.Core.dll
- **Purpose**: Global hotkey support
- **Location**: lib/NonInvasiveKeyboardHookLibrary.Core.dll
- **Functionality**:
  - Non-invasive keyboard hooks
  - Hotkey registration
  - Key event handling

## Performance Optimizations

### Data Structures
- **Binary search**: Used in Timing class for O(log n) lookups
- **Sorted lists**: Maintain sorted timing points and hit objects
- **Lazy loading**: Tool views loaded on demand
- **Object pooling**: Minimize allocations in tight loops

### UI Optimizations
- **VirtualizingWrapPanel**: Efficient rendering of large lists
- **Background workers**: Keep UI responsive during operations
- **Progress reporting**: Incremental UI updates
- **Dispatcher**: Marshal UI updates from background threads

### File I/O
- **Streaming**: Line-by-line file reading
- **Buffered writes**: Efficient file writing
- **Temp files**: For editor reader dual-backup system