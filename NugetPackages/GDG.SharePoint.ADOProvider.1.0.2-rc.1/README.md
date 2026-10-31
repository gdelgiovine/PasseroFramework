# GDG.SharePoint.ADOProvider 1.0.2-rc.1 (Release Candidate 1)

**ADO.NET Data Provider REST-only per Microsoft SharePoint Online e SharePoint Server Subscription Edition (SE).**

**Autore:** Gabriele Del Giovine  
**Copyright:** (c) Gabriele Del Giovine 2026

**RC1 (prerelease):** questa build utilizza la versione NuGet `1.0.2-rc.1`; non è una release stabile né una pubblicazione automatica su NuGet.org. Il documento [RELEASE_CANDIDATE_RC1.md](RELEASE_CANDIDATE_RC1.md) descrive contenuto, limiti dichiarati, qualità richiesta e verifica SHA-256 degli artefatti. Il package include tale documento. L'AssemblyVersion rimane `1.0.1.0` per compatibilità binaria.

## 1. Obiettivo

`GDG.SharePoint.ADOProvider` espone liste e document library SharePoint attraverso il modello ADO.NET (`DbConnection`, `DbCommand`, `DbDataReader`, `DbParameter`, `DbProviderFactory`) e un linguaggio SQL-like. L'applicazione client non deve usare direttamente le API SharePoint.

A partire dalla versione 1.0.1.R, il provider adotta un'architettura **REST-only**: il runtime operativo comunica con SharePoint esclusivamente tramite SharePoint REST e `HttpClient`. Non esiste più alcuna dipendenza runtime da CSOM o da `Microsoft.SharePointOnline.CSOM`.

## 2. Architettura

```text
Applicazione .NET / ASP.NET Core
            |
            v
      ADO.NET API
  SharePointConnection
  SharePointCommand
  SharePointDataReader
  SharePointParameter
  SharePointProviderFactory
            |
            v
   SQL tokenizer/parser
            |
            v
       SQL AST / Plan
        /         \
       /           \
  SELECT       DML / DDL / DCL
     |                |
     v                v
    CAML          REST operations
     \                /
      \              /
       v            v
          RestRuntime
              |
              v
    SharePointRestTransport
              |
              v
           HttpClient
          /          \
         /            \
 SharePoint Online   SharePoint SE
 OAuth/Bearer        Windows auth
```

### 2.1 Perché REST-only

La stessa pipeline viene utilizzata per SharePoint Online e SharePoint SE e per tutti i target .NET supportati. Questo evita di mantenere un runtime CSOM differente per ogni combinazione di piattaforma e rende il target `net8.0` utilizzabile anche da applicazioni ASP.NET Core non Windows, compatibilmente con il meccanismo di autenticazione configurato sull'ambiente SharePoint.

Il provider usa **CAML come linguaggio di query server-side primario** per le scansioni SELECT, inviato via REST `GetItems(CamlQuery)`. Il relational query engine aggiunge un livello di pianificazione sopra questa pipeline: projection/filter pushdown continua a sfruttare CAML; quando una JOIN coincide con una relazione Lookup e il piano è traducibile senza perdita semantica, il planner può scegliere il percorso OData `GET .../items` con `$select`/`$expand`; le strategie multi-query possono usare `$filter` e `$batch`.

## 3. Target e compatibilità

Il provider è multi-target:

```xml
<TargetFrameworks>net48;net8.0;net10.0</TargetFrameworks>
```

Matrice prevista:

| Client | SharePoint Online | SharePoint SE |
|---|---|---|
| .NET Framework 4.8 / Windows | REST | REST |
| .NET 8 / Windows | REST | REST |
| .NET 10 / Windows | REST | REST |
| ASP.NET Core / Windows | REST | REST |
| ASP.NET Core / Linux | REST | REST* |
| ASP.NET Core / macOS | REST | REST* |

`*` Il protocollo REST è cross-platform. Per SharePoint SE con Windows Integrated Authentication, Kerberos/Negotiate/NTLM deve essere correttamente configurato sul sistema operativo e nell'infrastruttura. In particolare, su Linux/macOS l'autenticazione integrata dipende dalla configurazione Kerberos/GSSAPI disponibile sul sistema.

La matrice di supporto dei tipi colonna SharePoint è disponibile in [SHAREPOINT_TYPE_SUPPORT.md](SHAREPOINT_TYPE_SUPPORT.md). Le classificazioni PARTIAL/COMPATIBILITY restano conservative finché non sono certificate LIVE su Online e SE.

Il provider REST-only `net10.0` mantiene la stessa API del target `net8.0`; l'esecuzione LIVE sulle diverse piattaforme SharePoint richiede convalida distinta sui singoli runtime.

Il **Tester WinForms** rimane intenzionalmente Windows-only e multi-target:

```text
net48;net8.0-windows;net10.0-windows
```

## 4. Dipendenze

Il provider non dipende da CSOM.

Dipendenze NuGet principali:

- `Microsoft.Identity.Client` — autenticazione applicativa SharePoint Online;
- `Newtonsoft.Json` — payload SharePoint REST.

Sul target `net48` viene utilizzato `System.Net.Http` del framework.

## 5. ADO.NET

### 5.1 Classi principali

```text
SharePointProviderFactory         : DbProviderFactory
SharePointConnection              : DbConnection
SharePointCommand                 : DbCommand
SharePointDataReader              : DbDataReader
SharePointDataAdapter             : DbDataAdapter
SharePointCommandBuilder          : DbCommandBuilder
SharePointParameter               : DbParameter
SharePointParameterCollection     : DbParameterCollection
SharePointConnectionStringBuilder : DbConnectionStringBuilder
SharePointException               : DbException
```

Invariant name del provider:

```text
GDG.SharePoint
```

Esempio provider-agnostic:

```csharp
DbProviderFactories.RegisterFactory(
    "GDG.SharePoint",
    SharePointProviderFactory.Instance);

DbProviderFactory factory = DbProviderFactories.GetFactory("GDG.SharePoint");

using DbConnection connection = factory.CreateConnection();
connection.ConnectionString = connectionString;
connection.Open();

using DbCommand command = connection.CreateCommand();
command.CommandText = "SELECT ID, Title FROM Customers WHERE ID = @Id";

DbParameter parameter = command.CreateParameter();
parameter.ParameterName = "@Id";
parameter.DbType = DbType.Int32;
parameter.Value = 42;
command.Parameters.Add(parameter);

using DbDataReader reader = command.ExecuteReader();
while (reader.Read())
    Console.WriteLine(reader["Title"]);
```

## 6. Connection string e autenticazione

### 6.1 SharePoint Server SE - credenziali esplicite

```text
SiteUrl=https://sharepoint/sites/Test;
Mode=OnPremSE;
Authentication=NetworkCredential;
Domain=CONTOSO;
UserName=gabriele;
Password=secret
```

Il runtime configura `HttpClientHandler.Credentials` con `NetworkCredential`.

È supportata anche la forma `DOMINIO\\utente` in `UserName`.

### 6.2 SharePoint Server SE - identità del processo

```text
SiteUrl=https://sharepoint/sites/Test;
Mode=OnPremSE;
Authentication=DefaultNetworkCredentials
```

Alias accettato:

```text
Authentication=Windows
```

Il runtime usa `HttpClientHandler.UseDefaultCredentials = true`.

### 6.3 SharePoint Online - Access Token

```text
SiteUrl=https://contoso.sharepoint.com/sites/Test;
Mode=Online;
Authentication=AccessToken;
AccessToken=<token>
```

### 6.4 SharePoint Online - certificato / app-only

```text
SiteUrl=https://contoso.sharepoint.com/sites/Test;
Mode=Online;
Authentication=Certificate;
TenantId=<tenant-guid>;
ClientId=<app-guid>;
CertificatePath=C:\Certificates\app.pfx;
CertificatePassword=<password>
```

In alternativa è disponibile `CertificateThumbprint`.

Username/password legacy per SharePoint Online non è intenzionalmente supportato.

### 6.5 Opzioni operative

Fra le opzioni disponibili:

```text
ConnectionTimeout
RequestTimeoutSeconds
PageSize
MaxRows
MaxMutationItems
MaxSubqueryRows
MaxSubqueryExecutions
MaxSubqueryDepth
MaxClientScanRows
AllowUnfilteredMutations
SchemaCacheEnabled
SchemaCacheValidationSeconds
RequestDigestCacheEnabled
RequestDigestSafetyMarginSeconds
CostBasedOptimizer
PlanCacheEnabled
```


### 6.6 Schema cache e invalidazione tramite SharePoint Change Log

> Nota REST/OData: `SP.ChangeQuery.FetchLimit` e' `Edm.Int64`; nei payload `application/json;odata=verbose` usati da SharePoint SE il provider lo serializza come stringa JSON (es. `"1"`), come richiesto dalla rappresentazione OData verbose.

La cache dello schema è attiva di default:

```text
SchemaCacheEnabled=true;
SchemaCacheValidationSeconds=30
```

La cache è **process-wide ma identity-scoped**: connessioni successive allo stesso sito e con la stessa identità possono riusare catalogo liste/raccolte e metadata dei field. Questo è particolarmente utile per applicazioni che aprono connessioni ADO.NET brevi e ripetute, incluso il Tester. La modalità `AccessToken` mantiene la cache connection-local per evitare di condividere metadata security-trimmed tra bearer token potenzialmente riferiti a principal diversi.

L'invalidazione segue questa strategia:

```text
DDL eseguito dal provider
        |
        +--> invalidazione immediata della cache interessata
        +--> riallineamento del Web.CurrentChangeToken

Modifica esterna
        |
        +--> Web.CurrentChangeToken cambia
        +--> POST /_api/web/getchanges
             ChangeQuery: List + Field + ContentType
        +--> se esiste un cambio di schema: invalidazione
        +--> altrimenti: avanzamento del token e cache mantenuta

Nessuna modifica schema
        |
        +--> cache mantenuta
```

Nel percorso SQL normale il cold start non enumera più l'intero `/_api/web/lists`: per una lista identificata per titolo o GUID il provider esegue una singola GET mirata alla lista con `RootFolder` e `Fields` espansi, popolando insieme `RestListInfo` e `ListMetadata`. Il catalogo completo viene caricato solo quando serve realmente enumerare le liste, ad esempio `GetSchema("Tables")`, oppure come fallback per gli alias basati sulla root-folder che non coincidono con il titolo SharePoint. In scenari ASP.NET concorrenti il caricamento mirato è single-flight per identificatore e i metadata restano condivisi per ListId; liste diverse possono essere caricate in parallelo. Alla scadenza dell’intervallo di validazione una sola request per cache condivisa interroga il Change Log; le request concorrenti attendono e riusano il risultato, evitando un thundering herd.

Il provider usa un solo token di change log a livello Web, invece di interrogare separatamente il change log di ogni lista. Le modifiche a item/file non sono incluse nella `ChangeQuery` di validazione e quindi **non invalidano lo schema**. `SchemaCacheValidationSeconds` controlla la validazione periodica delle connessioni longeve; nelle connessioni brevi il token acquisito durante `Open()` viene confrontato con quello della cache senza una GET aggiuntiva. Impostando `SchemaCacheValidationSeconds=0` la validazione viene effettuata a ogni accesso alla cache.

Se il change token è scaduto/non valido o il Change Log non è disponibile, il provider adotta una strategia conservativa: invalida la cache e rilegge lo schema da SharePoint.

### 6.7 Request Digest cache (`/_api/contextinfo`)

