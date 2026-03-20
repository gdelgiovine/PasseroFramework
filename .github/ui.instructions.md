---
applyTo: "src/**/*.cs"
---

# UI instructions

## UI behavior
- Keep UI event handlers thin.
- Move business rules out of forms, pages, and UI components.
- Prefer delegating work to application services.

## State and interaction
- Avoid duplicating business validation already enforced in services/domain code.
- Keep UI state changes explicit and easy to trace.
- Prefer predictable event flows over implicit side effects.

## Wisej / WinForms style
- Preserve existing control naming and event wiring patterns unless explicitly asked to improve them.
- Do not redesign the UI unless requested.
- Prefer incremental changes that minimize regression risk in existing forms/pages.

## Error handling
- Show user-facing errors clearly, but keep technical details in logs when appropriate.
- Do not swallow exceptions silently in UI event handlers.
