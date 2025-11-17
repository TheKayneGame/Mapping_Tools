# Context Details — SnappingTools

Purpose
SnappingTools provides geometric generators and an in-game overlay to help place objects precisely using constructions like intersections, parallels, bisectors and slider path generators. It is a geometry-focused bounded context that supplies relevant objects to the Beatmap Editing context.

```mermaid
%% mermaid
classDiagram
  class SnappingAggregate {
    +GenerateRelevantObjects(beatmapArea, options)
    +GetLayer(layerId)
  }
  class RelevantObject {
    string Type
    string Data
    int Time
  }
  class GeneratorSettings {
    string Id
    string Name
    string Parameters
  }
  class LayerCollection {
    +Add(layer)
    +Remove(layer)
  }
  class Generator {
    +Generate(parameters)
  }
  class RelevantRepository {
    +Save(state)
    +Load(id)
  }
  class OverlayService {
    +Show(objects)
    +Hide()
  }
  class ObjectsGenerated <<event>>

  SnappingAggregate "1" -- "0..*" RelevantObject : produces
  SnappingAggregate ..> GeneratorSettings : <<configuredBy>>
  SnappingAggregate ..> LayerCollection : <<manages>>
  SnappingAggregate ..> OverlayService : <<uses>>
  SnappingAggregate ..> RelevantRepository : <<persists>>
  SnappingAggregate --> ObjectsGenerated
```

Assumptions
- SnappingTools contains many generator implementations under `SnappingTools/DataStructure/RelevantObjectGenerators/Generators/`; they are represented as a generic Generator type inside the aggregate.
- Persistent project state (SnappingToolsProject) and preferences map to RelevantRepository and GeneratorSettings.
- OverlayService denotes in-game overlay integration using Overlay.NET.

Related links
- [`README.md`](./docs/ddd/README.md:1)
- [`Context_Details_Sliderator.md`](./docs/ddd/Context_Details_Sliderator.md:1)