# Tactical Model Catalog

This catalog defines initial DDD tactical patterns for each bounded context.

## 1) Beatmap Composition Context

### Aggregate Roots
- **BeatmapAggregate**

### Entities
- `HitObject`
- `StoryboardEvent`
- `BreakPeriod`
- `ComboColourEntry`

### Value Objects
- `BeatmapId`
- `MetadataIdentity`
- `DifficultyProfile`
- `ColourScheme`
- `BeatTime`

### Domain Services
- `BeatmapIntegrityService`
- `BeatmapNamingPolicy`

### Domain Events
- `BeatmapMetadataUpdated`
- `BeatmapColoursUpdated`

## 2) Timing & Rhythm Context

### Aggregate Roots
- **TimingMapAggregate**

### Entities
- `TimingPoint`
- `TempoChange`
- `MeterChange`

### Value Objects
- `MillisecondsPerBeat`
- `TempoSignature`
- `BeatDivisor`
- `SnapLeniency`
- `SliderVelocity`

### Domain Services
- `ResnapService`
- `TimingReconstructionService`
- `SnapEligibilityService`

### Domain Events
- `ObjectsResnapped`
- `TimingMapRebuilt`

## 3) Hitsound Context

### Aggregate Roots
- **HitsoundProfileAggregate**

### Entities
- `HitsoundEvent`
- `SampleReference`
- `LayerAssignment`

### Value Objects
- `SampleSetRef`
- `AdditionSetRef`
- `CustomIndex`
- `SampleVolume`
- `SamplePath`

### Domain Services
- `HitsoundResolutionService`
- `SampleUsageAnalyzer`
- `HitsoundNormalizationService`

### Domain Events
- `HitsoundsNormalized`
- `UnusedSamplesDetected`
- `SampleReferencesPruned`

## 4) Map Maintenance Context

### Aggregate Roots
- **MaintenanceRunAggregate**

### Entities
- `CleanupTask`
- `ConsistencyIssue`
- `MaintenanceResult`

### Value Objects
- `CleanupPolicy`
- `CleanupScope`
- `ResnapPolicy`

### Domain Services
- `MapCleanupService`
- `ConsistencyRepairService`

### Domain Events
- `MaintenanceRunCompleted`
- `ConsistencyIssuesResolved`

## 5) Tool Runtime Context

### Aggregate Roots
- **ToolRegistrationCatalog**

### Entities
- `ToolModule`
- `ToolCapability`
- `ExecutionModeBinding`

### Value Objects
- `ToolId`
- `ToolVersion`
- `UseCaseContractVersion`

### Domain Services
- `ToolActivationService`
- `ToolExecutionRouter`

### Domain Events
- `ToolRegistered`
- `ToolExecutionStarted`
- `ToolExecutionCompleted`

## Cross-Context Repositories / Ports

- `IBeatmapRepository`
- `IMapsetRepository`
- `IToolRegistrationRepository`
- `ISettingsProvider`
- `IEditorReaderGateway`
- `INotificationPort`

## Application Service Pattern (All Contexts)

Each use case should follow:

- Input DTO (application contract)
- Validation
- Domain orchestration
- Repository/Gateway calls via ports
- Output DTO (result + diagnostics)
