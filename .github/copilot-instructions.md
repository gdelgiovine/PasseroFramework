# Copilot instructions for this repository

## Language and communication
- Answer the user in Italian unless the request explicitly asks for English.
- Write source code, identifiers, commit-style summaries, and technical comments in English.
- Keep explanations concise and practical.
- When proposing changes, explain impact and touched files.

## General engineering rules
- Prefer minimal, safe, incremental changes over broad refactors.
- Do not rewrite working code unless there is a clear reason.
- Preserve existing architecture and naming conventions unless explicitly asked to improve them.
- Avoid introducing new external dependencies unless necessary and explicitly justified.
- Prefer deterministic solutions that are easy to review and test.

## .NET / C# defaults
- Target modern C# conventions and nullable reference types when already enabled in the project.
- Prefer async/await for I/O-bound operations.
- Use dependency injection instead of service locators or static state when possible.
- Prefer small methods with clear responsibilities.
- Validate inputs at boundaries.
- Prefer immutable DTOs/models where practical.
- Do not swallow exceptions. Either handle them meaningfully or propagate them.
- Use CancellationToken in async public APIs when appropriate.

## Architecture preferences
- Keep business logic out of controllers, UI code, and infrastructure glue.
- Prefer interfaces for services that are consumed across layers.
- Keep mapping logic explicit and easy to follow.
- Avoid hidden side effects.

## Code generation rules
- Before generating new files, check whether an existing file/pattern already solves the same problem.
- When editing code, preserve formatting and style already used in the repository.
- When adding a method/class, keep names aligned with domain terminology already present in the solution.
- Do not generate placeholder TODO code unless explicitly requested.

## Tests
- For new business logic, suggest or add tests when the project already contains a test suite.
- Prefer focused unit tests over large integration-style tests unless the scenario clearly needs integration coverage.
- Keep tests readable and deterministic.

## Output format for coding tasks
When proposing a change, prefer this structure:
1. Goal
2. Files to change
3. Main code changes
4. Risks / compatibility notes
5. Suggested validation steps

## Build and validation
- Prefer solution-level validation first when the repository clearly uses a single main solution.
- Suggested commands:
  - `dotnet build`
  - `dotnet test`
- If commands are repo-specific, infer them from existing files before inventing new ones.

## Pull request / review behavior
- Highlight breaking changes explicitly.
- Point out assumptions when repository context is incomplete.
- If there are multiple valid approaches, prefer the simplest one and briefly mention alternatives.

## Agent execution constraints
- Do not make broad refactors unless explicitly requested.
- Prefer editing existing files over creating new files.
- Before creating a new abstraction, verify whether an equivalent pattern already exists in the repository.
- Limit changes to the smallest set of files necessary.
- For each non-trivial change, explain why the file was touched.
- If a migration or schema change is needed, call it out explicitly before generating it.
- If confidence is low, state assumptions clearly instead of inventing repository conventions.

## Legacy and compatibility constraints
- Respect backward compatibility when the repository contains legacy integrations or long-lived application flows.
- Do not modernize code only for style reasons.
- When touching legacy code, prefer small containment improvements over architectural rewrites.
- Preserve public contracts, serialization shapes, database expectations, and integration points unless explicitly asked to change them.

## Repository hygiene
- Do not reformat unrelated files.
- Do not rename files, namespaces, or public members unless required by the task.
- Do not change package versions, SDK versions, or build configuration unless the task requires it.
- Do not generate database migrations or schema changes unless they are necessary for the requested change.
