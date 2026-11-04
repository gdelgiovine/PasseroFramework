# Pubs SQL Server integration tests (read-only)

The probe uses the existing **pubs** database, querying actual PasseroDemo.Models.
It performs no DDL, INSERT, UPDATE, DELETE, migrations or database creation.

## Preferred: Windows Integrated Authentication (self-hosted runner)

Configure repository **Actions variables** (Settings -> Secrets and variables -> Actions -> Variables):
- `PASSERO_DEMO_SQL_SERVER`: SQL Server hostname or `HOST\\INSTANCE` accessible from the runner.
- `PASSERO_DB_RUNNER_LABELS`: JSON array of existing Windows runner labels, e.g. `["self-hosted","Windows","X64"]`.
- Optional `PASSERO_DEMO_TRUST_SERVER_CERTIFICATE`: `true` only if the test server lacks a CA-trusted TLS certificate; otherwise leave it unset (TLS validation on).

The runner service must execute as an authorized Windows identity (domain service account, gMSA, or an appropriately configured machine identity). SQL Server must grant this identity database access and SELECT permissions to the demo tables. Merely installing a self-hosted runner does **not** confer Integrated Authentication permissions.

The workflow builds a SqlConnectionStringBuilder with database `pubs`, `IntegratedSecurity=true`, and `Encrypt=true`. Do not place credentials in the repo or workflow variables.

If GitHub-hosted runners are not domain-connected and cannot reach SQL Server, use a self-hosted runner with suitable network access and identity.

## Alternative: SQL connection secret

If integrated authentication is unavailable, use repository Actions secret
`PASSERO_DEMO_DB_CONNECTION_STRING` (never a variable). When `PASSERO_DEMO_SQL_SERVER` is configured it takes precedence.

## Results

If neither setting is present, the workflow prints **SKIPPED** and does not execute the LIVE integration probe. This is a configuration skip, *not* an integration-test PASS. SQL failure exits non-zero and fails the job.

Validation checks the connected database is named `pubs` and performs bounded SELECT queries against `dbo.authors` and `dbo.publishers` via Dapper, plus an EF Core read for Author. Note that these tests require the demo schema and model mappings to be compatible with the configured database.

Exit status: 0 verified; 1 failed; 2 not configured.
