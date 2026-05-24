using System;
using System.Globalization;
using System.Resources;

namespace Passero.Framework.Controls;

/// <summary>
/// Fornisce la localizzazione delle etichette del QueryBuilder.
/// Per sostituire le stringhe di default è sufficiente impostare
/// <see cref="CustomResolver"/> all'avvio dell'applicazione.
/// </summary>
public static class QueryBuilderLocalizer
{
    private static readonly ResourceManager _defaultResources =
        new ResourceManager(
            "Passero.Framework.Controls.QueryBuilder.QueryBuilderOperatorResources",
            typeof(QueryBuilderLocalizer).Assembly);

    /// <summary>
    /// Resolver personalizzato. Se impostato, viene interrogato per primo.
    /// Restituire <c>null</c> per ricadere sul valore di default.
    /// </summary>
    /// <example>
    /// QueryBuilderLocalizer.CustomResolver = (key, culture) =>
    ///     MyAppResources.ResourceManager.GetString(key, culture);
    /// </example>
    public static Func<string, CultureInfo, string> CustomResolver { get; set; }

    /// <summary>
    /// Restituisce la stringa localizzata per la chiave operatore indicata
    /// usando la cultura corrente dell'UI.
    /// </summary>
    public static string GetOperatorText(string key)
    {
        return GetOperatorText(key, CultureInfo.CurrentUICulture);
    }

    /// <summary>
    /// Restituisce la stringa localizzata per la chiave operatore indicata
    /// usando la cultura specificata.
    /// </summary>
    public static string GetOperatorText(string key, CultureInfo culture)
    {
        if (string.IsNullOrWhiteSpace(key))
            return string.Empty;

        if (CustomResolver != null)
        {
            var custom = CustomResolver(key, culture);
            if (!string.IsNullOrEmpty(custom))
                return custom;
        }

        try
        {
            var value = _defaultResources.GetString(key, culture);
            if (!string.IsNullOrEmpty(value))
                return value;
        }
        catch (MissingManifestResourceException)
        {
            // fallback alla chiave
        }

        return key;
    }
}
