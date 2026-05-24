using System;
using System.Collections.Generic;

namespace Passero .Framework.Controls
{
    /// <summary>
    /// Risultato della decodifica AIM-ID di un barcode seriale,
    /// comprensivo di Application Identifiers GS1.
    /// </summary>
    /// Risultato della decodifica AIM-ID di un barcode seriale,
    /// comprensivo di Application Identifiers GS1.
    /// </summary>
    public class BarcodeDecodeResult
    {
        /// <summary>Nome della simbologia rilevata (es. "ean13", "qr-code", "code128").</summary>
        public string Symbology { get; set; }

        /// <summary>Prefisso AIM originale (es. "]E0", "]C1").</summary>
        public string AimPrefix { get; set; }

        /// <summary>Valore del barcode senza il prefisso AIM.</summary>
        public string Value { get; set; }

        /// <summary>Indica se il barcode contiene dati GS1 con FNC1.</summary>
        public int? Fnc1 { get; set; }

        /// <summary>Modello QR Code (1 o 2), valorizzato solo per qr-code.</summary>
        public int? Model { get; set; }

        /// <summary>
        /// Lista degli Application Identifiers GS1 estratti dal valore.
        /// Valorizzata quando la simbologia è GS1 (gs1-128, gs1-databar-*, data-matrix, ecc.)
        /// oppure quando il valore contiene separatori FNC1.
        /// </summary>
        public List<Gs1ApplicationIdentifier> ApplicationIdentifiers { get; set; }
            = new List<Gs1ApplicationIdentifier>();

        /// <summary>True se il prefisso AIM è stato trovato e riconosciuto.</summary>
        public bool IsAimDecoded => AimPrefix != null;

        /// <summary>True se sono stati estratti Application Identifiers.</summary>
        public bool HasApplicationIdentifiers => ApplicationIdentifiers != null
                                                 && ApplicationIdentifiers.Count > 0;

