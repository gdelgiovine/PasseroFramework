---
applyTo: "tests/**/*.cs"
---

# Test instructions

## Test framework and style
- Prefer xUnit conventions unless the repository clearly uses another framework.
- Name tests clearly using behavior-oriented naming.
- Follow existing repository naming style before introducing a new one.

## Test quality
- Keep each test focused on one behavior.
- Avoid brittle assertions tied to implementation details.
- Prefer deterministic data and stable clocks; inject time/providers if needed.
- Avoid randomness unless seeded and justified.

## Assertions
- Prefer readable assertions.
- Verify observable behavior, not internal private implementation.
- Add negative-path tests for validation and error handling when relevant.

## Test doubles
- Use mocks/stubs sparingly.
- Prefer simple fakes when they improve readability.
- Do not mock value objects or simple data containers.

## Coverage guidance
- Prioritize business-critical paths, validation rules, and regression scenarios.
- For bug fixes, add a regression test when practical.