Per SharePoint Server SE le richieste REST di scrittura richiedono un Form Digest. La cache e' attiva di default:

```text
RequestDigestCacheEnabled=true;
RequestDigestSafetyMarginSeconds=60
```

Il digest e' cacheato **process-wide e per Web/identita'**. Connessioni ADO.NET successive allo stesso `SiteUrl` con la stessa autenticazione Windows riusano quindi il `FormDigestValue` finche' rimane valido, evitando un `POST /_api/contextinfo` per ogni `INSERT`, `UPDATE`, `DELETE`, DDL o modifica permessi.

La scadenza usa `FormDigestTimeoutSeconds` restituito da SharePoint, sottraendo il margine di sicurezza configurato. Per lifetime insolitamente brevi il margine viene limitato a meta' della durata, in modo da non rendere inutile la cache.

Il refresh e' **single-flight**: se piu' request ASP.NET arrivano contemporaneamente quando il digest e' scaduto, una sola esegue `/_api/contextinfo`; le altre attendono e riusano il valore appena rinnovato. Disabilitando `RequestDigestCacheEnabled`, ogni operazione di scrittura non OAuth acquisisce un digest nuovo.

SharePoint Online con OAuth/Bearer non usa il Form Digest e quindi non esegue `/_api/contextinfo`.

Percorso caldo tipico per una mutazione multi-item su SE, dopo warm-up schema/digest:

```text
POST /getItems
POST /_api/$batch
```

senza round-trip `contextinfo` aggiuntivo.

## 7. Pipeline SQL

```text
CommandText
   |
   v
Tokenizer
   |
   +-- commenti -- / /* ... */
   +-- string literal
   +-- identificatori [ ... ]
   +-- statement separator ;
   |
   v
Parser
   |
   v
SqlPlan / AST
   |
   +-- SELECT -> CAML -> REST GetItems
   +-- INSERT -> REST item creation / $batch
   +-- UPDATE -> CAML selection + REST MERGE / $batch
   +-- DELETE -> CAML selection + REST DELETE / $batch
   +-- DDL    -> REST Lists / Fields
   +-- DCL    -> REST RoleDefinitions / RoleAssignments / inheritance
```

## 8. Commenti SQL e statement vuoti

Sono supportati:

```sql
-- commento fino a fine riga
SELECT ID, Title FROM Customers;
```

```sql
/* commento
   multilinea */
SELECT ID, Title FROM Customers;
```

I marker presenti dentro stringhe o identificatori non vengono interpretati come commenti:

```sql
SELECT [Code--Value]
FROM Customers
WHERE Title = 'A--B'
  AND Notes = '/* valore */';
```

Un testo formato esclusivamente da commenti/spazi è un **NoOp** e non produce richieste SharePoint:

```sql
-- SELECT TOP 100 ID, Title FROM Tasks ORDER BY ID
```

## 9. SELECT

Esempio:

```sql
SELECT TOP 100 ID, Title, Status
FROM Tasks
WHERE Status = 'Open'
ORDER BY ID DESC;
```

Il provider costruisce CAML server-side e lo invia via REST `GetItems`.

Sono supportati, secondo il grammar corrente:

- proiezione di colonne;
- `TOP`;
- `WHERE`;
- operatori logici e di confronto;
- ordinamento;
- parametri ADO.NET;
- lookup e tipi SharePoint gestiti dal metadata layer;
- predicati/server filtering e, quando necessario, filtraggio client-side protetto dai limiti configurabili.

Esempio parametrizzato:

```sql
SELECT ID, Title
FROM Customers
WHERE ID = @Id;
```

### 9.1 Predicati `BETWEEN` / `NOT BETWEEN`

Il motore predicati supporta intervalli inclusivi con limiti letterali, parametrici o riferimenti a campi compatibili con i confronti scalari esistenti:

```sql
SELECT ID, Title, Amount
FROM Orders
WHERE Amount BETWEEN @Min AND @Max;

SELECT ID, Title
FROM Orders
WHERE Amount NOT BETWEEN 100 AND 500;
```

`X BETWEEN A AND B` equivale a `X >= A AND X <= B`; i limiti non vengono riordinati. `NOT BETWEEN` applica il `NOT` SQL al predicato completo. Un campo o limite `NULL` produce `UNKNOWN` secondo la logica SQL ternaria, quindi la riga non supera un `WHERE`; `NOT` conserva `UNKNOWN`.

Per campi scalari Integer/Counter, Number/Currency, DateTime, Text/Choice, Guid, Boolean e Lookup/User singoli, il provider usa le stesse conversioni dei confronti `>=` e `<=`. Quando campo e due limiti sono rappresentabili in CAML, il pushdown è atomico: `BETWEEN` genera `And(Geq,Leq)` e `NOT BETWEEN` genera `Or(Lt,Gt)`. Limiti `NULL`, subquery scalari, confronti field-to-field della stessa riga, campi multi-value e tipi non ordinabili mantengono l'intero predicato per la valutazione locale; non viene mai inviato un solo lato dell'intervallo.

Nel relational engine il nodo logico resta nativo (`Between`, con lower e upper ordinati nella collection `Values`). CBO_V2 usa la selettività euristica `0.25`; CBO_V3, CBO_V4 e CBO_V5 riusano rispettivamente forma adattiva, binding fingerprint e fingerprint AST esistenti senza esporre valori raw.

### 9.2 Relational Engine V2 — Phase 1: Scalar Aggregation Engine

Senza `GROUP BY`, una SELECT aggregata rappresenta un unico gruppo implicito e restituisce sempre una riga:

```sql
SELECT COUNT(*) AS TotalRows,
       COUNT(Amount) AS AmountRows,
       SUM(Amount) AS TotalAmount,
       AVG(Amount) AS AverageAmount,
       MIN(Amount) AS MinimumAmount,
       MAX(Amount) AS MaximumAmount
FROM Orders
WHERE Status = 'Open';
```

`COUNT(*)` conta tutte le righe qualificate; `COUNT(field)` conta soltanto i valori non `NULL`. `SUM`, `AVG`, `MIN` e `MAX` ignorano i `NULL` e restituiscono `NULL` per input vuoto o interamente `NULL`; i due COUNT restituiscono zero. Una proiezione aggregata non può essere combinata con un campo ordinario finché `GROUP BY` non è supportato.

L'operatore logico è `SCALAR AGGREGATE`; la strategia fisica attiva è `LOCAL SCALAR AGGREGATE`. WHERE, JOIN e semi/anti-join vengono completati prima dell'aggregazione. Tutti gli aggregati della SELECT vengono calcolati in un solo passaggio sull'input. `TOP` e `MaxRows` limitano la riga finale, mai l'input: se un limite di sicurezza client impedisce una scansione completa, la query fallisce invece di restituire un aggregato parziale.

`COUNT` espone `Int64`; `SUM` su Integer/Counter espone `Int64`; `SUM` su Number/Currency e ogni `AVG` espongono `Double`; `MIN/MAX` conservano il tipo scalare supportato. Gli overflow interi sono checked e risultati/input `Double` non finiti vengono rifiutati. `GetCommandSchema` restituisce nomi, alias e tipi senza leggere item.

Il candidato REST-only `SERVER AGGREGATE` è intenzionalmente disabilitato finché `RenderListDataAsStream`/ViewXml Aggregations non sarà validato localmente, con fixture reali, sia su SharePoint Online sia su SharePoint Server Subscription Edition. Non viene usato `$apply`, CSOM o una View SharePoint persistente.

### 9.3 Relational query engine: JOIN ed EXPLAIN

Le SELECT possono combinare liste SharePoint con `JOIN`, `INNER JOIN`, `LEFT [OUTER] JOIN` e `CROSS JOIN`:

```sql
SELECT O.ID, O.Title, C.Title AS CustomerName
FROM Orders O
INNER JOIN Customers C ON O.CustomerId = C.ID
WHERE O.Status = 'Open'
ORDER BY O.ID;
```

Dalla 1.0.1.R il relational engine usa **CBO_V5** quando `CostBasedOptimizer=true`, `PlanCacheEnabled=true` e la validazione dello schema cache è disponibile. CBO_V5 riusa solo template fisici immutabili, indicizzati da identity endpoint, AST logico SHA-256, generazione schema, modalità optimizer, classi di selettività e ambiente di planning; valori parametro e letterali non sono inclusi né esposti. Ogni hit ribinda metadata e parametri correnti e rivalida lookup, `$expand`, join e probe. La cache è process-local (LRU 256, TTL 10 minuti), single-flight per chiave e viene resa obsoleta da DDL o da modifiche schema esterne rilevate dal Change Log. Sette q-error qualificati >= 4 possono richiedere una ricompilazione con cooldown di un minuto conservato tra generazioni della stessa chiave; gli errori durante il cooldown non vengono accumulati. Il q-error di sorgente accetta soltanto scansioni complete riconosciute dalla classificazione CBO_V3, quindi probe ID/key mirati non vengono confrontati con la cardinalità dell'intera lista; il q-error dell'output JOIN resta indipendente. Impostando `PlanCacheEnabled=false` restano disponibili CBO_V4/CBO_V3 e il percorso legacy esistente. Se `SchemaCacheEnabled=false`, CBO_V5 viene bypassato e EXPLAIN riporta il modo effettivo CBO_V4/CBO_V3 con `BYPASS_SCHEMA_CACHE_DISABLED`. La catena di stima resta CBO_V2, CBO_V3 e CBO_V4; un valore mai osservato, scaduto o privo di confidence usa la stima CBO_V3 corrente. Per ogni query JOIN il motore genera soltanto piani fisicamente validi per il metadata e le capability SharePoint correnti, li stima e seleziona quello con `EstimatedTotalCost` minimo. Il costo non può mai rendere eleggibile una strategia semanticamente non valida: i controlli di Lookup, `$expand`, pushdown e fallback restano vincoli di correttezza.

A livello query CBO_V4 confronta, quando disponibili:

1. **REST EXPAND / REST MULTI-EXPAND**, che elimina il join client e usa `GET .../items?$select=...&$expand=...` con navigation name derivato da `SP.Field.EntityPropertyName`;
2. **CLIENT LEFT-DEEP**, che costa separatamente fetch e algoritmo per ogni arco e può riordinare in sicurezza le componenti di soli `INNER JOIN`.

Per ogni JOIN client il candidato può combinare:

- `FULL SCAN` CAML;
- `FILTERED ID PROBE` (`ID IN (...) AND <predicato destro>`);
- `BATCHED ID PROBE` OData, con `$batch` quando sono necessari più chunk;
- `HASH JOIN (BUILD RIGHT)`;
- `HASH JOIN (BUILD LEFT)` per `INNER JOIN` quando il lato sinistro stimato è più piccolo;
- `NESTED LOOP` per condizioni non hashabili o quando risulta meno costoso;
- null-extension invariata per `LEFT JOIN`.

CBO_V3 mantiene separati `CardinalityEstimator` e `RelationalCostModel` ed enumera esaustivamente gli ordini connessi nelle componenti composte esclusivamente da `INNER JOIN` fino a otto archi. La sorgente radice resta stabile per preservare le opportunità REST; `LEFT JOIN`, `CROSS JOIN`, componenti oltre la soglia e dipendenze non ancora connesse sono barriere semantiche e mantengono l'ordine SQL. Le stime mancanti usano fallback conservativi riportati nella diagnostica.

