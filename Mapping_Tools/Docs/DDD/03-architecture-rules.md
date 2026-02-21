# Architecture Rules

This document defines mandatory dependency and layering constraints for the target architecture.

## Layer Definitions

- **Foundation**
   - `mt!core`: Pure domain model and shared domain operations for all tools.
  - `mt!infrastructure`: Technical adapters and external integrations.
- **Logic & Framework**
  - `mt!framework`: Tool runtime and host orchestration.
  - `mt!toolX.dll`: Tool module with Domain/Application/Presentation slices.
- **Apps**
  - `mt!App`: GUI host.
  - `mt! Console App`: CLI host.

## Dependency Direction

Allowed dependency flow:

1. Apps -> `mt!framework`
2. Tool Presentation -> Tool Application
3. Tool Application -> Tool Domain
4. Tool Domain -> `mt!core` domain primitives/services
5. Tool Application -> `mt!infrastructure` abstractions (ports)
6. `mt!infrastructure` -> external libraries / OS / file system

Forbidden dependency flow:

- `mt!core` -> any UI framework, host runtime, or infrastructure implementation.
- Tool Domain -> WPF/CLI/presentation types.
- Tool Application -> direct static global host state.
- Apps -> tool internals bypassing `mt!framework` contracts.
- Tool module -> another tool module internals.

## Strict Core Purity Rules

`mt!core` must not reference:
- `System.Windows.*`
- WPF controls, `Visibility`, `Color` UI types, dispatcher APIs
- file dialogs, process memory readers, registry access

`mt!core` may define:
- entities, value objects, aggregates
- domain services and domain events
- domain exceptions
- repository and gateway interfaces (ports)
- beatmap file contracts and mapping boundaries for Stable/Lazer formats
- shared base manipulation operations reusable across tools

`mt!core` must not define:
- tool-specific advanced algorithms that are only relevant to a single tool module

## Tool Module Structure Rule

Each tool module must be conceptually split into:

1. `Domain`
   - Advanced algorithms and manipulation operations specific to the tool domain.
2. `Application`
   - Use cases, orchestration, transactions, validation.
3. `Presentation.GUI`
   - GUI adapter models/controllers.
4. `Presentation.CLI`
   - CLI adapter models/controllers.

Presentation layers are replaceable adapters, not business rule owners.

Tool modules are separate assemblies loaded at runtime by `mt!framework`.

## Registration Rule

Tool discovery is explicit registration/manifest based.

Required metadata per registration:
- Tool identity (stable ID)
- Tool display name
- Supported execution modes (`GUI`, `CLI`)
- Input/output contract version
- Entry points for application use cases
- assembly compatibility information for runtime loading

No implicit reflection-only auto-discovery in production architecture.

## State & Configuration Rule

- Application services receive dependencies via constructor injection.
- No direct static access to host-wide global state in domain/application logic.
- Configuration is read through interfaces owned by application layer.

## Validation Gate

Any proposed implementation PR must pass this architecture checklist:

- Domain code free of UI/infrastructure references.
- Use case boundaries explicit.
- Tool registered via manifest contract.
- GUI and CLI adapters call the same application use case.
- New terms align with `02-ubiquitous-language.md`.
- Shared logic is in `mt!core`; tool-only logic remains in tool assemblies.
