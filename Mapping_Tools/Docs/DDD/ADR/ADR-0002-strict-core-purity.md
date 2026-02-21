# ADR-0002: Strict Core Purity

- **Status**: Accepted
- **Date**: 2026-02-21

## Context

Legacy domain-like code is mixed with presentation and infrastructure concerns.
Target architecture requires `mt!core` to be reusable, testable, and independent of UI/host concerns.

## Decision

Enforce strict purity for `mt!core` from the start.

`mt!core` will contain only:

- domain entities, value objects, aggregates
- domain services and domain events
- domain contracts (ports)

`mt!core` will not reference UI frameworks, host globals, dialogs, registry/process APIs, or infrastructure implementations.

## Consequences

### Positive

- clean domain boundary
- easier testing and reuse across GUI/CLI
- reduced coupling and clearer ownership

### Negative

- higher upfront design effort for adapters/ports
- immediate pressure to model boundaries explicitly

## Follow-Up

- Maintain an architecture compliance checklist for every migration PR.
- Capture temporary exceptions only via explicit ADR updates.
