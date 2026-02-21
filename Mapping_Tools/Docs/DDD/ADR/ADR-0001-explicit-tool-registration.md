# ADR-0001: Explicit Tool Registration

- **Status**: Accepted
- **Date**: 2026-02-21

## Context

Tool discovery in the legacy system relies heavily on runtime reflection of view/control types.
The target architecture requires predictable modular boundaries, versioned contracts, and app-host compatibility for GUI and CLI modes.

## Decision

Adopt explicit registration/manifest-based tool registration in `mt!framework`.

Each tool must declare:

- stable tool ID
- display name
- supported execution modes
- application use case entry points
- contract version

## Consequences

### Positive

- deterministic startup behavior
- safer modular loading and compatibility checks
- clearer deployment and diagnostics

### Negative

- additional manifest maintenance overhead
- less implicit convenience during prototyping

## Follow-Up

- Define registration schema in framework design docs.
- Add validation rules for manifest integrity in future implementation phase.