CBO_V4 costruisce a ogni esecuzione un contesto immutabile dai valori correnti dei parametri usati nel predicato. Il fingerprint SHA-256 type-aware comprende identità normalizzata, `DbType`, tipo wire, marcatore NULL e rappresentazione culture-invariant. Le stringhe mantengono esattamente contenuto e maiuscole/minuscole. I predicati con più parametri usano un fingerprint composito ordinato stabilmente per nome e vengono appresi congiuntamente, senza assumere indipendenza. La chiave include inoltre un `PredicateBindingFingerprint` SHA-256 ricavato dalla visita ricorsiva del predicato: conserva struttura sinistra/destra, nodo, operatore, campo, kind e posizione delle espressioni, identità normalizzata delle occorrenze parametro, tipo e hash dei letterali e voci `Values`. In questo modo binding o contesti letterali diversi non condividono statistiche, mentre l'ordine della collection dei parametri resta irrilevante. Le chiavi `PV|...` non contengono valori raw e lo store separato è process-local, con TTL di 30 minuti e limite di 4096 entry. Non esiste plan cache: ogni esecuzione viene pianificata normalmente.

Le classi diagnostiche sono `VERY_SELECTIVE` (`≤1%`), `SELECTIVE` (`≤5%`), `MEDIUM` (`≤25%`), `BROAD` (`≤75%`) e `VERY_BROAD` (`>75%`); `UNKNOWN` indica assenza di una statistica parameter-specific utilizzabile. La stima numerica, non la classe, determina costi e piano fisico.

Le statistiche adattive sono condivise nel processo ma isolate per URL canonico del sito, GUID lista, InternalName/campo, forma del predicato e relazione di join. Non vengono salvate su file, database o SharePoint. Il feedback JOIN memorizza un fattore normalizzato `ActualRows / BaseEstimatedRows` (limitato a un intervallo conservativo) e lo applica alla stima base corrente; non riusa cardinalità assolute apprese con input di dimensioni diverse. Il feedback dei predicati viene raccolto soltanto da scansioni complete della sorgente, mai da `FILTERED ID PROBE` o `BATCHED KEY LOOKUP`, perché solo una scansione completa ha `List.ItemCount` come denominatore valido. Ogni entry conserva EWMA, varianza, conteggio campioni, prima/ultima osservazione, confidence e scadenza. Il TTL è 30 minuti e lo store è limitato a 4096 entry con rimozione delle entry scadute e successiva eviction deterministica least-recently-used. Un singolo campione resta sotto la soglia di utilizzo; con campioni maturi la stima base e quella osservata vengono fuse progressivamente in spazio logaritmico. Nessuna query o chiamata REST aggiuntiva viene eseguita per l'apprendimento.

La confidence è `sample * freshness * stability`, dove `sample = 1-exp(-count/5)`, `freshness = exp(-age/TTL)` e `stability = 1/(1+normalizedVariance)`. In assenza di campioni, con statistiche scadute o con confidence insufficiente, stime e coefficienti coincidono con CBO_V2.

La calibrazione dei costi locali accetta soltanto operatori con almeno 1 ms misurato e 100 righe complessive di build/probe; fixture minuscole e tempi sub-millisecondo vengono esclusi perché il rumore del timer sarebbe maggiore del segnale. Questa soglia non limita il feedback di cardinalità/selectivity.

La calibrazione REST usa un unico fattore conservativo condiviso per endpoint applicato al costo REST totale. Una singola osservazione che contiene round-trip, subrequest `$batch` e righe trasferite non identifica indipendentemente i tre coefficienti, quindi CBO_V3 non attribuisce lo stesso rapporto a componenti separati.

I predicati `WHERE` riferiti a una sola sorgente vengono separati e spinti nella relativa scansione CAML/REST. Il motore raccoglie inoltre solo i campi necessari a proiezione, filtri, ordinamento e condizioni di join.

Le forme semplici di `EXISTS`, `NOT EXISTS`, `IN (SELECT ...)` e `NOT IN (SELECT ...)` sono pianificate come **semi join / anti semi join** con un hash set di chiavi; le forme non riscrivibili continuano a usare l'esecutore di subquery esistente.

`EXPLAIN` restituisce il piano senza eseguire la scansione dati:

```sql
EXPLAIN
SELECT O.ID, C.Title
FROM Orders O
LEFT JOIN Customers C ON O.CustomerId = C.ID
WHERE O.Status = 'Open';
```

`EXPLAIN ANALYZE` esegue invece lo stesso piano fisico e aggiunge le metriche effettive:

```sql
EXPLAIN ANALYZE
SELECT O.ID, C.Title
FROM Orders O
JOIN Customers C ON O.CustomerCode = C.Code;
```

Il reader di EXPLAIN mantiene tutte le colonne precedenti. CBO_V4 aggiunge in coda `ParameterSensitive`, `ParameterClass`, `ParameterStatisticsSource`, `ParameterFingerprint`, `ParameterEstimatedSelectivity`, `ParameterStatisticsAge`, `ParameterStatisticsSampleCount` e `ParameterEstimateConfidence`. `ParameterFingerprint` espone soltanto `SHA256:<hex>` e mai il valore raw. La radice è parameter-sensitive se lo è almeno una sorgente; con fingerprint diversi usa `ParameterStatisticsSource=MULTIPLE` e lascia nullo il fingerprint aggregato. Le sorgenti non parametrizzate e le modalità CBO_V3/legacy lasciano nullo `ParameterEstimatedSelectivity`. Il dettaglio root riporta la versione effettiva `LEGACY / rule-based`, `CBO_V3` o `CBO_V4`. Le colonne CBO_V3 `StatisticsSource`, `StatisticsAge`, `StatisticsSampleCount` ed `EstimateConfidence` conservano il significato generico. EXPLAIN usa i parametri forniti ma non esegue scansioni dati aggiuntive.

`EstimateErrorFactor` è il q-error simmetrico con guardia a una riga: se stima e valore effettivo sono entrambi zero vale `1`; altrimenti vale `max(max(EstimatedRows,1)/max(ActualRows,1), max(ActualRows,1)/max(EstimatedRows,1))`.

Le stime usano `SP.List.ItemCount`, `PageSize`, `MaxRows`, la forma del predicato e la strategia fisica. `ItemCount` viene mantenuto coerente dopo gli INSERT/DELETE eseguiti dal provider e, per `EXPLAIN`/`EXPLAIN ANALYZE`, viene aggiornato tramite uno snapshot metadata breve (cache 30 s) di `Web.Lists`, così le modifiche effettuate da altri client non lasciano la cardinalità permanentemente obsoleta. Le selettività sono euristiche e servono al confronto dei piani; `EXPLAIN ANALYZE` affianca sempre i valori reali.

Il cost model usa **unità relative, non monetarie**:

```text
REST = 100 * REST_RTT + 5 * BATCH_SUBREQUEST + 0.02 * ROWS_TRANSFERRED
HASH = 0.04 * BUILD_ROWS + 0.015 * PROBE_ROWS
NESTED_LOOP = 0.08 * LEFT_ROWS * RIGHT_ROWS
TOTAL = REST + LOCAL(materialization/hash/nested/projection/sort)
```

`BuildSide`/`ProbeSide` sono valorizzati per gli hash join client-side; per `$expand` il build side è assente perché la JOIN viene delegata a SharePoint. Il refresh metadata usato dal planner non è incluso nel costo della query.

Le opzioni di connessione rilevanti sono:

- `CostBasedOptimizer` (default `true`): abilita il cost-based optimizer; `false` mantiene il comportamento rule-based e disabilita lettura/aggiornamento di ogni statistica adattiva;
- `ParameterSensitiveOptimizer` (default `true`): con CBO attivo abilita CBO_V4; `false` conserva esattamente CBO_V3. Non ha effetto quando `CostBasedOptimizer=false`;
- `PlanCacheEnabled` (default `true`): con CBO e schema cache attivi abilita CBO_V5; `false`, o `SchemaCacheEnabled=false`, conserva CBO_V4 quando la parameter sensitivity è attiva, oppure CBO_V3 quando è disattiva. La cache è process-local, LRU 256 e TTL 10 minuti;
- `MaxBatchLookupKeys` (default `2000`);
- `BatchLookupKeyChunkSize` (default `40`, massimo effettivo `100`).

Il pushdown `$expand` può ora coprire anche più JOIN Lookup indipendenti, purché formino una stella radicata nella prima sorgente (per esempio `Orders.CustomerId = Customers.ID` e `Orders.AgentId = Agents.ID`) e non sia presente un `WHERE` che richieda CAML. In questo caso il planner genera una singola scansione `/items` con più navigation property in `$expand`. Le JOIN Lookup concatenate (per esempio `Orders -> Customers -> Agents`) e i JOIN filtrati restano sul percorso CAML + relational engine finché il predicato completo non è traducibile in OData senza perdita semantica. Per una equi-join verso `ID` con filtro locale sul lato destro, il planner usa un probe CAML mirato `ID IN (...) AND <filtro destro>`, conservando insieme riduzione delle chiavi e server-side filtering. Lo stesso pushdown viene applicato ai predicati che, dentro `ON`, riferiscono esclusivamente la sorgente destra: anche con `LEFT JOIN` è semanticamente sicuro perché riduce soltanto i candidati destri e la null-extension delle righe sinistre resta invariata. Le subquery che contengono a loro volta JOIN non sono ancora trasformate dal semi-join planner e restano fuori dal percorso ottimizzato.


### 9.3.1 Suite reale Orders / Customers / Agents

La soluzione include `tests/GDG.SharePoint.ADOProvider.JoinIntegration`, una suite di integrazione reale per SharePoint Online/SE. Con l'opzione `--recreate` crea un fixture deterministico basato sulle liste `Agents`, `Customers` e `Orders`, quindi esegue J01-J18 e la validazione focalizzata BETWEEN: risultati inclusivi, `NOT BETWEEN`, NULL, parametri, CAML, fallback locale, JOIN, mutation e cache CBO_V5.

La connection string può essere fornita tramite `GDG_SP_CONNECTION` o `--connection`. Il file `samples/RelationalJoinSuite.sql` contiene le stesse query numerate per l'esecuzione manuale nel Tester.

### 9.3 Document library e colonne virtuali

Quando il target e' una **SharePoint document library**, il metadata layer aggiunge cinque colonne virtuali del provider:

- `FileName`: nome del file;
- `FileContent`: contenuto binario (`byte[]`), **deferred** e mai incluso implicitamente in `SELECT *`;
- `FolderPath`: percorso relativo alla root della document library;
- `FileSize`: dimensione in byte, read-only e **non deferred**; quando SharePoint espone il campo di sistema `File_x0020_Size`, viene materializzata nello stesso `POST getItems` della riga;
- `MimeType`: MIME type dedotto dal nome file in lettura e utilizzabile come hint HTTP durante upload/sostituzione del contenuto.

Queste colonne virtuali **non esistono sulle liste normali**. Una normale lista SharePoint puo' quindi avere un proprio campo reale chiamato `FileName`: in quel caso viene trattato come un normale campo SharePoint e non assume alcuna semantica documentale.

Il contenuto viene scaricato soltanto quando `FileContent` e' richiesto esplicitamente:

```sql
SELECT ID, FileName, FileContent
FROM Documents
WHERE ID = 42;
```

La selezione degli item continua sempre a usare CAML; per ogni riga che proietta `FileContent`, il provider esegue il download binario REST del file associato. `FileSize`, invece, non genera richieste REST aggiuntive: usa `File_x0020_Size` nella stessa risposta `getItems`. Se tale campo di sistema non e' esposto dallo schema della raccolta, `FileSize` non viene incluso in `SELECT *` e una proiezione esplicita non viene eseguita tramite fallback per-file.

