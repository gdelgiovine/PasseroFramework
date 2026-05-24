using System;
using System.Collections.Generic;

namespace Passero.Framework.Controls
{
    /// <summary>
    /// Rappresenta un singolo Application Identifier GS1 estratto dal barcode.
    /// </summary>
    public class Gs1ApplicationIdentifier
    {
        /// <summary>Codice AI (es. "01", "10", "17").</summary>
        public string AI { get; set; }

        /// <summary>Etichetta leggibile dell'AI (es. "GTIN", "BATCH/LOT", "USE BY OR EXPIRY").</summary>
        public string Label { get; set; }

        /// <summary>Valore associato all'AI.</summary>
        public string Value { get; set; }

        public override string ToString() => $"({AI}) {Label}: {Value}";
    }

    /// <summary>
    /// Parser GS1 Application Identifiers.
    /// Porta in C# la logica delle costanti s (label map), a (fixed-length map)
    /// e della classe n (parser) presenti in webserialbarcode.js.
    /// </summary>
    public static class Gs1AiParser
    {
        /// <summary>Carattere separatore FNC1 (ASCII GS = 0x1D).</summary>
        private const char Fnc1Separator = (char)29;

        // ── Mappa AI → lunghezza fissa (incluso il prefisso AI) ─────────────────
        // Traduzione della costante JavaScript 'a'
        private static readonly Dictionary<string, int> FixedLengthAis =
            new Dictionary<string, int>
            {
                { "00", 20 }, { "01", 16 }, { "02", 16 }, { "03", 16 }, { "04", 18 },
                { "11",  8 }, { "12",  8 }, { "13",  8 }, { "14",  8 }, { "15",  8 },
                { "16",  8 }, { "17",  8 }, { "18",  8 }, { "19",  8 },
                { "20",  4 },
                { "31", 10 }, { "32", 10 }, { "33", 10 }, { "34", 10 },
                { "35", 10 }, { "36", 10 },
                { "41", 16 }
            };

