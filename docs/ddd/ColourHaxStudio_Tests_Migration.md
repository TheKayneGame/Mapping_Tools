[Memory Bank: Active]

# ColourHaxStudio — Tests migration

Purpose: actionable guidance to move and add tests when extracting the ColourHax domain.

Tests to move or add

- Move existing unit tests covering legacy project model:
  - [`Mapping_Tools_Tests/Classes/ComboColourStudio/ComboColourProjectTests.cs`](Mapping_Tools_Tests/Classes/ComboColourStudio/ComboColourProjectTests.cs:1)
- Add domain unit tests (MappingTools.Domain.Tests)
  - Colour resolver determinism
  - Policy implementations (GradientPolicy, PaletteMapPolicy)
  - ComboColourProject invariants (ordering, palette key existence)
- Add application integration tests (MappingTools.Application.Tests)
  - IColourHaxService.Preview and Apply orchestration using fakes
- Add infra adapter tests (MappingTools.Infrastructure.Tests)
  - Beatmap repository load/save roundtrip with sample .osu resources

Example fake IBeatmapRepository for unit tests

```csharp
// csharp
public class FakeBeatmapRepository : IBeatmapRepository
{
    private readonly Dictionary<string, Beatmap> storage = new();
    public Beatmap Load(string path)
    {
        if (!storage.TryGetValue(path, out var b)) throw new FileNotFoundException(path);
        return b.Clone(); // assume cloning exists for test isolation
    }
    public void Save(Beatmap beatmap, string path)
    {
        storage[path] = beatmap.Clone();
    }
    public void Seed(string path, Beatmap beatmap) => storage[path] = beatmap.Clone();
}
```

Suggested test matrix

- Domain unit tests (fast, pure)
  - Colour resolution: inputs -> expected combo-colour map
  - Policy determinism: same input => same output
- Application integration tests (fast-medium)
  - Preview uses FakeBeatmapRepository and returns expected mapping
  - Apply performs Save on repository and returns ExportResult.Success
- Infrastructure adapter tests (medium)
  - Legacy adapter delegates to existing [`Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs`](Mapping_Tools/Classes/Tools/ComboColourStudio/ComboColourProject.cs:1)
  - File repository roundtrip using test resources in [`Mapping_Tools_Tests/Resources/TestMap.osu`](Mapping_Tools_Tests/Resources/TestMap.osu:1)

Test organisation and CI

- Keep legacy tests in Mapping_Tools_Tests until domain API is stable.
- Add MappingTools.Domain.Tests and MappingTools.Application.Tests to CI.
- Use run-order: domain unit tests → application integration → infra adapter tests → legacy tests.

Migration checklist for tests

- [ ] Move tests that assert pure logic into MappingTools.Domain.Tests.
- [ ] Replace direct file system usage with FakeBeatmapRepository in application tests.
- [ ] Ensure mapping of existing tests to new coverage; avoid duplicate assertions.
- [ ] Keep at least one end-to-end smoke test in Mapping_Tools_Tests that uses real files.

Notes

- Preserve test data examples in Mapping_Tools_Tests/Resources for infra integration.
- Run test suite after each migration step; block merge on failing tests.