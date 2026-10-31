# Secure deployment e informazioni al cliente — CRA Allegato II
Prodotto: GDG.SharePoint.ADOProvider v1.0.2-rc.1 (prerelease B2B). Titolare: Gabriele Del Giovine — GDG, Information Technologies Consulting. Sito: https://www.gabrieledelgiovine.it/.
**Recapito postale, email e punto unico per segnalazioni sicurezza:** DA INSERIRE e verificare prima della distribuzione commerciale; consultare SECURITY.md.

## Finalità e ambiente
ADO.NET provider per interrogazioni e mutazioni SharePoint REST da applicazioni autorizzate .NET Framework 4.8 / .NET 8 / .NET 10. SharePoint Online e SharePoint Server Subscription Edition supportati secondo README. Richiesti SharePoint, autorizzazioni appropriate, runtimes aggiornati, TLS/HTTPS e permessi di accesso minimi. Non è destinato a esporre un backend pubblico senza autenticazione/autorizzazione applicativa.

## Installazione e configurazione sicura
- Usare solo .nupkg autorizzati della esatta versione con SHA256 verificato sul manifesto di release.
- Gestire TenantID/ClientID distintamente dalle credenziali sensibili. Non includere Password, CertificatePassword, token, PFX o secret in repository, log e distribuzioni.
- Su SharePoint Online è obbligatorio HTTPS. Su Server SE preferire TLS e un segmento affidabile.
- In produzione impostare DiagnosticsEnabled=false; DiagnosticsPayloadEnabled=false è il default e deve essere cambiato soltanto per diagnosi temporanee protette.
- MaxBinaryDownloadBytes=134217728 limita i singoli download; 0 significa illimitato esplicito. Dimensionare memoria/concorrenza e imporre limiti applicativi.
- Limitare PageSize, MaxRows, MaxClientScanRows, timeout e modifiche massive; prevedere backup, retry/throttling e rollback secondo esigenze.
- Per applicazioni app-only, controllare l'autorizzazione dell'utente finale separatamente: i privilegi di SharePoint della app non garantiscono la segregazione degli utenti.
- Minimizzare il trattamento di dati personali, retention dei log e audit in relazione al GDPR, se applicabile.

## Aggiornamenti, vulnerabilità e supporto
Verificare le notifiche di sicurezza del titolare, aggiornare in ambiente di staging e registrare l'installazione. Usare SBOM del TFM di distribuzione per analizzare dipendenze. Support period e termine del supporto CRA: da deliberare e comunicare prima di impegni commerciali. Coordinated disclosure: SECURITY.md.

## Limiti
Non tutte le semantiche SharePoint sono coperte: Taxonomy, Geolocation, UserMulti e altre aree PARZIALI/DIFERITE; FOLDER/SCOPE supportato solo per forme SELECT dichiarate. Nessuna garanzia di performance arbitraria o su versioni SharePoint diverse da Online e Server SE. Vedere SHAREPOINT_TYPE_SUPPORT.md e RELEASE_CANDIDATE_RC1.md.
