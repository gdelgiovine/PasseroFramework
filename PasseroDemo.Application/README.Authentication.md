# Autenticazione della demo

## Architettura

L'autenticazione locale usa `IAuthenticationUserStore` e `ILocalAuthenticationProvider`. L'implementazione attuale, `JsonSecurityPrincipalProvider`, legge `Security/UsersAndRoles.json`; la pagina Wisej di gestione dipende dal contratto e non dal provider JSON, così un futuro provider SQL può implementare lo stesso contratto senza legare la UI al formato di persistenza.

Le password sono archiviate come hash PBKDF2-SHA256 con salt casuale e 210.000 iterazioni. Per modificare una password usare la pagina **Users and roles** oppure `scripts/Set-LocalAuthenticationPasswords.ps1`; non salvare password in chiaro nel JSON. Gli account `test` e `admin` presenti nel file di esempio usano entrambi la password `password`: è una credenziale dimostrativa debole e va cambiata prima di qualsiasi esposizione non locale.

La gestione utenti/ruoli è consentita agli utenti abilitati e non bloccati cui è assegnato il ruolo `role:administrator` direttamente o tramite il gruppo `group:administrators`. Deve rimanere almeno un amministratore abilitato e non bloccato. Il DTO mostrato dalla UI non include salt o hash delle password.

## OpenID Connect

Microsoft e Google sono configurabili solo nei target ASP.NET Core (`net8.0` e `net9.0`) tramite le impostazioni:

- `Authentication:Microsoft:ClientId`
- `Authentication:Microsoft:ClientSecret`
- `Authentication:Microsoft:Authority` (facoltativo)
- `Authentication:Microsoft:CallbackPath` (facoltativo)
- `Authentication:Google:ClientId`
- `Authentication:Google:ClientSecret`
- `Authentication:Google:CallbackPath` (facoltativo)

Usare User Secrets, variabili di ambiente o un secret store per i segreti; non aggiungere client secret ai file versionati. Le variabili di ambiente ASP.NET Core usano i nomi con doppio underscore, ad esempio `Authentication__Microsoft__ClientId` e `Authentication__Microsoft__ClientSecret`. Gli endpoint di challenge sono `/auth/microsoft` e `/auth/google`.

Il target `net48` mantiene l'host System.Web/IIS originale: OIDC federato non è abilitato su questo target. Non usare OWIN né riattivare l'autostart OWIN.

## JWT

Il validatore JWT legge `Jwt:SecretKey`, `Jwt:Issuer` e `Jwt:Audience` dalla configurazione ASP.NET Core. È necessario fornire tutti i valori; la chiave deve essere mantenuta fuori dal repository e avere entropia adeguata. La richiesta deve includere `Authorization: Bearer <token>` e seguire il percorso Wisej previsto dall'handler.