`_UIVersionString` e' incluso nella superficie canonica di `SELECT *` per liste e document library, anche se SharePoint lo espone come campo di sistema hidden. Il valore ADO.NET e' la versione visuale corrente dell'item/documento (`1.0`, `1.1`, `2.0`, ...); il payload REST corrispondente e' `OData__UIVersionString`. La colonna e' read-only, eager e non introduce round-trip aggiuntivi. Non va confusa con `owshiddenversion`, che e' un contatore tecnico di concorrenza.

`ContentType` e `ContentTypeId` sono **eager**. `SELECT *` include entrambi nella superficie canonica anche quando SharePoint marca `ContentTypeId` come campo di sistema hidden; i campi hidden non appartenenti alla superficie canonica eager restano esclusi. Il `POST getItems` forza `ContentTypeId` e, come fallback testuale nello stesso payload, `FieldValuesAsText/ContentType`; non usa piu' `$expand=ContentType`, che SharePoint Server SE puo' rifiutare sul method endpoint `/getitems`. `ContentTypeId` e' il valore autorevole dell'item e il nome `ContentType` viene risolto anche tramite la mappa dei content type della lista conservata nella schema cache. La mappa viene caricata insieme ai metadata a cache fredda; a cache calda la `SELECT` continua quindi a richiedere un solo `POST getItems`. Il valore ADO.NET di `ContentType` e' il nome del content type come stringa, mentre `ContentTypeId` e' l'ID stringa (`0x...`).

La superficie canonica eager di `SELECT *` include inoltre, quando i relativi field esistono nello schema SharePoint, i system field `GUID`, `owshiddenversion`, `FSObjType`, `UniqueId`, `_ModerationStatus`, `_ModerationComments` e `CheckoutUser`. `owshiddenversion` e' il contatore tecnico utilizzabile per conflict detection ed e' distinto da `_UIVersionString`; `FSObjType` viene letto dalla proprieta' REST `FileSystemObjectType`; i field che iniziano con `_` sono mappati genericamente sul nome OData corrispondente (`_ModerationStatus` -> `OData__ModerationStatus`, ecc.). `Author`, `Editor` e `CheckoutUser` restano eager nello stesso `POST getItems`, ma in modo CAML-safe: il provider seleziona i companion scalar `<Field>Id` e usa `$expand=FieldValuesAsText` per il display value, evitando gli invalidi `$expand=Author`, `$expand=Editor` e `$expand=CheckoutUser` su SharePoint Server SE. Non vengono emesse query REST per riga. `GUID`, `owshiddenversion`, `FSObjType`, `UniqueId`, `_UIVersionString`, `_ModerationStatus`, `_ModerationComments` e `CheckoutUser` sono trattati come read-only; `UniqueId`/`GUID` sono normalizzati a `Guid`, mentre `owshiddenversion`, `FSObjType` e `_ModerationStatus` sono valori interi. Gli altri field hidden restano esclusi da `SELECT *`.

Per i campi tecnici delle document library (`FileRef`, `FileDirRef`, `FileLeafRef`) il provider mantiene CAML come linguaggio di query ma aggiunge, quando necessario, un `$select` REST esplicito alla chiamata `getItems`. Alcune build SharePoint Server non serializzano questi campi di sistema nel JSON basandosi soltanto sui `ViewFields` CAML; il `$select` serve esclusivamente a definire la forma della risposta REST e non cambia filtro, ordinamento o paging CAML.

## 10. INSERT

### 10.1 Riga singola

```sql
INSERT INTO Customers (Title, Email)
VALUES ('Mario Rossi', 'mario@example.com');
```

### 10.2 Multi-row VALUES

```sql
INSERT INTO Customers (Title)
VALUES
    ('1'),
    ('2'),
    ('3'),
    ('99'),
    ('100');
```

Ogni tupla deve contenere lo stesso numero di valori dell'elenco colonne.

`ExecuteNonQuery()` restituisce il numero totale di item creati. Su un INSERT eseguito tramite `ExecuteReader()`/`ExecuteScalar()`, il provider conserva gli ID SharePoint generati.

Per più righe il runtime usa REST `$batch` a blocchi, evitando un round-trip HTTP per item. La risposta REST viene ottimizzata in base alla API ADO.NET usata:

- `INSERT + ExecuteNonQuery()` invia `Prefer: return-no-content` e `Accept: application/json;odata=nometadata`; il provider necessita solo dello status delle sub-request e di `RecordsAffected`;
- `INSERT + ExecuteReader()` / `ExecuteScalar()` usa `odata=nometadata` ma non `return-no-content`, perché deve conservare gli ID generati;
- se SharePoint non onora `Prefer: return-no-content`, la risposta resta valida e viene comunque accettata.

### 10.3 INSERT in una document library = upload

Soltanto quando la destinazione e' una document library, `INSERT` usa la semantica di upload del file:

```sql
INSERT INTO Documents
    (FileName, FileContent, Title)
VALUES
    ('Contratto.pdf', @Content, 'Contratto 2026');
```

`FileName` e `FileContent` sono obbligatori per l'`INSERT` documentale. `FileContent` accetta un parametro `byte[]` oppure uno `Stream` leggibile. `FolderPath` e' opzionale e, se omesso o vuoto, indica la root della raccolta:

```sql
INSERT INTO Documents
    (FileName, FolderPath, FileContent, Title)
VALUES
    ('Contratto.pdf', 'Clienti/ACME/Contratti', @Content, 'Contratto ACME');
```

Le cartelle indicate devono gia' esistere. Per default l'`INSERT` documentale e' **create-only** e usa `overwrite=false`: se il file esiste gia' nello stesso `FolderPath`, l'operazione fallisce e non si trasforma implicitamente in un aggiornamento. Per la modifica ordinaria di un documento esistente si usa `UPDATE`.

L'overwrite durante `INSERT` e' ammesso soltanto quando viene richiesto esplicitamente:

```sql
INSERT INTO Documents
    (FileName, FolderPath, FileContent, Title)
VALUES
    ('Contratto.pdf', 'Clienti/ACME/Contratti', @Content, 'Contratto ACME')
WITH (OVERWRITE = TRUE);
```

In questo caso l'upload usa `Files/add(...,overwrite=true)`. `WITH (OVERWRITE = FALSE)` e' accettato ed equivale alla semantica create-only predefinita. `OVERWRITE` e' un'opzione esclusiva dell'`INSERT` verso una document library: sulle liste normali viene rifiutata e non viene mai ignorata silenziosamente. L'opzione riguarda l'upload del file identificato da `FolderPath + FileName`; non trasforma genericamente `INSERT` in un UPSERT SQL.

Per le **liste normali** non cambia nulla: `INSERT INTO Customers (...)` crea normali list item e non richiede mai `FileName` o `FileContent`. Se `Customers` possiede un campo SharePoint reale `FileName`, quel campo viene gestito normalmente.

## 11. SQL multi-statement

Sono supportati statement multipli separati da `;`:

```sql
INSERT INTO Customers (Title) VALUES ('1');
INSERT INTO Customers (Title) VALUES ('99');
INSERT INTO Customers (Title) VALUES ('100');
```

Il separatore viene riconosciuto lessicalmente: un `;` dentro una stringa, un commento o un identificatore non divide lo statement.

`ExecuteNonQuery()` esegue batch di mutazioni/DDL e aggrega `RecordsAffected` per le operazioni DML.

`ExecuteReader()` e `ExecuteScalar()` richiedono attualmente un solo statement effettivo: veri multiple result set con `NextResult()` non fanno parte della 1.0.1.R.

## 12. UPDATE

```sql
UPDATE Customers
SET Status = 'Inactive'
WHERE ID = 42;
```

Il provider:

1. genera CAML per individuare gli item;
2. applica il limite `MaxMutationItems`;
3. invia gli aggiornamenti REST con `MERGE`;
4. raggruppa aggiornamenti multipli tramite `$batch`;
5. richiede una risposta minimale (`odata=nometadata` + `Prefer: return-no-content`) perché `UPDATE` non deve restituire l'entità aggiornata.
5. restituisce il numero di righe aggiornate.

Il Tester mostra esplicitamente l'esito e il numero di righe aggiornate.

### 12.1 UPDATE di documenti

Su una document library, `UPDATE` puo' modificare indipendentemente metadata, contenuto, nome e percorso:

```sql
-- sostituzione contenuto
UPDATE Documents
SET FileContent = @Content
WHERE ID = 42;

-- rename
UPDATE Documents
SET FileName = 'Contratto-2026.pdf'
WHERE ID = 42;

-- move
UPDATE Documents
SET FolderPath = 'Archivio/2026'
WHERE ID = 42;

-- operazione combinata
UPDATE Documents
SET FileContent = @Content,
    FileName = 'Contratto-Finale.pdf',
    FolderPath = 'Archivio/2026',
    Title = 'Contratto finale'
WHERE ID = 42;
```

Un `UPDATE` che modifica soltanto normali campi SharePoint continua a usare il percorso list-item ottimizzato e il batching REST gia' previsto dal provider. Le operazioni su contenuto/nome/percorso attivano invece gli endpoint REST file nativi. `FileSize` e' read-only; `MimeType` in `UPDATE` e' ammesso solo insieme a `FileContent`.

## 13. DELETE

```sql
DELETE FROM Customers
WHERE ID = 42;
```

Il provider seleziona gli item mediante CAML e li elimina tramite REST. Per eliminazioni multiple utilizza `$batch`. Le sub-request `DELETE` usano response minimali/no-content e il runtime contabilizza gli status HTTP senza richiedere la rappresentazione degli item eliminati.

Il Tester mostra esplicitamente l'esito e il numero di righe eliminate.

Le mutazioni senza filtro sono protette dalla configurazione `AllowUnfilteredMutations`.

### 13.1 DELETE da document library

Su una document library, `DELETE` elimina il **file SharePoint** associato all'item selezionato e non tenta di eliminare soltanto il list item sottostante:

```sql
DELETE FROM Documents
WHERE ID = 42;
```

La selezione degli item da eliminare continua a essere effettuata tramite CAML. Per piu' documenti, le operazioni `DELETE` vengono raggruppate in `$batch` quando supportato dal percorso REST.

## 14. DDL

Le tabelle SQL-like corrispondono a liste SharePoint.

### 14.1 CREATE TABLE

```sql
CREATE TABLE Customers
(
    Title VARCHAR(255) NOT NULL,
    CustomerCode VARCHAR(50) UNIQUE,
    Email VARCHAR(255),
    Active BOOLEAN DEFAULT TRUE,
    CreditLimit DECIMAL(18,2),
    CreatedDate DATETIME
);
```

Il runtime crea la lista via SharePoint REST e i field tramite le REST API delle liste/field.

La visibilita' della nuova lista o document library nella **Quick Launch / navigazione veloce** puo' essere controllata con `QUICK_LAUNCH`:

```sql
CREATE TABLE Documents
(
    Protocol VARCHAR(50),
    DocumentDate DATE
)
WITH (
    TEMPLATE = DOCUMENT_LIBRARY,
    QUICK_LAUNCH = TRUE
);
```

