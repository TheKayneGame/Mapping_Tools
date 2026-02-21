# DDD Communication Foundation

This folder is the source of truth for Domain-Driven Design communication artifacts for Mapping Tools.

Scope of this phase:
- Build shared language and architecture clarity.
- Define boundaries and dependency rules.
- Prepare tool-by-tool migration guidance.
- Do **not** perform implementation migration yet.

## Document Index

1. [01-context-map.md](01-context-map.md) — System context, bounded contexts, and relationships.
2. [02-ubiquitous-language.md](02-ubiquitous-language.md) — Shared domain vocabulary and canonical meanings.
3. [03-architecture-rules.md](03-architecture-rules.md) — Layering and dependency constraints.
4. [04-tactical-models.md](04-tactical-models.md) — Aggregates, entities, value objects, and services by context.
5. [05-integration-acl.md](05-integration-acl.md) — Integration points and anti-corruption layer guidance.
6. [06-tool-migration-playbook.md](06-tool-migration-playbook.md) — Future tool-by-tool migration checklist.

## ADRs

See [ADR](ADR) for architecture decisions that govern this DDD baseline.

- [ADR-0001-explicit-tool-registration.md](ADR/ADR-0001-explicit-tool-registration.md)
- [ADR-0002-strict-core-purity.md](ADR/ADR-0002-strict-core-purity.md)
- [ADR-0003-separated-app-models.md](ADR/ADR-0003-separated-app-models.md)

## Governance

- New terms must be added to `02-ubiquitous-language.md` before use in implementation proposals.
- Cross-context data flow changes must update `01-context-map.md` and `05-integration-acl.md`.
- Dependency changes must be validated against `03-architecture-rules.md`.
- Decisions that affect architecture direction require an ADR entry.
