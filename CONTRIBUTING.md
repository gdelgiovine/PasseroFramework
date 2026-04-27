# CONTRIBUTING.md

## Linee guida per i contributi

Grazie per contribuire a Passero Framework. Segui queste regole per mantenere il codice coerente, leggibile e sicuro.

### 1. Formattazione e stile
- Indentazione: 4 spazi (no tabulazioni).
- Massima lunghezza riga: 120 caratteri.
- Usa nome file PascalCase per classi e camelCase per parametri e variabili locali.
- Prefix private fields con `_` (es. `_vmSalesmaster`) e rendili `private readonly` quando possibile.
- Proprietà pubbliche in PascalCase.
- Commenti in inglese per API pubbliche; commenti internamente nel repository possono essere in italiano quando rilevante.

### 2. .editorconfig
- Il repository deve includere un file `.editorconfig` che applica le regole sopra (indentazione 4 spazi, trimmare final whitespace, charset utf-8).

### 3. Naming conventions
- Classi, struct, enum: PascalCase
- Metodi: PascalCase
- Proprietà: PascalCase
- Campi privati: `_camelCase`
- Costanti: PascalCase
- Interfacce: I + PascalCase (es. `IRepository`)

### 4. Dependency Injection e gestione delle risorse
- Evitare `new` di servizi core (es. `ConfigurationManager`, `DbConnection`) direttamente nelle View; passali tramite costruttore (DI) o factory.
- Non esporre `IDbConnection` pubblicamente senza necessità; preferire repository o ViewModel per accesso ai dati.
- Chiudere e disporre connessioni e oggetti IDispoable.

### 5. Asincronia e I/O
- Preferire API `async`/`await` per operazioni I/O (database, file, rete).
- Fornire versioni sincrone solo quando strettamente necessarie per retrocompatibilità con .NET Framework.

### 6. Performance e query DB
- Evitare pattern N+1: caricare dati in batch quando possibile.
- Usare parametri nelle query (Dapper lo fa) e paginazione per liste grandi.
- Caching a livello ViewModel per lookup ripetuti (es. titoli, store) durante il ciclo di vita della view.

### 7. Logging ed error handling
- Centralizzare il logging (es. ILogger) e non lasciare try/catch vuoti.
- Restituire messaggi utente chiari tramite meccanismi centralizzati (ErrorNotificationMessageBox).

### 8. Test e CI
- Ogni feature significativa deve avere test unitari.
- Configurare pipeline CI (GitHub Actions) per build, lint e test.

### 9. Pull Request
- Intestazione PR chiara (tipo: feat/fix/refactor), descrizione dei cambiamenti, e link alle issue correlate.
- Target branch: `master` o `develop` a seconda del flusso del progetto.
- Eseguire `dotnet build` e `dotnet test` localmente prima di aprire PR.

### 10. Dipendenze e compatibilità
- Mantenere compatibilità multi-target (net48, net8.0) quando possibile.
- Aggiornare i pacchetti NuGet con attenzione; documentare breaking changes.

### 11. Pulizia del codice
- Rimuovere using inutilizzati.
- Evitare duplicazioni (estrarre helper o servizi).

---

Esempio rapido di best practice per field e DI in una Form:

```csharp
// Esempio: iniettare via costruttore
public partial class frmSales : Form
{
    private readonly ConfigurationManager _configurationManager;
    private readonly vmSalesmaster _vmSalesmaster;

    public frmSales(ConfigurationManager configurationManager, vmSalesmaster vmSalesmaster)
    {
        _configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
        _vmSalesmaster = vmSalesmaster ?? throw new ArgumentNullException(nameof(vmSalesmaster));
        InitializeComponent();
    }
}
```

Grazie per mantenere il progetto pulito e mantenibile.