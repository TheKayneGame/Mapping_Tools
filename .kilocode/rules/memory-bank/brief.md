# Mapping Tools - Project Brief

## Overview
Mapping Tools is a comprehensive desktop application suite designed to streamline and enhance the beatmap creation workflow for osu! rhythm game mappers. The project provides 20+ specialized tools that automate complex mapping tasks, improve workflow efficiency, and enable creative possibilities that would be difficult or time-consuming to achieve manually.

## Main Objectives
- Simplify complex beatmap editing operations through automation
- Provide professional-grade tools for timing, hitsounding, and visual design
- Enable advanced pattern generation and manipulation capabilities
- Reduce repetitive manual work in the mapping process
- Maintain compatibility with osu! file formats (.osu, .osb)

## Key Features

### Core Tools
- **Map Cleaner**: Automated beatmap optimization and cleanup
- **Hitsound Studio**: Advanced hitsound layer management and editing
- **Slider Tools**: Merger, Completionator, Sliderator, and Picturator for slider manipulation
- **Timing Suite**: Helper and Copier tools for precise timing adjustments
- **Snapping Tools**: Geometry-based placement with overlay support
- **Property Transformer**: Batch editing of object properties
- **Metadata Manager**: Centralized metadata editing across mapsets
- **Pattern Gallery**: Pattern storage and reuse system
- **Combo Colour Studio**: Advanced combo color management
- **Tumour Generator**: Algorithmic pattern generation
- **Rhythm Guide**: Visual rhythm analysis tool

### Technical Capabilities
- Real-time osu! editor integration via EditorReader
- Non-invasive keyboard hook system for hotkeys
- Audio processing with NAudio and Vorbis support
- Automatic updates via Onova
- Material Design UI with WPF

## Technology Stack

### Framework & Language
- **.NET 5.0** (Windows Desktop Runtime)
- **C#** with WPF (Windows Presentation Foundation)
- **XAML** for UI design

### Key Libraries
- **MaterialDesignInXamlToolkit**: Modern Material Design UI components
- **Newtonsoft.Json**: Beatmap and configuration serialization
- **NAudio & NAudio.Vorbis**: Audio processing and playback
- **OsuMemoryDataProvider**: Real-time osu! editor monitoring
- **Overlay.NET**: In-game overlay rendering
- **Onova**: Automated update system
- **Extended WPF Toolkit**: Enhanced UI controls
- **NonInvasiveKeyboardHook**: Global hotkey support

### Build System
- Visual Studio 2022 solution with Inno Setup installers
- Multi-platform builds (x86/x64)
- Comprehensive test suite with xUnit

## Project Significance

### For the osu! Community
- Empowers mappers with professional-level tools previously unavailable
- Significantly reduces time spent on tedious mapping tasks
- Enables complex techniques and creative approaches
- Active development with community feedback integration
- Open-source MIT license encouraging contributions

### Technical Achievement
- Complex geometry and mathematics for slider path manipulation
- Real-time editor integration without modifying osu! client
- Robust beatmap parsing and serialization system
- Sophisticated UI with graph-based editors and timeline visualizations
- Modular architecture supporting extensible tool development

## Development
- **Primary Developer**: OliBomby with community contributors
- **Project Management**: Trello board for feature tracking
- **Version**: 1.12.27 (actively maintained)
- **Repository**: GitHub with installer distributions
- **Support**: Ko-fi funding available