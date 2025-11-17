
using Mapping_Tools.Domain.ColourHaxStudio;
using Mapping_Tools.Application;
using Mapping_Tools.Domain.Beatmaps;

namespace Mapping_Tools.Application.Tests
{
    public class ComboColourStudioServiceTests
    {
        private ComboColourStudioService service;
        private FakeComboColourProjectRepository repository;

        [SetUp]
        public void Setup()
        {
            repository = new FakeComboColourProjectRepository();
            service = new ComboColourStudioService(repository);
                service.LoadProject("test-path");
            if (service.CurrentProject == null)
                throw new InvalidOperationException("Failed to load test project.");
        }

        [Test]
        public void AddColourPoint_ShouldAddPoint()
        {
            service.AddColourPoint(1.23);
            Assert.That(service.CurrentProject.ComboColourPoints.Count, Is.EqualTo(1));
        }

        [Test]
        public void RemoveColourPoint_ShouldRemovePoint()
        {
            service.AddColourPoint(2.34);
            service.RemoveColourPoint(0);
            Assert.That(service.CurrentProject.ComboColourPoints.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddColourToPalette_ShouldAddColour()
        {
            var colour = new ComboColour(255, 255, 255);
            var result = service.AddColourToPalette(colour);
            Assert.That(result, Is.True);
        }

        [Test]
        public void RemoveColourFromPalette_ShouldRemoveColour()
        {
            service.AddDefaultColourToPalette();
            var result = service.RemoveColourFromPalette();
            Assert.That(result, Is.True);
        }

        [Test]
        public void FullFlow_Test()
        {
            // add colour to palette
            service.AddColourToPalette(new ComboColour(100, 150, 200));
            Assert.That(service.CurrentProject.ColourPalette.Size, Is.EqualTo(1));
            // add colour point
            service.AddColourPoint(3.45, new SortedSet<int> { 0 });
            Assert.That(service.CurrentProject.ComboColourPoints.Count, Is.EqualTo(1));
            // update colour point
            var newPoint = new ComboColourPoint(3.45, new SortedSet<int> { 0 }, ColourPointMode.Normal);
            var updateResult = service.UpdateColourPoint(0, newPoint);
            Assert.That(updateResult, Is.True);
            // 
        }



        // Fake repository for testing
        private class FakeComboColourProjectRepository : IColourHaxProjectRepository
        {
            private ColourHaxProject project = new ColourHaxProject();

            public ColourHaxProject Load(string path)
            {
                return project;
            }

            public void Save(ColourHaxProject project, string path)
            {
                // No-op for test
            }
        }
    }
}