`QUICK_LAUNCH = TRUE` imposta `SP.ListCreationInformation.QuickLaunchOption` su `On` e, dopo la creazione, forza anche `SP.List.OnQuickLaunch = true` tramite REST `MERGE`, verificando il valore persistito; `FALSE` applica analogamente `Off` / `OnQuickLaunch = false`. Il passaggio post-creazione rende il comportamento deterministico anche sulle farm SharePoint Server che accettano `QuickLaunchOption` ma non materializzano la voce nella navigazione veloce. Se l'opzione viene omessa, il provider non forza alcun valore e conserva il comportamento di creazione precedente/template di SharePoint. L'opzione e' valida sia per liste sia per document library.

### 14.2 CHOICE e MULTICHOICE

```sql
CREATE TABLE Products
(
    Title VARCHAR(255),
    Status CHOICE('Active', 'Inactive', 'Suspended') DEFAULT 'Active',
    Categories MULTICHOICE('Retail', 'Wholesale', 'Partner')
);
```

### 14.3 LOOKUP e MULTILOOKUP

```sql
CREATE TABLE Orders
(
    Title VARCHAR(255) NOT NULL,
    Customer LOOKUP(Customers, Title),
    RelatedProducts MULTILOOKUP(Products, Title)
);
```

Il provider risolve la lista target e l'InternalName del campo `ShowField`, quindi crea il field lookup tramite schema XML SharePoint REST.

Sono supportati anche nomi bracketed:

```sql
Customer LOOKUP([Customer Master], [Customer Code])
```

La creazione è ordinata in modo da consentire anche self-lookup verso `Title` o verso field normali creati nella stessa `CREATE TABLE`.

### 14.4 ALTER TABLE

```sql
ALTER TABLE Orders
ADD COLUMN Customer LOOKUP(Customers, Title);
```

La sintassi DDL implementata comprende:

- `ADD COLUMN`;
- `DROP COLUMN`;
- `ALTER COLUMN`;
- `SET/DROP DEFAULT`;
- `SET/DROP NOT NULL`;
- `SET/DROP UNIQUE`;
- `RENAME COLUMN`;
- `RENAME TO`;
- `SET (DESCRIPTION = ..., QUICK_LAUNCH = TRUE|FALSE)` per le proprietà della lista/library.

Le proprietà SharePoint della lista o document library possono essere aggiornate dopo la creazione con un unico `ALTER TABLE ... SET (...)`:

```sql
ALTER TABLE [Contratti]
SET (
    DESCRIPTION = 'Archivio contratti',
    QUICK_LAUNCH = TRUE
);
```

Le due proprietà sono indipendenti e possono essere specificate singolarmente. `DESCRIPTION = ''` rimuove la descrizione; `QUICK_LAUNCH = FALSE` rimuove la lista/library dalla navigazione veloce. Il runtime emette un unico REST `MERGE` su `SP.List`, usando rispettivamente `Description` e `OnQuickLaunch`, quindi rilegge soltanto le proprietà richieste per verificarne la persistenza. Le proprietà omesse non vengono modificate.

`RENAME COLUMN` modifica il display name SharePoint mantenendo l'InternalName.

### 14.5 DROP TABLE

```sql
DROP TABLE Customers;
```

Corrisponde alla rimozione della lista SharePoint.

## 15. REST mapping

| Funzione provider | SharePoint REST |
|---|---|
| Open / verifica sito | `/_api/web` |
| Enumerazione liste | `/_api/web/lists` |
| Metadata field | `/fields` |
| SELECT | `/getItems` con `SP.CamlQuery` |
| INSERT | `/items` |
| INSERT multipli | `/_api/$batch` |
| UPDATE | `/items(id)` + `MERGE` |
| UPDATE multipli | `/_api/$batch` + `MERGE` |
| DELETE | `/items(id)` + `DELETE` |
| DELETE multipli | `/_api/$batch` + `DELETE` |
| CREATE TABLE | REST Lists API |
| CREATE COLUMN | `fields/createFieldAsXml` |
| ALTER list/field | REST `MERGE` |
| DROP list/field | REST `DELETE` |
| BREAK/INHERIT permissions | `breakroleinheritance` / `resetroleinheritance` |
| GRANT/REVOKE | `RoleDefinitions` + `RoleAssignments` |

Per SharePoint SE le operazioni di scrittura acquisiscono automaticamente il Form Digest tramite `/_api/contextinfo`; il valore viene riutilizzato tramite una cache process-wide Web+identity scoped fino alla scadenza server-driven. Per SharePoint Online autenticato con Bearer token il Form Digest non viene richiesto.

## 16. Diagnostica CAML / REST

La diagnostica del provider è **opzionale** ed è attiva per impostazione predefinita. Può essere disabilitata nella connection string:

```text
DiagnosticsEnabled=false
```

oppure tramite `SharePointConnectionStringBuilder.DiagnosticsEnabled`. Quando è disabilitata, il runtime non accumula CAML/REST trace e non formatta/copia request/response diagnostiche; `LastCaml*` e `LastRest*` risultano vuoti. La diagnostica non genera mai richieste HTTP aggiuntive.

Il provider espone il CAML dell'ultimo comando:

```csharp
string caml = command.LastCaml;
IReadOnlyList<string> all = command.LastCamlQueries;
```

Le stesse informazioni sono disponibili sulla connessione:

```csharp
connection.LastCaml
connection.LastCamlQueries
```

`LastCaml` contiene l'ultimo CAML generato; `LastCamlQueries` conserva tutte le query CAML prodotte dall'esecuzione quando sono necessarie più query (per esempio paginazione/subquery/batch logici).

Il CAML viene registrato prima della richiesta REST, quindi rimane disponibile anche se SharePoint restituisce successivamente un errore.

`INSERT` e operazioni DDL che non usano CAML lasciano normalmente `LastCaml` vuoto.

Il trace REST è disponibile tramite:

```csharp
string rest = command.LastRestCommand;
IReadOnlyList<string> allRest = command.LastRestCommands;


`LastRestCommands` is operation-scoped: it contains the complete ordered REST trace generated by the most recent ADO.NET operation. For `UPDATE` and `DELETE` this normally includes the CAML `getItems` selection request followed by the write request or `$batch`, plus each logical batch sub-request. `LastRestCommand` is retained as a convenience alias for only the final trace entry.

connection.LastRestCommand
connection.LastRestCommands
```

Ogni entry mostra verbo HTTP, URL, header rilevanti, payload, status code, response headers e response body. `Authorization`, Form Digest e cookie sono mascherati. Per `$batch` vengono esposte anche le singole sub-request. I body diagnostici sono limitati a 64 KB per exchange per contenere l'overhead in memoria.

## 17. SharePointDataReader

`SharePointDataReader : DbDataReader` implementa la superficie ADO.NET necessaria a consumer generici, inclusi:

```text
Read
FieldCount
HasRows
RecordsAffected
GetName
GetOrdinal
GetValue
GetValues
IsDBNull
GetString/GetInt32/GetInt64/GetBoolean/GetDateTime/...
GetFieldType
GetDataTypeName
GetSchemaTable
NextResult
Close
```

È compatibile con scenari come:

```csharp
var table = new DataTable();
table.Load(reader);
```

## 18. Parametri

Sono supportati `DbParameter` e `DbParameterCollection`.

```csharp
using var cmd = connection.CreateCommand();
cmd.CommandText = "DELETE FROM Customers WHERE ID = @Id";

var p = cmd.CreateParameter();
p.ParameterName = "@Id";
p.DbType = DbType.Int32;
p.Value = 42;
cmd.Parameters.Add(p);

int deleted = cmd.ExecuteNonQuery();
```

Il parser normalizza le forme di parametro supportate dal provider.

## 19. Metadata ADO.NET

`SharePointConnection.GetSchema()` espone le common schema collections e collezioni SharePoint-specifiche.

Fra le collezioni:

```text
MetaDataCollections
DataSourceInformation
DataTypes
Restrictions
ReservedWords
Tables
Lists
Columns
Fields
Indexes
```

Per i field:

```text
COLUMN_NAME      = DisplayName SharePoint
BASE_COLUMN_NAME = InternalName SharePoint
```

`GetSchema("Columns")` può enumerare anche tutte le colonne senza richiedere obbligatoriamente un `TABLE_NAME`.

## 20. CommandBehavior

Sono gestiti i comportamenti rilevanti, inclusi:

```text
Default
CloseConnection
SingleResult
SingleRow
SchemaOnly
KeyInfo
```

`SchemaOnly` non può eseguire accidentalmente DML o DDL.

## 21. Async, cancellation e timeout

Il provider espone le API ADO.NET async appropriate e propaga `CancellationToken` al transport HTTP.

Il timeout del singolo comando è temporaneo e non modifica permanentemente il comportamento delle operazioni successive.

## 22. REST batching

Il runtime usa SharePoint REST `/_api/$batch` per ridurre i round-trip HTTP nelle mutazioni multiple.

Sono raggruppati a blocchi:

- INSERT multi-row;
- UPDATE che interessa più item;
- DELETE che interessa più item;
- INSERT consecutivi nello stesso batch SQL quando applicabile.

Il batching HTTP **non costituisce una transazione ADO.NET/ACID** e non introduce una garanzia di rollback globale.

### Response minimization per le mutazioni

Per ridurre traffico di rete, allocazioni e parsing JSON, il transport distingue fra mutazioni che richiedono dati di ritorno e mutazioni che richiedono soltanto l'esito:

```text
INSERT + ExecuteNonQuery   -> odata=nometadata + Prefer:return-no-content
INSERT + ExecuteReader     -> odata=nometadata, conserva gli ID
UPDATE                     -> odata=nometadata + Prefer:return-no-content
DELETE                     -> no-content / response minimale
```

Con diagnostica disabilitata, per una mutazione riuscita senza body il provider non materializza una stringa di risposta; se il server restituisce comunque un body, il contenuto viene drenato senza conservarlo per permettere il riuso efficiente della connessione HTTP.

## 23. Transazioni

SharePoint non offre una transazione relazionale multi-statement equivalente a un RDBMS. Il provider non simula quindi false garanzie ACID.

```csharp
connection.BeginTransaction();
```

è intenzionalmente non supportato e genera `NotSupportedException`.

## 24. DataAdapter, CommandBuilder e DbBatch

### 24.1 `SharePointDataAdapter`

La 1.0.1.R implementa `SharePointDataAdapter : DbDataAdapter`. La factory restituisce il DataAdapter tramite `CreateDataAdapter()` e, sui framework moderni, espone `CanCreateDataAdapter = true`.

Sono disponibili i quattro costruttori ADO.NET convenzionali:

```csharp
new SharePointDataAdapter();
new SharePointDataAdapter(selectCommand);
new SharePointDataAdapter(selectSql, connection);
new SharePointDataAdapter(selectSql, connectionString);
```

`Fill` e `FillSchema` utilizzano `SelectCommand` e la logica standard di `DbDataAdapter`, inclusa l'apertura temporanea della connessione quando il comando possiede una connessione chiusa.

```csharp
using var adapter = new SharePointDataAdapter(
    "SELECT ID, Title, Email FROM Customers ORDER BY ID",
    connection);

var table = new DataTable("Customers");
adapter.Fill(table);
```

`Update(DataTable)` / `Update(DataSet)` può usare comandi DML espliciti oppure `SharePointCommandBuilder : DbCommandBuilder`. Il builder genera automaticamente `INSERT`, `UPDATE` e `DELETE` per SELECT single-table che espongono la chiave del provider (normalmente `ID`). Usa `ConflictOption.OverwriteChanges`, quindi `UPDATE` e `DELETE` individuano la riga tramite la chiave e i parametri `Original`, senza confrontare tutte le colonne selezionate.

