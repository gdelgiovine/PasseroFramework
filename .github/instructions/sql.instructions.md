---
applyTo: "**/*.sql"
---

# SQL instructions

## General
- Preserve compatibility with the SQL dialect already present in the repository.
- Prefer readable SQL over overly compact SQL.
- Keep aliases meaningful.

## Safety
- Be careful with destructive statements.
- For schema changes, prefer explicit and reversible migration steps when the repo uses migrations.
- Do not drop or rename objects without highlighting impact.

## Performance
- Avoid `SELECT *` unless there is a strong reason.
- Be mindful of filtering, indexing implications, and join cardinality.
- Prefer explicit column lists in production queries.

## Maintainability
- Keep formatting consistent.
- Add comments only when the intent is not obvious from the query itself.

## SQL Server specifics
- Prefer T-SQL compatible syntax already used in the repository.
- When changing queries, consider existing indexes, parameterization, and execution plan impact.
- Avoid changing semantic behavior of joins or filters unless explicitly required.
- For reporting/ERP queries, preserve result shape and column names expected by application code.
