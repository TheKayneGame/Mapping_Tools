# Tool Migration Playbook

This playbook defines how to migrate tools incrementally in the future using this DDD foundation.

## Purpose

- Migrate one tool at a time.
- Keep production behavior stable.
- Enforce architecture rules from `03-architecture-rules.md`.
- Reuse ubiquitous language from `02-ubiquitous-language.md`.

## Migration Unit

A migration unit is one tool module (`mt!toolX.dll`) with:

- Domain slice
- Application slice
- GUI adapter
- CLI adapter (if applicable)

## Readiness Checklist (Before Starting a Tool)

1. Tool scope documented in context terms.
2. Domain vocabulary for tool verified in glossary.
3. Use case contract candidates listed.
4. Infrastructure dependencies identified and mapped to ACL ports.
5. Host registration metadata defined.

## Execution Checklist (During Migration)

1. Extract tool domain rules to domain slice.
2. Define use case services and DTO contracts.
3. Move host/UI concerns to presentation adapters.
4. Replace direct external calls with port interfaces.
5. Register tool module explicitly in framework manifest.

## Verification Checklist (After Migration)

1. Domain/application contain no UI framework references.
2. Use cases callable from both GUI and CLI adapters where intended.
3. External integration only through ACL interfaces.
4. Tool registration metadata complete and versioned.
5. Terms in docs match implementation naming.

## Risk Controls

- Keep backward-compatible host routing until replacement path is validated.
- Use side-by-side execution toggle during adoption.
- Maintain deterministic output comparison for critical operations.

## Suggested Prioritization Model

Score each candidate tool by:

- Domain isolation ease
- Infrastructure dependency count
- UI coupling level
- business impact

Start with high impact and moderate extraction complexity.

## Deliverables per Migrated Tool

- Updated context map section
- Updated glossary entries (if new terms)
- Tool module registration manifest
- Use case contract documentation
- ACL mapping updates