```csharp
using var adapter = new SharePointDataAdapter(
    "SELECT ID, Title, CompanyName FROM Customers", connection);
using var builder = new SharePointCommandBuilder(adapter);

var table = new DataTable("Customers");
adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
adapter.Fill(table);

// modifica/aggiunta/cancellazione di DataRow
table.Rows[0]["Title"] = "Nuovo titolo";

adapter.Update(table);
```

Per gli `INSERT` generati il builder imposta `UpdatedRowSource = FirstReturnedRecord`: il runtime REST restituisce l'`ID` SharePoint creato e `DbDataAdapter.Update()` lo rimappa sulla `DataRow` aggiunta. Nel tester le colonne `AutoIncrement` usano valori temporanei negativi per le nuove righe, evitando collisioni con gli ID SharePoint positivi già caricati.

> **Diagnostica DataAdapter:** durante `DataAdapter.Update()` il tester registra separatamente lo SQL generato dal `SharePointCommandBuilder` e accumula CAML/REST per ogni `INSERT`, `UPDATE` e `DELETE`; le operazioni interne di schema discovery non sovrascrivono più la traccia DML.

I comandi espliciti restano supportati e usano `SharePointParameter.SourceColumn` e `SourceVersion` secondo la semantica ADO.NET. `SharePointProviderFactory.CreateCommandBuilder()` restituisce il nuovo builder e, sui framework moderni, `CanCreateCommandBuilder = true`.

`SharePointCommandBuilder` opera sulle sorgenti fisiche single-table (liste e document library). `$ATTACHMENTS(<lista>)` resta aggiornabile tramite `SharePointDataAdapter` con `InsertCommand`/`UpdateCommand`/`DeleteCommand` espliciti: `DbCommandBuilder` quota sempre `BaseTableName` come identificatore singolo e reinterpretare una forma quotata come pseudo-tabella violerebbe la regola canonica dei nomi SharePoint tra `[]`.

Sono esposti anche gli eventi provider-specific `RowUpdating` e `RowUpdated`. `UpdateBatchSize` è vincolato a `1`: valori `0` o maggiori di `1` generano `NotSupportedException`, perché il batching del `DataAdapter` non è ancora implementato e non va confuso con il `$batch` REST usato internamente dal provider per singoli statement SQL multi-row.

### 24.2 Capability ancora non incluse

```text
DbBatch / DbBatchCommand
```

Su framework che espongono le capability corrispondenti la factory dichiara `CanCreateDataAdapter = true`, `CanCreateCommandBuilder = true` e `CanCreateBatch = false`. Il batching SharePoint REST è un dettaglio interno del runtime e non equivale all'API ADO.NET `DbBatch`.

## 25. Tester WinForms

Il progetto:

```text
GDG.SharePoint.ADOProvider.Tester
```

è multi-target:

```text
net48;net8.0-windows;net10.0-windows
```

Permette di:

- configurare la connection string;
- eseguire SQL;
- valorizzare parametri ADO.NET binari scegliendo un file dal filesystem: il tester lo carica come `byte[]` e imposta automaticamente `DbType.Binary`;
- visualizzare i result set;
- visualizzare il numero di righe modificate per UPDATE/DELETE;
- verificare SharePoint Online e SharePoint SE;
- testare entrambi i runtime .NET supportati su Windows;
- aprire dalla `MainForm` la form **Diagnostica ultima esecuzione**.

### Configurazione condivisa del Tester

Le build `net48` e `net8.0-windows` non usano più file di configurazione separati nelle rispettive directory `bin`. Per impostazione predefinita entrambe leggono e scrivono lo stesso file per-utente:

```text
%LOCALAPPDATA%\GDG.SharePoint.ADOProvider\Tester\TestEndpoints.config
```

Alla prima esecuzione, se il file condiviso non esiste, viene inizializzato dal `TestEndpoints.config` distribuito con il tester. Il percorso può essere sovrascritto con la variabile d'ambiente:

```text
GDG_SHAREPOINT_TEST_ENDPOINTS
```

La configurazione condivisa comprende endpoint Online/SE, modalità di autenticazione, query SQL, target selezionato, `PageSize`, `MaxRows`, request timeout, command timeout, lo stato **Diagnostica CAML / REST** e **Cost-Based Optimizer (CBO_V1)**. Diagnostica e CBO sono ON di default e possono essere commutati direttamente dalla `MainForm`.

### Parametri binari da file

La griglia **Parametri ADO.NET** contiene una colonna **Binary / File...**. Il pulsante `File...` della singola riga apre il filesystem, legge immediatamente il file selezionato con `File.ReadAllBytes`, imposta `DbType.Binary` e conserva il relativo `byte[]` come valore effettivo del parametro. Il percorso mostrato nella colonna `Value / File path` è solo la rappresentazione UI del file associato.

Questo meccanismo è generico e serve sia per `FileContent` delle document library sia per `FileContent` del modello `$ATTACHMENTS(<lista>)`. Esempio di parametro documentale:

```sql
INSERT INTO [Documents] (FileName, FileContent)
VALUES (@FileName, @Content)
```

Per un overwrite intenzionale del documento esistente:

```sql
INSERT INTO [Documents] (FileName, FileContent)
VALUES (@FileName, @Content)
WITH (OVERWRITE = TRUE)
```

Nella griglia si definisce `@FileName` come `DbType.String`; per `@Content` si usa **File...**, ottenendo un `SharePointParameter.Value` di tipo `byte[]`. È possibile creare più parametri binari (`@Content1`, `@Content2`, ...) e associare a ciascuno un file distinto.

Se un parametro viene impostato manualmente come `DbType.Binary`, la colonna `Value / File path` può contenere direttamente un percorso valido: all'esecuzione il tester lo converte comunque in `byte[]`. La diagnostica non serializza il contenuto binario e mostra soltanto il numero di byte (e, nella vista preliminare della griglia, il percorso del file).

### Form di diagnostica

`DiagnosticsForm` è accessibile direttamente da `MainForm` tramite **Diagnostica ultima esecuzione** e conserva lo snapshot dell'ultima operazione, anche se terminata in errore. Mostra:

- esito, operazione, timestamp e durata;
- target framework (`net48` / `net8.0-windows`), runtime, sistema operativo e architettura del processo;
- percorso della configurazione condivisa;
- connection string effettiva con password/token mascherati;
- `DataSource`, `ServerVersion` e stato connessione;
- `PageSize`, `MaxRows`, request timeout e command timeout;
- stato `CBO_V1 ON/OFF`;
- SQL e tipi di statement riconosciuti dal parser;
- parametri ADO.NET (nome, `DbType`, valore);
- righe restituite, `RecordsAffected`, scalar result e riepilogo;
- `LastCaml` / tutti i CAML generati per l'esecuzione;
- tutti i comandi REST dell'esecuzione, comprese le sub-request `$batch`;
- messaggio e stack trace completo dell'eventuale eccezione;
- vista raw copiabile negli appunti.

Le form mantengono la convenzione Visual Studio con file `.cs` e `.Designer.cs` separati.

## 26. Conformance project

`GDG.SharePoint.ADOProvider.Conformance` verifica la superficie ADO.NET senza dipendere dalle classi concrete quando non necessario.

La suite comprende test per:

- factory;
- connection string builder;
- parametri;
- reader e `GetSchemaTable()`;
- `DataTable.Load()`;
- commenti/no-op;
- multi-statement;
- multi-row VALUES;
- lookup DDL;
- parser permission DCL (`ALTER ... PERMISSION`, `GRANT`, `REVOKE`, role custom);
- diagnostica CAML/REST e opzione `DiagnosticsEnabled`;
- superficie e round-trip dell'opzione `CostBasedOptimizer`, inclusa validazione booleana.

I test di integrazione reali richiedono ovviamente un endpoint SharePoint Online/SE disponibile.

## 27. Sicurezza e permessi

La 1.0.1.R gestisce i permessi SharePoint REST su **liste, raccolte documenti e singoli item/documenti**. Le raccolte documenti usano lo stesso modello `SecurableObject` delle liste; un documento viene gestito tramite l'ID del relativo list item.

### 27.1 Ereditarietà lista/raccolta

```sql
ALTER TABLE Customers
BREAK PERMISSION INHERITANCE COPY;

ALTER TABLE Customers
INHERIT PERMISSIONS;
```

`COPY` conserva le role assignment ereditate quando viene creato lo scope univoco. Per sicurezza, `COPY` è anche il default se omesso. È possibile richiedere esplicitamente la semantica SharePoint `copyRoleAssignments=false` con:

```sql
ALTER TABLE Customers
BREAK PERMISSION INHERITANCE NO COPY;
```

È inoltre supportato:

```sql
ALTER TABLE Customers
BREAK PERMISSION INHERITANCE COPY CLEAR SUBSCOPES;
```

### 27.2 Ereditarietà item/documento

```sql
ALTER ITEM Customers(42)
BREAK PERMISSION INHERITANCE COPY;

ALTER ITEM Customers(42)
INHERIT PERMISSIONS;
```

Per una document library la stessa sintassi opera sul list item associato al documento:

```sql
ALTER ITEM Documents(125)
BREAK PERMISSION INHERITANCE COPY;
```

### 27.3 GRANT e REVOKE

```sql
GRANT READ
ON Customers
TO 'Sales';

GRANT EDIT
ON Customers(42)
TO 'Mario Rossi';

REVOKE READ
ON Customers
FROM 'Sales';
```

Sono accettati permission level multi-parola:

```sql
GRANT FULL CONTROL
ON Documents
TO 'Document Owners';
```

Il provider non usa ID di RoleDefinition hard-coded. I livelli built-in principali vengono risolti tramite `RoleDefinitions.GetByType`, così funzionano anche su Web localizzati; gli altri livelli e quelli custom vengono risolti per nome nelle `RoleDefinitions` del Web SharePoint. Esempio:

```sql
GRANT 'Document Approver'
ON Documents
TO 'Approvers';
```

Il parser accetta quindi `Read`, `Edit`, `Contribute`, `Design`, `Full Control`, `Approve`, `Limited Access`, `View Only`, `Restricted Read` e qualsiasi RoleDefinition personalizzata o localizzata. L'effettiva assegnabilità resta governata da SharePoint: per esempio alcuni ruoli di sistema/limited-access possono non essere assegnabili esplicitamente.

### 27.4 Risoluzione principal

Il principal specificato in `TO`/`FROM` viene risolto nell'ordine seguente:

1. SharePoint group per `Title`;
2. site user per `Title`, `LoginName` o `Email`;
3. per `GRANT`, se il valore ha forma login/e-mail/claims, `ensureuser` viene usato come fallback.

Le risoluzioni di RoleDefinition e principal sono cacheate per connessione per ridurre i round-trip REST. In caso di display name utente ambiguo è necessario utilizzare login name o e-mail.

`GRANT`/`REVOKE` non interrompono automaticamente l'ereditarietà: la modifica dello scope deve rimanere esplicita tramite `BREAK PERMISSION INHERITANCE`, evitando cambiamenti di sicurezza impliciti.

## 28. Allegati delle liste: `$ATTACHMENTS(<lista>)`

Gli allegati degli item di una lista SharePoint sono esposti tramite la pseudo-tabella canonica `$ATTACHMENTS(<lista>)`. Il nome contenuto tra parentesi è sempre il nome reale della lista e può essere quotato con `[]`.

