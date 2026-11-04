# Pubs integration checks (read only)

Uses the existing **pubs** database and the actual demo model classes.
No DDL, INSERT, UPDATE, DELETE, database creation or migrations.

Configure the GitHub Actions repository secret `PASSERO_DEMO_DB_CONNECTION_STRING` with
a connection string to the demo SQL Server database. Prefer a SQL login with
SELECT-only permissions on `dbo.authors`, `dbo.publishers`, `dbo.titles`
and access to the SQL schema metadata.

**Do not commit the connection string** or print it to workflow logs.

The hosted Windows runner must be able to reach SQL Server (network routing/firewall).
When no secret is configured, the workflow explicitly reports the DB check as **SKIPPED**,
not PASS. Network connectivity and valid credentials are prerequisites for LIVE testing.

The application validates it is connected to the `pubs` database, then performs
limited SELECTs through Dapper and EF Core with the actual demo Models.

The standalone process exit status is: 0 = verified, 1 = failed, 2 = not configured.