        // ── Mappa AI → etichetta leggibile ───────────────────────────────────────
        // Traduzione della costante JavaScript 's'
        private static readonly Dictionary<string, string> AiLabels =
            new Dictionary<string, string>
            {
                { "00", "SSCC" },
                { "01", "GTIN" },
                { "02", "CONTENT" },
                { "10", "BATCH/LOT" },
                { "11", "PROD DATE" },
                { "12", "DUE DATE" },
                { "13", "PACK DATE" },
                { "15", "BEST BEFORE or BEST BY" },
                { "16", "SELL BY" },
                { "17", "USE BY OR EXPIRY" },
                { "20", "VARIANT" },
                { "21", "SERIAL" },
                { "22", "CPV" },
                { "30", "VAR. COUNT" },
                { "37", "COUNT" },
                { "90", "INTERNAL" }, { "91", "INTERNAL" }, { "92", "INTERNAL" },
                { "93", "INTERNAL" }, { "94", "INTERNAL" }, { "95", "INTERNAL" },
                { "96", "INTERNAL" }, { "97", "INTERNAL" }, { "98", "INTERNAL" },
                { "99", "INTERNAL" },
                { "240", "ADDITIONAL ID" },
                { "241", "CUST. PART NO." },
                { "242", "MTO VARIANT" },
                { "243", "PCN" },
                { "250", "SECONDARY SERIAL" },
                { "251", "REF. TO SOURCE" },
                { "253", "GDTI" },
                { "254", "GLN EXTENSION COMPONENT" },
                { "255", "GCN" },
                { "400", "ORDER NUMBER" },
                { "401", "GINC" },
                { "402", "GSIN" },
                { "403", "ROUTE" },
                { "410", "SHIP TO LOC" },
                { "411", "BILL TO" },
                { "412", "PURCHASE FROM" },
                { "413", "SHIP FOR LOC" },
                { "414", "LOC No" },
                { "415", "PAY TO" },
                { "416", "PROD/SERV LOC" },
                { "420", "SHIP TO POST" },
                { "421", "SHIP TO POST" },
                { "422", "ORIGIN" },
                { "423", "COUNTRY - INITIAL PROCESS." },
                { "424", "COUNTRY - PROCESS." },
                { "425", "COUNTRY - DISASSEMBLY" },
                { "426", "COUNTRY - FULL PROCESS" },
                { "427", "ORIGIN SUBDIVISION" },
                { "710", "NHRN PZN" },
                { "711", "NHRN CIP" },
                { "712", "NHRN CN" },
                { "713", "NHRN DRN" },
                { "714", "NHRN AIM" },
                { "7001", "NSN" },
                { "7002", "MEAT CUT" },
                { "7003", "EXPIRY TIME" },
                { "7004", "ACTIVE POTENCY" },
                { "7005", "CATCH AREA" },
                { "7006", "FIRST FREEZE DATE" },
                { "7007", "HARVEST DATE" },
                { "7008", "AQUATIC SPECIES" },
                { "7009", "FISHING GEAR TYPE" },
                { "7010", "PROD METHOD" },
                { "7020", "REFURB LOT" },
                { "7021", "FUNC STAT" },
                { "7022", "REV STAT" },
                { "7023", "GIAI - ASSEMBLY" },
                { "7030", "PROCESSOR # 0" }, { "7031", "PROCESSOR # 1" },
                { "7032", "PROCESSOR # 2" }, { "7033", "PROCESSOR # 3" },
                { "7034", "PROCESSOR # 4" }, { "7035", "PROCESSOR # 5" },
                { "7036", "PROCESSOR # 6" }, { "7037", "PROCESSOR # 7" },
                { "7038", "PROCESSOR # 8" }, { "7039", "PROCESSOR # 9" },
                { "7230", "CERT # 0" }, { "7231", "CERT # 1" }, { "7232", "CERT # 2" },
                { "7233", "CERT # 3" }, { "7234", "CERT # 4" }, { "7235", "CERT # 5" },
                { "7236", "CERT # 6" }, { "7237", "CERT # 7" }, { "7238", "CERT # 8" },
                { "7239", "CERT # 9" },
                { "8001", "DIMENSIONS" },
                { "8002", "CMT No" },
                { "8003", "GRAI" },
                { "8004", "GIAI" },
                { "8005", "PRICE PER UNIT" },
                { "8006", "ITIP" },
                { "8007", "IBAN" },
                { "8008", "PROD TIME" },
                { "8009", "OPT SEN" },
                { "8010", "CPID" },
                { "8011", "CPID SERIAL" },
                { "8012", "VERSION" },
                { "8013", "GMN" },
                { "8017", "GSRN - PROVIDER" },
                { "8018", "GSRN - RECIPIENT" },
                { "8019", "SRIN" },
                { "8020", "REF No" },
                { "8026", "ITIP CONTENT" },
                { "8111", "POINTS" },
                { "8200", "PRODUCT URL" },
                { "3100", "NET WEIGHT (kg)" }, { "3101", "NET WEIGHT (kg)" }, { "3102", "NET WEIGHT (kg)" },
                { "3103", "NET WEIGHT (kg)" }, { "3104", "NET WEIGHT (kg)" }, { "3105", "NET WEIGHT (kg)" },
                { "3200", "NET WEIGHT (lb)" }, { "3201", "NET WEIGHT (lb)" }, { "3202", "NET WEIGHT (lb)" },
                { "3203", "NET WEIGHT (lb)" }, { "3204", "NET WEIGHT (lb)" }, { "3205", "NET WEIGHT (lb)" },
                { "3300", "GROSS WEIGHT (kg)" }, { "3301", "GROSS WEIGHT (kg)" }, { "3302", "GROSS WEIGHT (kg)" },
                { "3303", "GROSS WEIGHT (kg)" }, { "3304", "GROSS WEIGHT (kg)" }, { "3305", "GROSS WEIGHT (kg)" },
                { "3900", "AMOUNT" }, { "3901", "AMOUNT" }, { "3902", "AMOUNT" }, { "3903", "AMOUNT" },
                { "3904", "AMOUNT" }, { "3905", "AMOUNT" }, { "3906", "AMOUNT" }, { "3907", "AMOUNT" },
                { "3908", "AMOUNT" }, { "3909", "AMOUNT" },
                { "3920", "PRICE" }, { "3921", "PRICE" }, { "3922", "PRICE" }, { "3923", "PRICE" },
                { "3924", "PRICE" }, { "3925", "PRICE" }, { "3926", "PRICE" }, { "3927", "PRICE" },
                { "3928", "PRICE" }, { "3929", "PRICE" },
                { "3940", "PRCNT OFF" }, { "3941", "PRCNT OFF" }, { "3942", "PRCNT OFF" }, { "3943", "PRCNT OFF" },
            };

