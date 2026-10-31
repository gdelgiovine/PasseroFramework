# Politica SBOM — v1.0.2-rc.1
- SBOM CycloneDX 1.6 JSON distinte per net48, net8.0, net10.0, generate nel packaging da project.assets.json dopo restore della versione esatta.
- Inventario: componente proprietary root identificato LicenseRef-GDG-Proprietary; pacchetti NuGet diretti/transitivi risolti e relazione tra pacchetti; licenza da .nuspec ove effettivamente dichiarata.
- PackageReference diretti: Microsoft.Identity.Client 4.87.0 e Newtonsoft.Json 13.0.3 (portale NuGet segnala MIT per entrambe); transitive da restore, NON da elenco manuale.
- Pacchetti runtime .NET, sistema operativo e servizi SharePoint/Entra non sono necessariamente pacchetti NuGet e rimangono piattaforme esterne.
- Custodire SBOM, test, SHA256, NuGet/Symbol package e prova di identità commit come insieme unitario; distribuire SBOM ad aventi diritto secondo accordo/richiesta dell'autorità. CRA non implica pubblicazione indiscriminata.
- Un componente senza metadata licenza verificati non deve essere etichettato MIT per analogia. La distribuzione deve rispettare le licenze terze parti e conservare gli avvisi previsti.
- SBOM non sostituisce audit vulnerabilità, license compliance, risk assessment, CE conformity o test applicativi. Nel workflow NuGetAudit controlla dipendenze dirette/transitive; l'assenza di Gitleaks non è evidenza di secret scan PASS.

Fonti: Regolamento (UE) 2024/2847 Allegato I parte II punto (1), Allegato VII; CycloneDX 1.6 specification.
