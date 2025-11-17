using NUnit.Framework;
using Mapping_Tools.Domain.ColourHaxStudio;
using Mapping_Tools.Domain.Beatmaps;
using System;

namespace Mapping_Tools.Domain.Tests.ColourHaxStudio
{
    [TestFixture]
    public class ColourPaletteTests
    {
        // Tests that the default constructor creates a palette with one default colour and correct sizes
        [Test]
        public void Constructor_DefaultValues_CreatesPaletteWithOneDefaultColour()
        {
            // Arrange & Act: Create palette with default constructor
            var palette = new ColourPalette();
            // Assert: Palette should have one default colour and correct sizes
            Assert.That(palette.Size, Is.EqualTo(1));
            Assert.That(palette.MaxSize, Is.EqualTo(8));
            Assert.That(palette.Colours.Count, Is.EqualTo(1));
            Assert.That(palette.Colours[0], Is.EqualTo(new ComboColour(0,0,0)));
        }

        // Tests that the constructor with custom sizes creates a palette with the correct size and all default colours
        [Test]
        public void Constructor_CustomSizes_CreatesPaletteWithCorrectSizeAndColours()
        {
            // Arrange & Act: Create palette with custom maxSize and initialSize
            var palette = new ColourPalette(maxSize:5, initialSize:3);
            // Assert: Palette should have correct size and all colours should be default
            Assert.That(palette.Size, Is.EqualTo(3));
            Assert.That(palette.MaxSize, Is.EqualTo(5));
            Assert.That(palette.Colours.Count, Is.EqualTo(3));
            foreach (var colour in palette.Colours)
                Assert.That(colour, Is.EqualTo(new ComboColour(0,0,0)));
        }

        // Tests that an invalid initial size throws an ArgumentOutOfRangeException
        [Test]
        public void Constructor_InvalidInitialSize_ThrowsException()
        {
            // Act & Assert: Creating palette with invalid initialSize should throw exception
            Assert.Throws<ArgumentOutOfRangeException>(() => new ColourPalette(maxSize:2, initialSize:3));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ColourPalette(maxSize:2, initialSize:0));
        }

        // Tests that AddColour adds a colour when there is space available
        [Test]
        public void AddColour_AddsColour_WhenSpaceAvailable()
        {
            // Arrange: Palette with space for more colours
            var palette = new ColourPalette(maxSize:3, initialSize:1);
            // Act: Add a new colour
            var result = palette.AddColour(new ComboColour(1,2,3));
            // Assert: Colour should be added and size increased
            Assert.That(result, Is.True);
            Assert.That(palette.Size, Is.EqualTo(2));
            Assert.That(palette.Colours[1], Is.EqualTo(new ComboColour(1,2,3)));
        }

        // Tests that AddColour returns false when the palette is full
        [Test]
        public void AddColour_ReturnsFalse_WhenPaletteFull()
        {
            // Arrange: Palette already at max size
            var palette = new ColourPalette(maxSize:2, initialSize:2);
            // Act: Try to add another colour
            var result = palette.AddColour(new ComboColour(1,2,3));
            // Assert: Should not add and return false
            Assert.That(result, Is.False);
            Assert.That(palette.Size, Is.EqualTo(2));
        }

        // Tests that AddColour with no argument adds a default colour and increases the size
        [Test]
        public void AddColour_DefaultColour_IncreasesSize()
        {
            // Arrange: Palette with space for more colours
            var palette = new ColourPalette(maxSize:3, initialSize:1);
            // Act: Add a default colour
            var result = palette.AddColour();
            // Assert: Size should increase
            Assert.That(result, Is.True);
            Assert.That(palette.Size, Is.EqualTo(2));
        }

        // Tests that RemoveColour decreases the palette size
        [Test]
        public void RemoveColour_DecreasesSize()
        {
            // Arrange: Palette with more than one colour
            var palette = new ColourPalette(maxSize:3, initialSize:2);
            // Act: Remove a colour
            var result = palette.RemoveColour();
            // Assert: Size should decrease
            Assert.That(result, Is.True);
            Assert.That(palette.Size, Is.EqualTo(1));
        }

        // Tests that RemoveColour disallows the palette size to reach zero
        [Test]
        public void RemoveColour_DisallowsZeroSize()
        {
            // Arrange: Palette with one colour
            var palette = new ColourPalette(maxSize:3, initialSize:1);
            // Act: Remove the only colour
            palette.RemoveColour();
            // Assert: Size should not be zero
            Assert.That(palette.Size, Is.EqualTo(1));
        }

        // Tests that SetColour updates the colour at a valid index
        [Test]
        public void SetColour_ValidIndex_UpdatesColour()
        {
            // Arrange: Palette with two colours
            var palette = new ColourPalette(maxSize:3, initialSize:2);
            // Act: Set colour at index1
            palette.SetColour(1, new ComboColour(5,6,7));
            // Assert: Colour at index1 should be updated
            Assert.That(palette.Colours[1], Is.EqualTo(new ComboColour(5,6,7)));
        }

        // Tests that SetColour throws an exception for an invalid index
        [Test]
        public void SetColour_InvalidIndex_ThrowsException()
        {
            // Arrange: Palette with two colours
            var palette = new ColourPalette(maxSize:3, initialSize:2);
            // Act & Assert: Setting colour at invalid index should throw exception
            Assert.Throws<ArgumentOutOfRangeException>(() => palette.SetColour(-1, new ComboColour(1,2,3)));
            Assert.Throws<ArgumentOutOfRangeException>(() => palette.SetColour(2, new ComboColour(1,2,3)));
        }

        // Tests that the Colours property returns a read-only list of the current colours
        [Test]
        public void ColoursProperty_ReturnsReadOnlyListOfCurrentColours()
        {
            // Arrange: Palette with two colours
            var palette = new ColourPalette(maxSize:4, initialSize:2);
            // Act: Set colours
            palette.SetColour(0, new ComboColour(10,20,30));
            palette.SetColour(1, new ComboColour(40,50,60));
            var colours = palette.Colours;
            // Assert: Colours property should reflect current colours
            Assert.That(colours.Count, Is.EqualTo(2));
            Assert.That(colours[0], Is.EqualTo(new ComboColour(10,20,30)));
            Assert.That(colours[1], Is.EqualTo(new ComboColour(40,50,60)));
        }
    }
}