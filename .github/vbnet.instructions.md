---
applyTo: "src/**/*.vb"
---

# VB.NET instructions

## General
- Preserve existing VB.NET coding style and naming conventions used in the repository.
- Do not translate VB.NET files to C# unless explicitly requested.
- Prefer minimal edits that keep compatibility with the current project structure.

## Design
- Keep business logic out of forms and UI event handlers.
- Prefer extracting reusable logic into services/modules only when this matches existing repository patterns.
- Avoid introducing C#-centric idioms that reduce readability in VB.NET.

## Safety
- Preserve Option Strict / Option Explicit / Option Infer settings already used by the project.
- Do not change project-wide VB compiler settings unless explicitly required.
