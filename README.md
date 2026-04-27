# Passero Framework

Un **framework MVVM (Model-View-ViewModel)** moderno e completo per **Wisej.NET**, progettato per semplificare lo sviluppo di applicazioni web enterprise con supporto multi-target (.NET Framework 4.8, .NET 8).

## 🚀 Caratteristiche Principali

### Architettura MVVM
- **ViewModel generico** basato su `ViewModel<T>` per gestione dati tipizzata
- **Repository pattern** con supporto Dapper
- **Data binding** automatico con `BindingSource` e modalità Passero proprietaria
- **Change tracking** integrato con supporto undo/redo

### Gestione Dati
- **ORM leggero** basato su Dapper con supporto CRUD completo
- **Query Builder** integrato (QBE - Query By Example)
- **Repository generico** con operazioni async e batch
- Supporto **SQL Server**, **MySQL**, **PostgreSQL**

### UI Controls
- **DataNavigator**: Toolbar avanzata per navigazione/editing dati
  - Pulsanti personalizzabili (First, Last, Next, Previous, New, Delete, Save, Undo, Find, Print, Refresh, Close)
  - Supporto tasti funzione (F2-F12)
  - Integrazione con DataGridView e DataRepeater
  - Modalità Compact e Mini per responsive design

- **DbLookUpTextBox**: Controllo lookup con ricerca intelligente
- **ErrorNotificationMessageBox**: Gestione errori centralizzata
- **QBEForm**: Form di ricerca dinamico basato su model

### Report
- **FastReport Integration** (OpenSource)
  - Esportazione PDF
  - Supporto SQL Server data source
  - Template personalizzabili

- **SSRS Reports** (SQL Server Reporting Services)
  - Integrazione con ReportViewer
  - Gestione parametri dinamici

- **Crystal Reports** (solo .NET Framework)
  - Supporto per Visual Studio 2022

### Business System
- **Framework per gestione sistemi business** complessi
- Supporto per workflow e processi aziendali
- Integrazione con barcode (Code 128, QR Code, etc.)

## 📦 Struttura Progetti

```
PasseroFramework/
│
├── Passero.Framework/                    # Core framework
│   ├── Base.ViewModel.cs                # ViewModel generico
│   ├── Base.Repository.cs               # Repository pattern
│   ├── Base.DbSet.cs                    # Gestione collezioni
│   ├── ConfigurationManager.cs          # Configurazione DB
│   └── ReflectionHelper.cs              # Utility reflection
│
├── Passero.Framework.Controls/          # UI Controls per Wisej
│   ├── DataNavigator.cs                 # Toolbar navigazione
│   ├── DbLookUpTextBox.cs               # Controllo lookup
│   └── ErrorNotificationMessageBox.cs   # Gestione errori
│
├── Passero.Framework.FRReports/         # FastReport support
│   └── FRQBEReport.cs                   # Report con QBE
│
├── Passero.Framework.SSRSReports/       # SSRS support
│   └── ReportManager.cs                 # Gestione SSRS
│
├── Passero.Framework.Barcode/           # Barcode generation
│   └── BarcodeHelper.cs                 # Generazione barcode
│
├── Passero.Framework.BusinessSystem/    # Business system framework
│   └── BusinessSystemCore.cs            # Core business logic
│
├── PasseroDemo.Models/                  # Modelli dati (esempio)
├── PasseroDemo.ViewModels/              # ViewModels (esempio)
├── PasseroDemo.Views/                   # Views Wisej (esempio)
├── PasseroDemo.Repositories/            # Repositories (esempio)
├── PasseroDemo.Reports/                 # Reports (esempio)
└── PasseroDemo.Application/             # Applicazione demo
```

## 🎯 Quick Start

### 1. Installazione Pacchetti NuGet

```bash
dotnet add package Passero.Framework
dotnet add package Passero.Framework.Controls
dotnet add package Passero.Framework.FRReports  # Opzionale per FastReport
```

### 2. Configurazione Database

```csharp
// Configurare connection string in ConfigurationManager
var config = new Passero.Framework.ConfigurationManager();
config.DBConnections.Add("MyApp", myDbConnection);
```

### 3. Creare un Model

```csharp
[Table("publishers")]
public class Publisher
{
    [Key]
    [Column("pub_id")]
    public string PubId { get; set; }
    
    [Column("pub_name")]
    public string PubName { get; set; }
    
    [Column("city")]
    public string City { get; set; }
}
```

### 4. Creare un ViewModel

```csharp
public class vmPublisher : ViewModel<Publisher>
{
    // Logica business personalizzata (opzionale)
    public void CustomMethod()
    {
        // ...
    }
}
```

