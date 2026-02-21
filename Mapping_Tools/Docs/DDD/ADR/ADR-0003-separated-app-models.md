# ADR-0003: Mostly Separate App Models for GUI and CLI

- **Status**: Accepted
- **Date**: 2026-02-21

## Context

GUI and CLI have different interaction patterns, validation feedback styles, and output expectations.
Trying to force one shared presentation model increases accidental coupling and weakens UX fit.

## Decision

Use mostly separate app models for GUI and CLI presentation layers.

Shared elements:

- domain model (`mt!core`)
- tool domain/application logic (`mt!toolX.dll`)
- common infrastructure ports

Separated elements:

- GUI presentation adapters/view models
- CLI presentation adapters/commands/formatters

## Consequences

### Positive

- better UX fit for each endpoint
- reduced UI-mode coupling
- clearer separation of presentation responsibilities

### Negative

- duplicate adapter-level wiring
- extra maintenance for parallel presenters

## Follow-Up

- Define consistent use case contracts to keep GUI and CLI behavior aligned.
- Add contract-level regression tests in future implementation phase.
