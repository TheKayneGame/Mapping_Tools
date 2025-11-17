# BDD Requirements for ColourPalette

## Feature: ColourPalette Management

### Scenario: Creating a ColourPalette with default values
**Given** a ColourPalette is constructed with no parameters  
**Then** its size should be 1  
**And** its maximum size should be 8  
**And** the palette should contain one default ComboColour (0,0,0)

### Scenario: Creating a ColourPalette with custom sizes
**Given** a ColourPalette is constructed with maxSize 5 and initialSize 3  
**Then** its size should be 3  
**And** its maximum size should be 5  
**And** the palette should contain three default ComboColours (0,0,0)

### Scenario: Creating a ColourPalette with invalid initial size
**Given** a ColourPalette is constructed with initialSize greater than maxSize  
**Then** an ArgumentOutOfRangeException should be thrown

**Given** a ColourPalette is constructed with initialSize less than 1  
**Then** an ArgumentOutOfRangeException should be thrown

### Scenario: Adding a ComboColour to the palette
**Given** a ColourPalette with available space  
**When** AddColour(ComboColour) is called  
**Then** the colour is added  
**And** the size increases by 1

### Scenario: Adding a ComboColour when palette is full
**Given** a ColourPalette at maximum size  
**When** AddColour(ComboColour) is called  
**Then** the method returns false  
**And** the palette size does not increase

### Scenario: Adding a default ComboColour
**Given** a ColourPalette with available space  
**When** AddColour() is called  
**Then** the size increases by 1

### Scenario: Removing a ComboColour
**Given** a ColourPalette with size greater than 1  
**When** RemoveColour() is called  
**Then** the size decreases by 1

### Scenario: Removing a ComboColour when size is 1
**Given** a ColourPalette with size 1  
**When** RemoveColour() is called  
**Then** the size decreases to 0 (potentially invalid, check implementation)

### Scenario: Setting a ComboColour at a valid index
**Given** a ColourPalette  
**When** SetColour(index, ComboColour) is called with a valid index  
**Then** the colour at that index is updated

### Scenario: Setting a ComboColour at an invalid index
**Given** a ColourPalette  
**When** SetColour(index, ComboColour) is called with an invalid index  
**Then** an ArgumentOutOfRangeException should be thrown

### Scenario: Accessing Colours property
**Given** a ColourPalette  
**When** Colours is accessed  
**Then** it returns a read-only list of ComboColours up to the current size