        /// <summary>
        /// Restituisce l'etichetta dell'AI o null se non trovata.
        /// </summary>
        public static string GetLabel(string ai) =>
            AiLabels.TryGetValue(ai, out var label) ? label : null;

        /// <summary>
        /// Parsa una stringa GS1 in qualsiasi formato supportato ed estrae
        /// la lista di Application Identifiers.
        ///
        /// Formati accettati:
        ///   • URL GS1 Digital Link (QR Code):  http[s]://dominio/AI1/Val1/AI2/Val2/...
        ///   • Human-readable con parentesi:     (01)05412345000013(17)250101(10)ABC123
        ///   • Raw con separatore GS (0x1D):     \x1D015412345000013\x1D17250101
        ///   • Raw con 'é' come separatore:      é015412345000013é17250101
        ///   • Misto dei precedenti
        /// </summary>
        public static List<Gs1ApplicationIdentifier> ParseFromString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new List<Gs1ApplicationIdentifier>();

            // 1) Rileva formato URL GS1 Digital Link (QR Code) — PRIMA di qualsiasi normalizzazione
            if (input.StartsWith("http://", System.StringComparison.OrdinalIgnoreCase) ||
                input.StartsWith("https://", System.StringComparison.OrdinalIgnoreCase))
                return ParseFromUrl(input);

            // 2) Normalizza separatori alternativi → GS (ASCII 29)
            string normalized = input.Replace('é', Fnc1Separator);
            normalized = normalized.Replace(@"\u001d", Fnc1Separator.ToString());
            normalized = normalized.Replace(@"\u001D", Fnc1Separator.ToString());
            normalized = normalized.Replace(@"\x1d", Fnc1Separator.ToString());
            normalized = normalized.Replace(@"\x1D", Fnc1Separator.ToString());

            // 3) Rileva se il formato usa le parentesi: (AI)value
            if (normalized.IndexOf('(') >= 0)
                normalized = ConvertBracketFormatToRaw(normalized);

            // 4) Rimuove il primo FNC1 (identifica il simbolo come GS1 DataMatrix)
            if (normalized.Length > 0 && normalized[0] == Fnc1Separator)
                normalized = normalized.Substring(1);

            return Parse(normalized);
        }