### 5. Utilizzare nella View (Wisej Form)

```csharp
public partial class frmPublishers : Form
{
    private vmPublisher vmPublisher = new vmPublisher();
    
    public void Init()
    {
        // Inizializza ViewModel
        vmPublisher.Init(dbConnection);
        vmPublisher.DataBindingMode = DataBindingMode.BindingSource;
        vmPublisher.BindingSource = this.bsPublishers;
        vmPublisher.AutoWriteControls = true;
        vmPublisher.AutoReadControls = true;
        
        // Carica dati
        vmPublisher.GetAllItems();
        
        // Configura DataNavigator
        dataNavigator1.AddViewModel(vmPublisher, "Publishers");
        dataNavigator1.SetActiveViewModel(vmPublisher);
        
        // Bind a DataGridView
        dgvPublishers.DataSource = vmPublisher.ModelItems;
    }
}
```

## 🔧 Funzionalità Avanzate

### Data Binding Automatico

```csharp
// Crea binding automatico dai controlli con naming convention
vmPublisher.CreatePasseroBindingFromBindingSource(this);

// I controlli con nomi come txt_<PropertyName> vengono automaticamente bindati
```

### Operazioni CRUD

```csharp
// Inserimento
var newItem = new Publisher { PubId = "9999", PubName = "New Publisher" };
var result = vmPublisher.InsertItem(newItem);

// Aggiornamento
vmPublisher.ModelItem.PubName = "Updated Name";
result = vmPublisher.UpdateItem();

// Cancellazione
result = vmPublisher.DeleteItem(vmPublisher.ModelItem);

// Query personalizzate
var items = vmPublisher.Repository.Query("SELECT * FROM publishers WHERE city = @City", 
    new { City = "Boston" });
```

### QBE (Query By Example)

```csharp
// Form di ricerca dinamico basato su model
QBEForm<Publisher> qbeForm = new QBEForm<Publisher>(dbConnection);
qbeForm.SetTargetRepository(vmPublisher.Repository, () => RefreshData());
qbeForm.ShowQBE();
```

### DataNavigator Events

```csharp
// Eventi del ciclo di vita
dataNavigator1.eAddNewRequest += (ref bool cancel) => {
    // Validazione prima dell'inserimento
    if (!IsValid()) cancel = true;
};

dataNavigator1.eSaveRequest += (ref bool cancel) => {
    // Conferma salvataggio
};

dataNavigator1.eDeleteRequest += (ref bool cancel) => {
    // Conferma eliminazione
};
```

### Gestione Barcode

```csharp
// Genera barcode Code 128
var barcodeImage = BarcodeHelper.GenerateCode128("123456789");

// Genera QR Code
var qrCode = BarcodeHelper.GenerateQRCode("https://example.com");
```

## 📚 Target Framework

- **.NET Framework 4.7** - Per compatibilità legacy
- **.NET Framework 4.8** - Per applicazioni Windows enterprise
- **.NET 8** - Per applicazioni moderne cross-platform
- **.NET 8 Windows** - Per applicazioni Windows con UI moderna
- **Wisej.NET 3.5.25** - Framework web UI

## 🔗 Dipendenze Principali

- **Dapper** 2.1.66 - Micro ORM
- **Dapper.Contrib** 2.0.78 - Estensioni CRUD
- **Microsoft.Data.SqlClient** 6.1.3 - SQL Server provider
- **Newtonsoft.Json** 13.0.4 - Serializzazione JSON
- **FastReport.OpenSource** 2026.1.2 - Report engine
- **Wisej-3** 3.5.25 - Web UI framework
- **MiniExcel** 1.42.0 - Export Excel leggero
- **FastDeepCloner** 1.3.6 - Deep cloning

## 🛠️ Build e Deploy

```bash
# Build soluzione
dotnet build -c Release

# Genera pacchetti NuGet (già configurati in .csproj)
dotnet pack -c Release

# I pacchetti .nupkg saranno in bin/Release/
```

### Pacchetti NuGet Disponibili

- **Passero.Framework** - Core framework
- **Passero.Framework.Controls** - UI controls
- **Passero.Framework.FRReports** - FastReport integration
- **Passero.Framework.SSRSReports** - SSRS integration
- **Passero.Framework.Barcode** - Barcode generation
- **Passero.Framework.BusinessSystem** - Business system framework

## 📖 Documentazione

### Esempi Completi

Il progetto include una demo completa (`PasseroDemo`) con esempi di:
- Form CRUD (Publishers, Titles, Sales, etc.)
- Relazioni master-detail (Title-Authors)
- Lookup avanzati con DbLookUpTextBox
- Report FastReport e SSRS
- Query By Example (QBE)
- Navigazione dati con DataNavigator

