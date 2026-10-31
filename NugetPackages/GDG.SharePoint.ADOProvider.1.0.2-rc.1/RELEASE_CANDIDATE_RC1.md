# GDG.SharePoint.ADOProvider — Release Candidate 1

**Candidate version:** `1.0.2-rc.1` (NuGet SemVer 2.0 prerelease)  
**Release status:** candidate; not a stable 1.0.2 release  
**Targets:** `net48`, `net8.0`, `net10.0`  
**Platforms:** SharePoint Online and SharePoint Server Subscription Edition  
**Runtime:** SharePoint REST + `HttpClient`; no CSOM.

## Included

- ADO.NET provider factory, connection, command, parameter, data reader, data adapter and command builder.
- SQL-like SELECT, WHERE, ORDER BY, TOP, OFFSET/FETCH, JOIN, DISTINCT, aggregation/GROUP BY/HAVING, UNION/UNION ALL and supported DML/DDL operations as documented in `README.md` and `ADO_NET_1.0_CONFORMANCE.md`.
- Document-library virtual fields, file upload/download/update, and folder-scoped SELECT with `WITH (FOLDER=..., SCOPE=Current|Recursive|RecursiveAll)`.
- Dynamic folder binding via `FOLDER=@Folder` and deterministic `ORDER BY ... OFFSET ... FETCH` within scoped document libraries.
- Conservative scan limits that reject unsafe/incomplete local-filter results rather than silently returning partial data.
- WinForms tester, sample applications and deterministic offline/LIVE test suites.

## Release acceptance gates

This RC must **not** be described as validated or promoted to stable until the following **for the exact RC commit** have succeeded:

1. Release Package Validation: restore/build, one `.nupkg` and one `.snupkg`, package-content and nuspec checks, consumer smoke for `net48`, `net8.0` and `net10.0`.
2. GDG SharePoint ADOProvider Validation: build without introduced warnings, offline conformance on all three TFMs, and LIVE tests against SharePoint Online and SharePoint Server SE, including document-library integration on all three TFMs.
3. Post-merge `master` validation, including the shared 120-row fixture.
4. Confirm package version, commit identity and SHA-256 hashes before distributing the artifacts.
5. NuGet Security Audit `NuGetAuditMode=all` direct/transitive PASS on the exact SHA, with advisory source reachable.
6. Exactly three CycloneDX 1.6 SBOM sidecars generated from resolved project.assets.json, one per TFM, consistent with the package and SHA-256 manifest.
7. Commercial proprietary LICENSE.txt and THIRD_PARTY_NOTICES.md present and declared in NuGet nuspec; secure deployment/CRA guidance included without false CE or EU DoC claims.

The artifacts from GitHub Actions are **build outputs**, not evidence that a GitHub release or public NuGet publication has occurred. Only publish them when both workflows for the same `master` SHA pass. Keep the release/tag commit immutable.

## Distributed artifacts

After the Release Package Validation workflow completes successfully, download the `GDG.SharePoint.ADOProvider-nuget-<commit>` artifact from the corresponding `master` workflow run:

- `GDG.SharePoint.ADOProvider.1.0.2-rc.1.nupkg`
- `GDG.SharePoint.ADOProvider.1.0.2-rc.1.snupkg`
- `GDG.SharePoint.ADOProvider.1.0.2-rc.1.net48.cdx.json`
- `GDG.SharePoint.ADOProvider.1.0.2-rc.1.net8.0.cdx.json`
- `GDG.SharePoint.ADOProvider.1.0.2-rc.1.net10.0.cdx.json`
- `GDG.SharePoint.ADOProvider.1.0.2-rc.1.sha256`

The `.sha256` manifest contains SHA-256 digests for both packages and the three SBOMs. Verify the digest against the **exact** downloaded files before distribution. Keep the workflow run URL and validated `master` SHA in release evidence.

The `AssemblyVersion` remains `1.0.1.0` for binary compatibility; `FileVersion` is `1.0.2.0` and `InformationalVersion` identifies this prerelease. NuGet prerelease consumers must explicitly request `1.0.2-rc.1`; this version must not replace the stable `1.0.1` package.

## Intentionally unsupported / not certified for RC1

- `FOLDER/SCOPE` inside JOIN, UNION, nested scoped subqueries, DISTINCT, GROUP BY/HAVING, aggregate SELECT, scalar projections and EXPLAIN. Folder-scoped SELECT is limited to the explicitly documented simple-source syntax.
- General-purpose multiple result sets from `ExecuteReader`, and ADO.NET `DbDataAdapter.UpdateBatchSize > 1`.
- Native Taxonomy and Geolocation semantics, plus other PARTIAL/DEFERRED cases listed in `SHAREPOINT_TYPE_SUPPORT.md`.
- Arbitrary SharePoint versions beyond Online and Server SE, and arbitrary production tenant permissions or large-volume performance beyond the certified fixtures.

The .NET 10 target in PR #55 introduces no new SQL behavior or public API. Earlier net48/net8.0 PASS results are not evidence of .NET 10 certification; the .NET 10 CI and LIVE checks must pass on the exact RC commit before claiming this target as validated.

The release candidate introduces **no new SQL behavior** beyond what was validated in the preceding feature merges; the RC-specific changes are versioning, packaging verification, integrity manifests, and documentation.

## Tag and publication policy

Only after **all three** post-merge workflows (Validation, Release Package Validation, RC1 Security Audit) are green for the same immutable master SHA, the guarded release workflow may create annotated tag `v1.0.2-rc.1` and publish a **private-repository GitHub Prerelease** with both packages, three CycloneDX SBOMs and the SHA-256 manifest. All five manifest entries must be verified after artifact download. Never publish to NuGet.org, mark stable or declare CRA/CE conformity automatically.

The proprietary EULA grants production rights only under a separate signed commercial Order. The CRA technical dossier remains restricted; contact address, vulnerability contact, support period, classification and any EU declaration of conformity must be validated before external commercial distribution.

For diagnostics, include the GitHub workflow run IDs, exact commit SHA and the platform/TFM matrix in any RC1 sign-off.