        /// <summary>
        /// Converte il formato con parentesi <c>(AI)value(AI)value...</c>
        /// nel formato raw con separatori GS tra campi a lunghezza variabile.
        /// </summary>
        private static string ConvertBracketFormatToRaw(string input)
        {
            var sb = new System.Text.StringBuilder();
            int i = 0;

            while (i < input.Length)
            {
                if (input[i] != '(') { i++; continue; }

                int closeIdx = input.IndexOf(')', i);
                if (closeIdx < 0) break;

                string aiCode = input.Substring(i + 1, closeIdx - i - 1);
                int valueStart = closeIdx + 1;
                int nextOpen = input.IndexOf('(', valueStart);

                string aiValue = nextOpen < 0
                    ? input.Substring(valueStart)
                    : input.Substring(valueStart, nextOpen - valueStart);

                bool isFixed = GetFixedKey(aiCode) != null;

                sb.Append(aiCode);
                sb.Append(aiValue);

                // Aggiunge FNC1 solo se il campo è variabile e non è l'ultimo
                if (!isFixed && nextOpen >= 0)
                    sb.Append(Fnc1Separator);

                i = nextOpen >= 0 ? nextOpen : input.Length;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Parsa una stringa GS1 DataMatrix (senza il FNC1 iniziale) ed estrae
        /// la lista di Application Identifiers.
        ///
        /// Regole GS1 DataMatrix applicate:
        ///   • Il FNC1 separa i campi a lunghezza variabile che non sono gli ultimi codificati.
        ///   • Se un AI variabile è l'ultimo campo del segmento, non serve separatore:
        ///     il valore è tutto il testo rimanente nel segmento.
        ///   • Gli AI a lunghezza fissa non necessitano mai di separatore.
        /// </summary>
        public static List<Gs1ApplicationIdentifier> Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return new List<Gs1ApplicationIdentifier>();

            var result = new List<Gs1ApplicationIdentifier>();

            // Ogni segmento è delimitato da FNC1.
            // Per lo standard GS1 DataMatrix un campo variabile termina con FNC1
            // quando non è l'ultimo; quindi ogni segmento contiene:
            //   - zero o più AI a lunghezza fissa (uno dopo l'altro)
            //   - al più un AI variabile, sempre come ultimo elemento del segmento
            foreach (var segment in value.Split(Fnc1Separator))
            {
                if (segment.Length == 0) continue;
                ParseSegment(segment, result);
            }

            return result;
        }

        // ── Metodi privati di supporto ───────────────────────────────────────────

        /// <summary>
        /// Parsa un singolo segmento (porzione tra due FNC1) e aggiunge
        /// gli AI trovati a <paramref name="result"/>.
        /// </summary>
        private static void ParseSegment(string segment, List<Gs1ApplicationIdentifier> result)
        {
            string remaining = segment;

            while (remaining.Length > 0)
            {
                // Identifica l'AI corrente (da 2 a 4 cifre, match più corto per primo)
                string aiCode = ResolveAiCode(remaining);

                if (aiCode == null)
                {
                    result.Add(new Gs1ApplicationIdentifier
                    {
                        AI = null,
                        Label = "UNKNOWN",
                        Value = remaining
                    });
                    return;
                }

                string afterAi = remaining.Substring(aiCode.Length);
                string fixedKey = GetFixedKey(aiCode);

                if (fixedKey != null)
                {
                    // AI a lunghezza FISSA: consuma esattamente (fixedLen - aiCode.Length) caratteri.
                    int valueLen = FixedLengthAis[fixedKey] - aiCode.Length;
                    int take = Math.Min(valueLen, afterAi.Length);

                    result.Add(new Gs1ApplicationIdentifier
                    {
                        AI = aiCode,
                        Label = GetLabel(aiCode),
                        Value = afterAi.Substring(0, take)
                    });

                    remaining = afterAi.Substring(take);
                }
                else
                {
                    // AI a lunghezza VARIABILE: per lo standard GS1 DataMatrix,
                    // il FNC1 è già stato usato come delimitatore del segmento.
                    // Questo AI è quindi l'ultimo nel segmento → consuma tutto il residuo.
                    result.Add(new Gs1ApplicationIdentifier
                    {
                        AI = aiCode,
                        Label = GetLabel(aiCode),
                        Value = afterAi
                    });

                    return; // Fine segmento
                }
            }
        }

        /// <summary>
        /// Cerca il codice AI all'inizio di <paramref name="s"/> (da 2 a 4 cifre).
        /// Restituisce il codice più corto trovato nella mappa, o <c>null</c>.
        /// </summary>
        private static string ResolveAiCode(string s)
        {
            for (int len = 2; len <= 4 && len <= s.Length; len++)
            {
                string candidate = s.Substring(0, len);
                if (AiLabels.ContainsKey(candidate))
                    return candidate;
            }
            return null;
        }

        /// <summary>
        /// Restituisce la chiave nella mappa <see cref="FixedLengthAis"/> per il dato
        /// <paramref name="aiCode"/>, considerando anche il prefisso a 2 cifre
        /// (es. "3100" → chiave "31"). Restituisce <c>null</c> se l'AI è variabile.
        /// </summary>
        private static string GetFixedKey(string aiCode)
        {
            if (FixedLengthAis.ContainsKey(aiCode))
                return aiCode;

            if (aiCode.Length >= 2 && FixedLengthAis.ContainsKey(aiCode.Substring(0, 2)))
                return aiCode.Substring(0, 2);

            return null;
        }

        /// <summary>
        /// Parsa un QR code in formato URL GS1 Digital Link e ne estrae
        /// la lista di Application Identifiers.
        ///
        /// Formato accettato (GS1 Digital Link):
        ///   http[s]://dominio/AI1/Val1/AI2/Val2[?AIx=Valx&AIy=Valy]
        ///
        /// Esempi:
        ///   https://id.miaazienda.it/01/08012345678901/10/LOTTO2026/21/SN000123?17=260630
        ///   https://example.com/01/04150753612930/17/250101/10/ABC123
        /// </summary>
        public static List<Gs1ApplicationIdentifier> ParseFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return new List<Gs1ApplicationIdentifier>();

            // Separa query string dal resto dell'URL
            string queryString = null;
            int queryStart = url.IndexOf('?');
            if (queryStart >= 0)
            {
                queryString = url.Substring(queryStart + 1);
                url = url.Substring(0, queryStart);
            }

            // Rimuove lo schema (http:// o https://)
            int schemaEnd = url.IndexOf("://", System.StringComparison.OrdinalIgnoreCase);
            if (schemaEnd < 0)
                return new List<Gs1ApplicationIdentifier>();

            string afterSchema = url.Substring(schemaEnd + 3);

            // Salta il dominio (host[:porta]) fino al primo '/'
            int firstSlash = afterSchema.IndexOf('/');
            if (firstSlash < 0)
                return new List<Gs1ApplicationIdentifier>();

            string path = afterSchema.Substring(firstSlash + 1);

            var result = new List<Gs1ApplicationIdentifier>();

            // ── 1) Parsa le coppie AI/Valore nel path ───────────────────────────
            if (!string.IsNullOrEmpty(path))
            {
                string[] segments = path.Split('/');

                for (int i = 0; i + 1 < segments.Length; i += 2)
                {
                    string aiCode = segments[i].Trim();
                    string aiValue = segments[i + 1].Trim();

                    if (string.IsNullOrEmpty(aiCode))
                        continue;

                    try { aiValue = Uri.UnescapeDataString(aiValue); }
                    catch { /* valore non valido, usato così com'è */ }

                    result.Add(new Gs1ApplicationIdentifier
                    {
                        AI = aiCode,
                        Label = GetLabel(aiCode),
                        Value = aiValue
                    });
                }
            }

            // ── 2) Parsa i parametri AI nella query string (?AI=Valore&AI=Valore) ─
            if (!string.IsNullOrEmpty(queryString))
            {
                foreach (string param in queryString.Split('&'))
                {
                    if (string.IsNullOrEmpty(param))
                        continue;

                    int eqIdx = param.IndexOf('=');
                    if (eqIdx <= 0)
                        continue;

                    string aiCode = param.Substring(0, eqIdx).Trim();
                    string aiValue = param.Substring(eqIdx + 1).Trim();

                    if (string.IsNullOrEmpty(aiCode))
                        continue;

                    try { aiValue = Uri.UnescapeDataString(aiValue); }
                    catch { /* valore non valido, usato così com'è */ }

                    result.Add(new Gs1ApplicationIdentifier
                    {
                        AI = aiCode,
                        Label = GetLabel(aiCode),
                        Value = aiValue
                    });
                }
            }

            return result;
        }
    }
}