### Naming Convention

Per il data binding automatico, seguire queste convenzioni:
- **TextBox/Label**: `txt_<PropertyName>`, `lbl_<PropertyName>`
- **CheckBox**: `chk_<PropertyName>`
- **ComboBox**: `cmb_<PropertyName>`
- **DateTimePicker**: `dtp_<PropertyName>`

Esempio:
```csharp
// Il controllo txt_pub_name verrà automaticamente bindato alla proprietà PubName
private TextBox txt_pub_name;
```

## 🎥 Video Tutorial

- [Introduzione a Passero Framework](https://youtu.be/ZTJJvuwr0lU?si=Q2oZpCtU7kEaoZOy)
- [Data Binding e MVVM Pattern](https://youtu.be/ThMz_N9xwkg?si=DXhSPcttzdM5SwYe)
- [Repository Pattern e Dapper](https://youtu.be/SSgEvU6jZ9w?si=yjoKycPz-fLAXz5W)

## 📄 Licenza

**MIT License** - Vedi file LICENSE per dettagli

Copyright (c) 2024 Gabriele Del Giovine

## 👤 Autore

**Gabriele Del Giovine**  
- GitHub: [github.com/gdelgiovine/PasseroFramework](https://github.com/gdelgiovine/PasseroFramework)
- YouTube: Tutorial disponibili sul canale

## 🤝 Contribuire

I contributi sono benvenuti! Per favore:

1. Fai un fork del progetto
2. Crea un branch per la tua feature (`git checkout -b feature/AmazingFeature`)
3. Commit delle modifiche (`git commit -m 'Add some AmazingFeature'`)
4. Push al branch (`git push origin feature/AmazingFeature`)
5. Apri una Pull Request

### Linee Guida per Contribuire

- Segui le convenzioni di codice esistenti
- Aggiungi test per nuove funzionalità
- Aggiorna la documentazione
- Mantieni la compatibilità con tutti i target framework

## 🐛 Bug Report e Feature Request

Per bug report, feature request o domande:
- **Issues**: [GitHub Issues](https://github.com/gdelgiovine/PasseroFramework/issues)
- **Discussions**: [GitHub Discussions](https://github.com/gdelgiovine/PasseroFramework/discussions)

## 🌟 Caratteristiche in Arrivo

- [ ] Supporto per Entity Framework Core
- [ ] GraphQL integration
- [ ] SignalR per real-time updates
- [ ] PWA (Progressive Web App) support con Wisej
- [ ] Docker containers per deploy
- [ ] Autenticazione OAuth2/OpenID Connect
- [ ] Audit trail automatico
- [ ] Multi-tenancy support

## 📊 Statistiche Progetto

- **Progetti**: 26 progetti nella soluzione
- **Target Frameworks**: .NET 4.7, 4.8, 8.0
- **Linee di Codice**: 50,000+ LOC
- **Versione Corrente**: 3.5.25
- **Stato**: Attivamente mantenuto

## 🏆 Casi d'Uso

Passero Framework è stato utilizzato con successo in:
- **ERP** - Gestione aziendale completa
- **CRM** - Customer Relationship Management
- **WMS** - Warehouse Management Systems
- **Applicazioni Healthcare** - Gestione cliniche e ospedali
- **Retail** - Punto vendita e gestione magazzino

## 💡 Best Practices

### Performance
- Usa `Repository.QueryAsync` per operazioni asincrone
- Implementa paginazione per liste grandi
- Utilizza `DbSet` per operazioni batch

### Sicurezza
- Valida sempre l'input utente
- Usa parametri per query SQL (Dapper lo fa automaticamente)
- Implementa logging per audit trail

### Manutenibilità
- Separa logica business nei ViewModels
- Usa Repository personalizzati per query complesse
- Documenta metodi pubblici con XML comments

## 🔄 Versioning

Questo progetto segue [Semantic Versioning](https://semver.org/):
- **MAJOR**: Breaking changes
- **MINOR**: Nuove funzionalità retrocompatibili
- **PATCH**: Bug fixes

## 🙏 Ringraziamenti

Un ringraziamento speciale a:
- **Ice Tea Group** - Per Wisej.NET framework
- **Dapper** team - Per l'eccellente micro ORM
- **FastReport** team - Per il report engine
- Tutti i contributors e utenti del framework

---

**Passero Framework** - Sviluppo rapido di applicazioni Wisej.NET con pattern MVVM moderno

*Made with ❤️ in Italy*