        /// <summary>
        /// Restituisce il valore del primo AI corrispondente al codice cercato,
        /// oppure null se non trovato.
        /// </summary>
        public string GetAiValue(string aiCode)
        {
            foreach (var ai in ApplicationIdentifiers)
                if (ai.AI == aiCode) return ai.Value;
            return null;
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append($"[{AimPrefix}] Symbology={Symbology ?? "unknown"}, Value={Value}");
            if (Fnc1.HasValue) sb.Append($", FNC1={Fnc1}");
            if (Model.HasValue) sb.Append($", Model={Model}");
            if (HasApplicationIdentifiers)
            {
                sb.AppendLine();
                foreach (var ai in ApplicationIdentifiers)
                    sb.AppendLine($"  {ai}");
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// Decodifica la simbologia di un barcode ricevuto da scanner seriale
    /// tramite il prefisso AIM-ID e parsa gli Application Identifiers GS1.
    /// </summary>
    public static class AimBarcodeDecoder
    {
        private const char AimStartChar = ']';
        private const char Fnc1Char = (char)29;  // GS / ASCII 29

        // Simbologie che trasportano Application Identifiers GS1
        private static readonly HashSet<string> Gs1Symbologies = new HashSet<string>
        {
            "gs1-128",
            "gs1-databar-omni",
            "gs1-databar-limited",
            "gs1-databar-expanded",
            "data-matrix",
            "qr-code",
            "aztec-code",
            "itf"
        };

        /// <summary>
        /// Decodifica il barcode grezzo ricevuto dallo scanner:
        /// 1) Normalizza separatori alternativi FNC1 (es. 'é' → GS ASCII 29)
        /// 2) Interpreta il prefisso AIM-ID (simbologia, Fnc1, Model)
        /// 3) Parsa gli Application Identifiers GS1 quando applicabile
        /// </summary>
        public static BarcodeDecodeResult Decode(string rawValue)
        {
            if (string.IsNullOrEmpty(rawValue))
                throw new ArgumentException("Il valore del barcode non può essere nullo o vuoto.", nameof(rawValue));

            rawValue = rawValue.TrimEnd('\r', '\n');

            // Normalizza separatori FNC1 alternativi → GS (ASCII 29)
            // Alcuni scanner inviano 'é' (0xE9) oppure '}' al posto di GS
            rawValue = rawValue.Replace('é', Fnc1Char);

            // Normalizza sequenze escape letterali del GS → GS (ASCII 29)
            rawValue = rawValue.Replace(@"\u001d", Fnc1Char.ToString());
            rawValue = rawValue.Replace(@"\u001D", Fnc1Char.ToString());
            rawValue = rawValue.Replace(@"\x1d", Fnc1Char.ToString());
            rawValue = rawValue.Replace(@"\x1D", Fnc1Char.ToString());

            // rawValue viene conservato integro (con il FNC1 iniziale se presente)
            // per essere passato come dato grezzo all'evento.
            // Il parsing lavora su una copia senza il leading FNC1.
            string normalizedValue = (rawValue.Length > 0 && rawValue[0] == Fnc1Char)
                ? rawValue.Substring(1)
                : rawValue;

            BarcodeDecodeResult result;

            if (normalizedValue.Length >= 3 && normalizedValue[0] == AimStartChar)
            {
                string aimPrefix = normalizedValue.Substring(0, 3);
                string barcodeValue = normalizedValue.Substring(3);
                result = DecodeAim(aimPrefix, barcodeValue);
            }
            else
            {
                result = new BarcodeDecodeResult
                {
                    AimPrefix = null,
                    Symbology = null,
                    Value = normalizedValue
                };
            }

            // Parsa gli AI GS1 quando:
            // - la simbologia è GS1 oppure ha Fnc1 impostato
            // - oppure il valore contiene il separatore FNC1 (ASCII 29)
            // - oppure il valore è una URL GS1 Digital Link (QR Code)
            bool isGs1Symbology = result.Symbology != null
                                  && Gs1Symbologies.Contains(result.Symbology);
            bool hasFnc1 = result.Fnc1.HasValue;
            bool hasFnc1Char = result.Value != null
                                  && result.Value.IndexOf(Fnc1Char) >= 0;
            bool isDigitalLink = result.Value != null
                                  && (result.Value.StartsWith("http://", System.StringComparison.OrdinalIgnoreCase)
                                   || result.Value.StartsWith("https://", System.StringComparison.OrdinalIgnoreCase));

            if (isGs1Symbology || hasFnc1 || hasFnc1Char || isDigitalLink)
            {
                result.ApplicationIdentifiers = Gs1AiParser.ParseFromString(result.Value);
            }

            return result;
        }

        private static BarcodeDecodeResult DecodeAim(string aimPrefix, string barcodeValue)
        {
            var result = new BarcodeDecodeResult
            {
                AimPrefix = aimPrefix,
                Value = barcodeValue,
                Symbology = null
            };

            char symbChar = aimPrefix[1];
            char modifier = aimPrefix[2];

            switch (symbChar)
            {
                case 'A':
                    result.Symbology = "code39";
                    break;

                case 'B':
                    result.Symbology = "telepen";
                    break;

                case 'C':
                    result.Symbology = "code128";
                    if (modifier == '1')
                    {
                        result.Symbology = "gs1-128";
                        result.Fnc1 = 1;
                    }
                    break;

                case 'D':
                    result.Symbology = "code1";
                    break;

                case 'E':
                    // EAN/UPC: la simbologia dipende dalla lunghezza del valore
                    if (barcodeValue.Length == 13)
                        result.Symbology = "ean13";
                    else if (barcodeValue.Length == 12)
                        result.Symbology = "upca";
                    else if (barcodeValue.Length == 8)
                        result.Symbology = modifier == '4' ? "ean8" : "upce";
                    break;

                case 'F':
                    result.Symbology = "codabar";
                    break;

                case 'G':
                    result.Symbology = "code93";
                    break;

                case 'H':
                    result.Symbology = "code11";
                    break;

                case 'I':
                    result.Symbology = "interleaved-2-of-5";
                    break;

                case 'K':
                    result.Symbology = "code16k";
                    break;

                case 'L':
                    result.Symbology = "pdf417";
                    break;

                case 'M':
                    result.Symbology = "msi";
                    break;

                case 'N':
                    result.Symbology = "anker";
                    break;

                case 'O':
                    if (modifier == '4' || modifier == '5')
                        result.Symbology = "codablock-f";
                    else if (modifier == '6')
                        result.Symbology = "codablock-a";
                    break;

                case 'P':
                    result.Symbology = "plessey";
                    break;

                case 'R':
                case 'S':
                    result.Symbology = "straight-2-of-5";
                    break;

                case 'Q':
                    result.Symbology = "qr-code";
                    if (modifier == '0')
                    {
                        result.Model = 1;
                    }
                    else
                    {
                        result.Model = 2;
                        if (modifier == '3' || modifier == '4') result.Fnc1 = 1;
                        if (modifier == '5' || modifier == '6') result.Fnc1 = 2;
                    }
                    break;

                case 'U':
                    result.Symbology = "maxicode";
                    break;

                case 'X':
                    if (modifier == '0')
                    {
                        result.Symbology = "ean13";
                    }
                    else
                    {
                        switch (modifier)
                        {
                            case '9': result.Symbology = "ean13"; break;
                            case 'C': result.Symbology = "ean8"; break;
                            case 'g': result.Symbology = "upca"; break;
                            case 'k': result.Symbology = "upce"; break;
                            case 'r': result.Symbology = "gs1-databar-omni"; break;
                            case 's': result.Symbology = "gs1-databar-limited"; break;
                            case 't': result.Symbology = "gs1-databar-expanded"; break;
                            case 'V': result.Symbology = "pdf417"; break;
                            case 'S': result.Symbology = "qr-code-micro"; break;
                        }
                    }
                    break;

                case 'c':
                    result.Symbology = "channel-code";
                    break;

                case 'd':
                    result.Symbology = "data-matrix";
                    if (modifier == '2' || modifier == '5') result.Fnc1 = 1;
                    if (modifier == '3' || modifier == '6') result.Fnc1 = 2;
                    break;

                case 'e':
                    result.Symbology = "gs1-databar-omni";
                    break;

                case 'h':
                    result.Symbology = "chinese-sensible-code";
                    break;

                case 'o':
                    result.Symbology = "ocr";
                    break;

                case 'p':
                    result.Symbology = "posi-code";
                    break;

                case 's':
                    result.Symbology = "super-code";
                    break;

                case 'z':
                    result.Symbology = "aztec-code";
                    if (modifier == '1' || modifier == '4' || modifier == '7' || modifier == 'A')
                        result.Fnc1 = 1;
                    if (modifier == '2' || modifier == '5' || modifier == '8' || modifier == 'B')
                        result.Fnc1 = 1;
                    break;
            }

            return result;
        }
    }
}