```sql
SELECT *
FROM $ATTACHMENTS([Customers]);
```

La superficie logica espone:

- `ItemId` (`Int64`): ID dell'item SharePoint padre;
- `FileName` (`String`): nome dell'allegato;
- `FileContent` (`Byte[]`): contenuto binario deferred.

`FileContent` **non è incluso in `SELECT *`** e non viene quindi scaricato durante la normale enumerazione. Il download avviene soltanto quando la colonna viene proiettata esplicitamente:

```sql
SELECT ItemId, FileName, FileContent
FROM $ATTACHMENTS([Customers])
WHERE ItemId = @ItemId
  AND FileName = @FileName;
```

CRUD:

```sql
INSERT INTO $ATTACHMENTS([Customers]) (ItemId, FileName, FileContent)
VALUES (@ItemId, @FileName, @Content);

UPDATE $ATTACHMENTS([Customers])
SET FileContent = @Content
WHERE ItemId = @ItemId AND FileName = @FileName;

UPDATE $ATTACHMENTS([Customers])
SET FileName = @NewFileName
WHERE ItemId = @ItemId AND FileName = @OldFileName;

DELETE FROM $ATTACHMENTS([Customers])
WHERE ItemId = @ItemId AND FileName = @FileName;
```

`ItemId` è immutabile in `UPDATE`: per spostare logicamente un allegato a un altro item si usa `DELETE` + `INSERT`. Il rename viene eseguito in modo conservativo creando prima il nuovo allegato e cancellando l'originale soltanto dopo il successo della creazione. `WITH (OVERWRITE = ...)` resta una semantica esclusiva dell'`INSERT` nelle document library e non si applica a `$ATTACHMENTS(...)`.

I nomi SharePoint reali quotati non vengono reinterpretati. Per esempio:

```sql
SELECT * FROM [Attachments(ALFA)];
SELECT * FROM $ATTACHMENTS([Attachments(ALFA)]);
```

Il primo statement accede alla lista reale `Attachments(ALFA)`; il secondo ai suoi allegati.

## 29. REST Operations opzionali: siti, sicurezza e ciclo documentale

La classe `SharePointOperations` estende la libreria senza modificare il linguaggio
SQL del provider, `SharePointCommand` o l'API ADO.NET. Non introduce stored
procedure fittizie e non effettua provisioning implicito. Usa lo stesso transport
REST-only e le stesse modalità di autenticazione della connection string.
È necessario chiamare `Open()` ed effettuare le mutazioni esplicitamente.

### 29.1 Subsite e site collection

- `GetCurrentWeb()`, `ListSubsites(recursive: true)`: Web corrente e sottositi
  visitabili dall'identità autenticata, con limite `maxSites` e controllo dell'host.
- `UpdateCurrentWeb`: aggiorna titolo e/o descrizione del Web connesso, inclusa
  la root di una site collection quando la connessione punta a quella root.
- `CreateSubsite`, `UpdateSubsite`, `DeleteSubsite`: gestiscono Web discendenti
  tramite REST (la cancellazione è distruttiva).
- `SearchSiteCollections`: discovery tramite SharePoint Search.
  **Non** rappresenta un inventario tenant/farm completo: i risultati dipendono
  da indicizzazione, permessi e query rules.
- `CreateSiteCollection`, `GetSiteCollectionStatus`, `DeleteSiteCollection`:
  disponibili **solo in SharePoint Online** tramite `SPSiteManager`.
  La creazione supporta `STS#3` e `SITEPAGEPUBLISHING#0`; non crea Team sites
  connessi a Microsoft 365 Groups. Richiede privilegi SharePoint sufficienti.
  Non è offerta una falsa equivalenza OnPremSE: la creazione/eliminazione
  delle site collection di Server SE richiede un servizio amministrativo di farm.

### 29.1.1 Inventario delle site collection di SharePoint Online: Microsoft Graph

La nuova API `SharePointOperations.ListSiteCollections()` usa **Microsoft Graph
REST `GET /v1.0/sites/getAllSites`** (non SharePoint Search), con paginazione
`@odata.nextLink`. È distinta da `SearchSiteCollections()`, che rimane
discovery *best-effort* basata sull'indice SharePoint Search. I risultati Graph
includono `GraphId`, `SiteId`, `Title`, `Url`, `HostName`,
`DataLocationCode` e `IsPersonalSite`.

Questa enumerazione usa `SharePointOnlineSiteCatalog`, che possiede un
**HttpClient Graph e un'acquisizione MSAL application-only separati**. Non usa
`SharePointRestTransport`, `SharePointConnection` o il token REST SharePoint.
Il token è richiesto per `https://graph.microsoft.com/.default` e
richiede permesso **Microsoft Graph Sites.Read.All (Application)** con
**admin consent**. Il token SharePoint `AccessToken` e le cookie
`BrowserCookies` non possono essere passati a Graph.

```csharp
// Se TenantId/ClientId/CertificatePath sono già nella connessione Online:
using (var op = new SharePointOperations(connectionString))
{
    op.Open();
    var sites = op.ListSiteCollections(); // Graph REST application-only
}

// Alternativamente: client Graph standalone, senza aprire una connessione SharePoint.
using (var catalog = new SharePointOnlineSiteCatalog(
    tenantId, graphClientId, certificatePath, certificatePassword))
{
    var sites = catalog.GetAllSiteCollections();
}
```

Per usare credenziali Graph differenti nella classe `SharePointOperations`,
configurare `GraphTenantId`, `GraphClientId`, `GraphCertificatePath` e
`GraphCertificatePassword`. Per `ListSiteCollections()` sono necessari
tenant/client e un **certificato su file** (i soli cookie o AccessToken non
sono sufficienti). SharePoint Server SE genera `NotSupportedException`
per questa API: non tenta Graph, né una discovery Search sostitutiva.

L'enumerazione non filtra per l'host SharePoint di una singola connessione,
perché Graph può restituire più geografie. Il catalogo rifiuta i paging link
non HTTPS o fuori da `graph.microsoft.com/v1.0/sites/`. Se si supera
`maxSites` con pagine ancora disponibili, genera errore anziché dichiarare
un inventario parziale come completo.

### 29.1.2 Test delle operazioni REST (Online e Server SE)

La suite `OperationsLiveSuite` è integrata nel progetto
`GDG.SharePoint.ADOProvider.Conformance`, compilato su `net48` e `net8.0`.
Le verifiche offline includono parsing di Graph `getAllSites`, paginazione,
blocco cross-host, protezione OnPremSE e divieto di usare un token SharePoint
per Graph.

Il workflow `GDG SharePoint ADOProvider Validation` include un passo
indipendente `SharePointOperations Online and Server SE LIVE`. Con le
credenziali già presenti sul runner, effettua realmente per **entrambi**
i target e i framework: Web corrente, subsites ricorsivi, utenti,
site groups/memberships, RoleDefinitions e RoleAssignments.

- `GDG_SP_GRAPH_LIVE_ENABLED=true`: abilita un vero `getAllSites` LIVE;
  se manca il permesso Graph Sites.Read.All il test fallisce in modo visibile.
  Senza l'opzione la riga viene marcata **NOT CERTIFIED**, non PASS.
- `GDG_SP_OPERATIONS_MUTATIONS_ENABLED=true`: abilita i test CRUD di gruppi,
  memberships, ruoli, ACL e check-in/check-out **solo nei workflow_dispatch**.
  Gli oggetti sono nominati `GDGOPS_E2E_*` con suffisso casuale e sono
  eliminati dalla suite con cleanup controllato.
- `GDG_SP_OPERATIONS_SUBSITE_MUTATIONS_ENABLED=true`: abilita creazione,
  aggiornamento ed eliminazione di un sottosito fixture; richiede l'opzione
  precedente. SharePoint Online può impedire la creazione di sottositi
  secondo le policy del tenant.
- `GDG_SP_OPERATIONS_IMAGE_MUTATIONS_ENABLED=true`: su Online crea un campo
  Thumbnail nella propria lista temporanea, carica un PNG tramite
  `AddImage`, controlla il campo persistito e rimuove il file caricato;
  richiede anche le mutazioni generali.
- Le operazioni **amministrative di site collection** (create/delete),
  `RemoveSiteUser` e il CRUD di Web non fixture non sono eseguite
  automaticamente. Richiedono ambienti di prova esplicitamente dedicati.

Per avviare soltanto la suite di lettura:

```powershell
.\tests\Run-LiveValidation.ps1 -ConfigPath $env:GDG_SHAREPOINT_TEST_ENDPOINTS -OperationsOnly
```

I risultati riportano distintamente **PASS**, **FAIL** e **NOT CERTIFIED**.
I test non autorizzati dal relativo opt-in non sono certificati. La presenza
di permessi sufficienti va verificata prima di eseguire test distruttivi
su un tenant o farm di prova.

### 29.2 Utenti, gruppi e ruoli

- `ListUsers`, `EnsureUser`, `RemoveSiteUser`: operano su **site user SharePoint**;
  non creano, modificano o eliminano account Entra ID / Active Directory.
- `ListGroups`, `CreateGroup`, `UpdateGroup`, `DeleteGroup`,
  `ListGroupMembers`, `AddUserToGroup`, `RemoveUserFromGroup`:
  riguardano i **SharePoint site groups**, non i Microsoft 365 Groups.
- `ListRoles`, `CreateRole`, `UpdateRole`, `DeleteRole`:
  livelli di autorizzazione nel Web corrente.
- `ListRoleAssignments`, `AssignRole`, `RemoveRoleAssignment`,
  `BreakRoleInheritance`, `RestoreRoleInheritance`: operazioni su Web,
  liste e item secondo gli ID di principal e ruolo.
  Le chiamate rispettano le ACL SharePoint senza elevazione automatica.

### 29.3 Check-in, check-out, annullamento check-out

`CheckOutDocument`, `CheckInDocument` e `DiscardCheckOutDocument` accettano
titolo della document library e ID del documento, risolvono `SP.File` via REST
e utilizzano rispettivamente `checkout`, `checkin` e `undocheckout`.
`SharePointDocumentCheckInType` espone `Minor`, `Major`, `Overwrite`.
Il comportamento resta vincolato alle impostazioni della document library
(versioning, approval, checkout) e ai permessi dell'identità.

### 29.4 AddImage: campo Image, NON allegato

`AddImage(listTitle, itemId, columnInternalName, fileName, content)` implementa
una semantica **separata da `$ATTACHMENTS`**:

1. Verifica che il target sia un campo scrivibile `Thumbnail`, l'item esista
   e sia disponibile la document library `SiteAssets`.
