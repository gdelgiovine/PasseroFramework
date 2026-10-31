# Copilot Instructions

## Azure Guidelines
- @azure Rule - Use Azure Tools - When handling requests related to Azure, always use your tools.
- @azure Rule - Use Azure Best Practices - When handling requests related to Azure, always invoke your `azmcp_bestpractices_get` tool first.
- @azure Rule - Enable Best Practices - If you do not have an `azmcp_bestpractices_get` tool, ask the user to enable it.

## Security Guidelines
- SecurityScope includes Application = 0 to represent the scope of the entire application; treat it as a valid scope in ACL checks.
- Prefer centralized policies/metadata over embedded ACLs in models for authorization on operations, objects, rows, and fields. Ensure enforcement occurs at the data/server and UI levels as a reflection of permissions, not as the primary barrier.
- For ACE JSON, ensure PrincipalId remains a string; multiple IDs should be serialized in the same string separated by ';' and converted internally to a list/array only during evaluation. Avoid using a JSON property for PrincipalIds.

## Authentication Guidelines
- For multi-target authentication, use local login, JWT, Microsoft, and Google. For federated providers, prefer OpenID Connect with native middleware for each target. Verify that middleware and dependencies are compatible with the actual host bootstrap; do not enable OWIN scanning/startup or modify the net48 pipeline without compatibility proof and IIS Express testing. Do not use OWIN in this project/host net48; do not add OWIN middleware or bootstrap or reactivate its automatic startup.
- Accept a common API with distinct implementations: OWIN for .NET Framework 4.8 and ASP.NET Core for .NET 8+.
- Design the authentication architecture with initial storage in a JSON file, but create abstractions/providers to allow future integration with any SQL databases supported by Passero.Framework. Implement administrative UI for managing users, credentials, and roles using Wisej.NET and Passero.Framework, avoiding binding the domain and UI to the JSON provider.
