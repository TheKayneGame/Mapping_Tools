# Mapping Tools - Product Documentation

## Purpose

Mapping Tools is a comprehensive desktop application suite for osu! rhythm game beatmap creators. It automates complex mapping tasks, improves workflow efficiency, and enables creative possibilities that would be difficult or time-consuming to achieve manually.

## Problems Solved

### 1. Manual Tedium
- **Problem**: Creating beatmaps involves repetitive tasks like timing adjustments, hitsound placement, and object manipulation
- **Solution**: Automated tools handle bulk operations, pattern generation, and systematic adjustments

### 2. Complex Operations
- **Problem**: Advanced mapping techniques require precise mathematical calculations and geometric manipulations
- **Solution**: Tools like Geometry Dashboard, Sliderator, and Slider Completionator handle complex geometry automatically

### 3. Timing & Hitsounding Complexity
- **Problem**: Managing timing points, hitsounds, and sample libraries is tedious and error-prone
- **Solution**: Hitsound Studio provides layer-based management; Timing Helper automates timing adjustments

### 4. Limited Editor Capabilities
- **Problem**: osu! editor lacks tools for advanced pattern work, bulk editing, and visual design
- **Solution**: 20+ specialized tools extend editor capabilities non-invasively

## Core Features

### Essential Tools

**Map Cleaner**
- Removes redundant timing points (greenlines)
- Optimizes beatmap file structure
- Resnaps objects to timing
- Shows before/after timeline visualization

**Hitsound Studio**
- Layer-based hitsound management
- Import from MIDI, beatmaps, storyboards
- Advanced sample manipulation (volume, panning, pitch)
- Export to multiple formats (standard, coinciding, storyboard, MIDI)

**Slider Tools Suite**
- **Merger**: Combines multiple sliders into one continuous path
- **Completionator**: Fills slider patterns automatically
- **Sliderator**: Generates sliders following mathematical curves
- **Picturator**: Creates sliders from images

**Geometry Dashboard (Snapping Tools)**
- Real-time virtual object generation
- Geometric snapping (intersections, parallels, bisectors)
- In-game overlay for precise placement
- 40+ generator types

**Timing Suite**
- **Helper**: BPM detection and timing point generation
- **Copier**: Transfer timing between beatmaps

**Property Transformer**
- Batch editing of object properties
- Time/position/hitsound transformation
- Graph-based value interpolation

**Pattern Gallery**
- Store and reuse mapping patterns
- Group organization
- Quick pattern insertion

**Combo Colour Studio**
- Timeline-based combo color management
- Color interpolation and gradients
- Visual preview

**Tumour Generator**
- Algorithmic pattern generation
- Multiple template types (circle, parabola, square, triangle)
- Layered complexity control

**Metadata Manager**
- Bulk metadata editing across mapsets
- Consistent information management

**Rhythm Guide**
- Visual rhythm analysis
- Helps identify pattern opportunities

## How It Works

### Architecture Overview

**Desktop Application**
- WPF-based Windows desktop app
- Material Design UI
- Plugin-like tool architecture

**osu! Integration**
- **EditorReader**: Real-time monitoring of osu! editor state
- **Non-invasive**: Reads memory without modifying osu! client
- **Automatic updates**: Detects open editor and current beatmap

**File Processing**
- Direct .osu file manipulation
- Beatmap parsing and serialization
- Automatic backup system

### User Workflow

1. **Launch Application**
   - Tools organized in navigation drawer
   - Search and favorites for quick access
   - Each tool has dedicated view

2. **Select Current Beatmap**
   - Auto-detect from open editor
   - Manual file selection
   - Drag-and-drop support

3. **Configure Tool**
   - Tool-specific parameters
   - Project save/load for complex tools
   - Real-time preview (when applicable)

4. **Execute**
   - Automatic backup creation
   - Progress indication for long operations
   - Result summary with statistics

5. **Verify & Iterate**
   - Return to osu! editor to review changes
   - Quick undo via backup system
   - Re-run with adjusted parameters

### Key Differentiators

**Smart Integration**
- Reads editor state in real-time
- No need to save before processing (for single-map operations)
- Preserves currently open beatmap context

**Professional Features**
- Graph-based value editors for smooth transitions
- Timeline visualizations for temporal data
- Advanced mathematical operations

**Extensibility**
- Project-based workflow for complex tools
- Sample schema management for consistent hitsounding
- Pattern library system

**Safety**
- Automatic backup before every operation
- Quick undo functionality
- Validation and error checking

## User Experience Goals

### Efficiency
- Reduce time spent on repetitive tasks
- Enable batch operations across multiple difficulties
- Streamline complex workflows

### Precision
- Mathematical accuracy for geometric operations
- Snap-to-grid with multiple divisor support
- Timeline-based visual editing

### Creativity
- Enable techniques not possible manually
- Pattern generation and reuse
- Advanced visual design tools

### Reliability
- Always create backups
- Validate operations before execution
- Clear error messages and recovery options

### Learnability
- Intuitive Material Design interface
- Tool descriptions and tooltips
- Consistent interaction patterns across tools

## Target Users

- **Experienced Mappers**: Power users creating complex, technical maps
- **Aspiring Mappers**: Learning advanced techniques and improving efficiency
- **Ranked Mappers**: Professional-level quality control and consistency
- **Specialized Creators**: Technical maps, storyboard integration, advanced hitsounding