2. Carica il file in `SiteAssets/Lists/{ListGuid}` usando un nome univoco.
3. Imposta nel campo Image dell'item il riferimento JSON `thumbnail`
   al file caricato (e l'ID del file se SharePoint lo restituisce).
4. In caso di errore nell'aggiornamento prova a eliminare il file caricato;
   eventuali cleanup falliti richiedono verifica manuale.

Supportati JPEG/PNG/GIF/WebP/BMP, limite esplicito 20 MB. La funzione è
attualmente limitata a SharePoint Online perché la compatibilità dei campi
`Thumbnail` su Server SE deve essere verificata. Non è equivalente alla
creazione di un allegato e non usa `AttachmentFiles`.

### 29.5 Esempio C# (API separate dal provider SQL)

```csharp
using (var op = new SharePointOperations(connectionString))
{
    op.Open();
    var users = op.ListUsers();
    var groups = op.ListGroups();
    var subsites = op.ListSubsites(recursive: true);

    op.CheckOutDocument("Documents", 42);
    // Modifica del file mediante il consueto accesso dati.
    op.CheckInDocument("Documents", 42, "Aggiornamento",
        SharePointDocumentCheckInType.Major);
}
```

**Stato test:** i test di superficie offline verificano validazione degli input,
nomenclatura, vincoli di piattaforma e lifecycle. I test LIVE delle operazioni
amministrative e di modifica documentale sono volutamente non distruttivi nelle
pipeline standard e richiedono ambienti/fixture isolati e privilegi adeguati.

## 30. Limitazioni intenzionali della 1.0.1.R

- nessuna transazione ACID multi-statement;
- `ExecuteReader()` non espone ancora multiple result set SQL;
- nessun provider Entity Framework Core incluso;
- `SharePointCommandBuilder` supporta generazione DML single-table; query senza chiave/proiezioni non aggiornabili restano intenzionalmente non generabili;
- l'autenticazione SharePoint SE cross-platform dipende dalla configurazione Kerberos/Negotiate/NTLM dell'ambiente;
- REST e CSOM non hanno una superficie 1:1 globale, ma il runtime 1.0.1.R implementa il perimetro necessario al provider: liste, field, item, query CAML, DML, DDL e metadata.

## 30. Prestazioni del transport REST

Il runtime minimizza il dialogo con SharePoint:

- un solo `HttpClient` per connessione, con keep-alive gestito dall'handler;
- compressione HTTP GZip/Deflate;
- Form Digest SharePoint SE cacheato fino alla scadenza;
- token OAuth applicativo cacheato fino a due minuti prima della scadenza;
- `$batch` per INSERT/UPDATE/DELETE multipli;
- schema cache process-wide per sito/identità; sul cold path SQL una sola GET mirata recupera lista + RootFolder + Fields, evitando l'enumerazione preventiva dell'intero catalogo; il catalogo completo resta riservato alle API che devono realmente enumerare le liste;
- invalidazione schema tramite `Web.CurrentChangeToken` + `/_api/web/getchanges` filtrato su `List`, `Field`, `ContentType`;
- DDL eseguito dal provider invalida immediatamente la cache interessata;
- RoleDefinition e principal SharePoint risolti una volta e cacheati per connessione;
- nessun round-trip aggiuntivo per la diagnostica;
- con `DiagnosticsEnabled=false` vengono eliminati anche i costi di serializzazione/formattazione del trace diagnostico.

## 31. Struttura della soluzione

```text
GDG.SharePoint.ADOProvider.sln
|
+-- src/
|   +-- GDG.SharePoint.ADOProvider/
|       +-- ADO.NET classes
|       +-- SqlParser / SqlModel
|       +-- RestRuntime
|       +-- SharePointRestTransport
|       +-- DirectRestClient
|
+-- samples/
|   +-- GDG.SharePoint.ADOProvider.Sample/
|   +-- GDG.SharePoint.ADOProvider.DemoData/
|   +-- GDG.SharePoint.ADOProvider.WinFormsTutorial/
|   +-- GDG.SharePoint.ADOProvider.WinFormsMiniErp/   # CRUD Agents/Customers/Orders + attachments
|   +-- GDG.SharePoint.ADOProvider.WinFormsMiniErpClassic/  # Esempio ADO.NET tradizionale
|   +-- GDG.SharePoint.ADOProvider.WinFormsMiniErpDapper/   # Stesso esempio, accesso dati Dapper
|   +-- AdvancedQueries.sql
|
+-- tests/
|   +-- GDG.SharePoint.ADOProvider.Tester/
|   +-- GDG.SharePoint.ADOProvider.Conformance/
|
+-- README.md
+-- CHANGELOG.md
+-- ADO_NET_1.0_CONFORMANCE.md
```

**Esempio gestionale WinForms:** [WinFormsMiniErp](samples/GDG.SharePoint.ADOProvider.WinFormsMiniErp/README.md)
gestisce le tre liste del Tutorial e tutti gli allegati delle singole righe tramite
`SharePointDataAdapter`, `SharePointCommand` e la pseudo-tabella `$ATTACHMENTS(...)`.
È un progetto separato, utilizzabile come startup project su .NET Framework 4.8 o .NET 8 Windows.

**Varianti didattiche:** [MiniERP Classic ADO.NET](samples/GDG.SharePoint.ADOProvider.WinFormsMiniErpClassic/README.md) e [MiniERP Dapper](samples/GDG.SharePoint.ADOProvider.WinFormsMiniErpDapper/README.md). La versione Dapper usa lo stesso provider REST-only con **Model C# tipizzati** (`Agente`, `Cliente`, `Ordine`, `Allegato`), mapping `Query<T>()` delle entita', `BindingList<T>` nelle form e comandi parametrizzati `Execute`/`DynamicParameters`, senza `DataTable`.


## 32. Build

Da Visual Studio 2026 aprire:

```text
GDG.SharePoint.ADOProvider.sln
```

e compilare entrambi i target.

Da command line, con un .NET SDK/MSBuild compatibile installato:

```powershell
./build.ps1
```

## 33. Riferimenti tecnici SharePoint REST

La progettazione REST-only utilizza le API SharePoint REST ufficiali, in particolare:

- REST service e `$batch`;
- `SP.CamlQuery` / `getItems`;
- Lists REST API;
- Fields REST API e `createFieldAsXml`;
- `contextinfo` / form digest per richieste non OAuth;
- `SecurableObject` inheritance, `RoleDefinitions` e `RoleAssignments` per DCL `GRANT`/`REVOKE`.

## 34. Licenza e copyright

Autore: **Gabriele Del Giovine**  
(c) **Gabriele Del Giovine 2026**

Ogni file sorgente della soluzione contiene l'intestazione autore/copyright corrispondente.

### REST round-trip diagnostics

When diagnostics are enabled, the provider distinguishes physical HTTP requests from logical operations embedded in a SharePoint `$batch` payload:

```csharp
int roundTrips = command.LastRestRoundTripCount;
int batchOperations = command.LastRestBatchSubRequestCount;
```

`LastRestRoundTripCount` is incremented only when the transport performs an actual `HttpClient.SendAsync`. A `$batch` containing 100 DELETE sub-requests therefore counts as one HTTP round-trip for the batch request, while `LastRestBatchSubRequestCount` reports 100 logical sub-requests. The tester displays both values and marks batch sub-requests as **no additional round-trip**.

Tester elapsed time is finalized before informational `MessageBox` dialogs are displayed; user interaction time is therefore excluded from execution timing.



### Tester DataAdapter
Il tester espone `Esegui DataAdapter` per `SharePointDataAdapter.Fill()` con la configurazione di login corrente. In questa modalità la griglia risultati è editabile sulle colonne aggiornabili e dispone dei comandi espliciti `Nuovo item` e `Elimina item`: il primo crea una riga in edit mode (`DataRowState.Added` dopo il commit dell'editing), il secondo marca la riga selezionata come `DataRowState.Deleted`. `Salva DataAdapter (CommandBuilder)` esegue `SharePointDataAdapter.Update()` usando `SharePointCommandBuilder` per generare automaticamente `INSERT`, `UPDATE` e `DELETE` in base allo stato delle righe. Le colonne `byte[]`, read-only e `AutoIncrement` non sono editabili; i binari sono visualizzati come `<binary: N bytes>` senza alterare il `DataTable` originale. Le nuove righe ricevono chiavi temporanee negative e, dopo l'INSERT, l'ID restituito da SharePoint viene rimappato nella riga. Una riga aggiunta e poi eliminata prima del salvataggio viene rimossa solo localmente e non produce alcuna DELETE remota. Il salvataggio automatico è abilitato solo quando la SELECT espone una chiave (normalmente `ID`); per `$ATTACHMENTS(...)` il tester consente `Fill`/editing ma richiede comandi DML espliciti e quindi disabilita il pulsante CommandBuilder.

## Windows Forms Test Suites runner

Il progetto `GDG.SharePoint.ADOProvider.Tester` include una finestra **Test suites** che consente di eseguire in modo controllato:

- `GDG.SharePoint.ADOProvider.Conformance`;
- `GDG.SharePoint.ADOProvider.Samples` (runner del progetto sample);
- `GDG.SharePoint.ADOProvider.JoinIntegration`.

La finestra riutilizza le configurazioni **SharePoint Online** e **SharePoint Server SE** presenti nel tester. È possibile selezionare uno o entrambi i target e una o più suite; ogni combinazione viene eseguita separatamente con la relativa connection string reale, mentre nella UI e nei log la connection string viene mostrata con password, certificate password e access token mascherati.

Per `JoinIntegration` è disponibile l'opzione **ricrea fixture (`--recreate`)**. L'opzione è volutamente disattivata per default e richiede una conferma esplicita perché elimina e ricrea le liste `Orders`, `Customers` e `Agents` su tutti i target selezionati.

Il runner mostra per ogni esecuzione target, suite, `PASS/FAIL`, exit code, durata e output dettagliato. L'annullamento è cooperativo tra una suite e la successiva: una richiesta già in corso viene lasciata terminare secondo i timeout configurati del provider.

Il runner `Samples` non presuppone l'esistenza di una lista `Tasks`: dopo `GetSchema("Tables")` seleziona automaticamente la prima lista/document library non nascosta disponibile e usa la colonna universale `ID` per il test parametrizzato e per `DbDataAdapter.Fill()`.

## Relational Engine V2 — GROUP BY (Phase 2)

Phase 2 aggiunge `GROUP BY` nativo al relational engine REST-only:

```sql
SELECT Status, COUNT(*) AS Rows, SUM(Amount) AS Total
FROM Orders
WHERE Amount >= 100
GROUP BY Status
ORDER BY Rows DESC
```

Sono supportate una o più chiavi di raggruppamento, anche in query con JOIN, e gli aggregati `COUNT(*)`, `COUNT(field)`, `SUM`, `AVG`, `MIN` e `MAX`. I valori `NULL` della stessa combinazione di chiavi appartengono allo stesso gruppo. Sono ammesse anche query di sola proiezione raggruppata (`SELECT Status FROM Orders GROUP BY Status`) e costanti insieme alle chiavi.

Ordine logico garantito:

```text
REST scans / safe predicate pushdown
          -> JOIN / semi-anti join
          -> remaining WHERE
          -> LOCAL HASH/STREAM AGGREGATE (CBO)
          -> ORDER BY
          -> TOP / MaxRows
```

L'input della fase di grouping deve essere completo: `TOP` e `MaxRows` non possono troncare le scansioni prima dell'aggregazione. `MaxClientScanRows` resta il limite di sicurezza esplicito; se viene superato la query fallisce invece di restituire gruppi parziali.

`EXPLAIN` espone `GROUP AGGREGATE` come operatore logico e `LOCAL GROUP AGGREGATE` come operatore fisico. In Phase 2 non vengono ancora selezionati `HASH AGGREGATE` o `STREAM AGGREGATE`; tali alternative cost-based sono riservate alla Phase 4. Il pushdown `SERVER AGGREGATE` resta disabilitato finché non viene dimostrata equivalenza semantica live su SharePoint Online e SharePoint Server SE.

`ORDER BY` nelle query raggruppate può riferirsi a una chiave `GROUP BY` oppure al nome/alias di una colonna proiettata. Per ordinare per un aggregato è quindi consigliato assegnargli un alias. `HAVING` non fa parte della Phase 2 ed è previsto per la Phase 3.
