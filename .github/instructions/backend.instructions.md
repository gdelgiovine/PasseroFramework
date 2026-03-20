---
applyTo: "src/**/*.cs"
---

# Backend instructions

## Layering
- Keep controllers/endpoints thin.
- Put business rules in application/domain services, not in transport layers.
- Keep data access concerns in repository/infrastructure code.

## API and service design
- Prefer explicit request/response models over loosely typed structures.
- Validate external input early.
- Return meaningful errors; avoid ambiguous null-based flows.
- Favor clear contracts over clever abstractions.

## C# style
- Prefer expression clarity over terseness.
- Use `var` when the type is obvious from the right-hand side; otherwise prefer explicit types.
- Prefer guard clauses for invalid inputs.
- Keep methods short when possible.

## Async and performance
- Use async for I/O-bound paths.
- Avoid sync-over-async.
- Avoid unnecessary allocations in hot paths.
- Do not prematurely optimize; only optimize when there is evidence.

## Logging and diagnostics
- Log important state transitions and failures.
- Never log secrets, tokens, passwords, or sensitive personal data.
- Prefer structured logging patterns already used in the project.

## Data access
- Avoid N+1 query patterns.
- Keep query logic readable.
- Preserve transactional boundaries already present in the project.
