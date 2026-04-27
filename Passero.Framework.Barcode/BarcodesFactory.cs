#if NETFRAMEWORK
    using System.Drawing;
    using System.Drawing.Imaging;
#else
using SkiaSharp;
#endif

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ZXing;
using ZXing.Common;
using System.Security;
using QRCoder;

#if NETFRAMEWORK
//[assembly: AllowPartiallyTrustedCallers]
//[assembly: SecurityRules(SecurityRuleSet.Level1)]
#endif

namespace Passero.Framework.BarcodeX
{
    public enum BarcodeFormat
    {
        AZTEC = ZXing.BarcodeFormat.AZTEC,
        CODABAR = ZXing.BarcodeFormat.CODABAR,
        CODE_39 = ZXing.BarcodeFormat.CODE_39,
        CODE_93 = ZXing.BarcodeFormat.CODE_93,
        CODE_128 = ZXing.BarcodeFormat.CODE_128,
        DATA_MATRIX = ZXing.BarcodeFormat.DATA_MATRIX,
        EAN_8 = ZXing.BarcodeFormat.EAN_8,
        EAN_13 = ZXing.BarcodeFormat.EAN_13,
        IMB = ZXing.BarcodeFormat.IMB,
        ITF = ZXing.BarcodeFormat.ITF,
        MAXICODE = ZXing.BarcodeFormat.MAXICODE,
        MSI = ZXing.BarcodeFormat.MSI,
        PDF_417 = ZXing.BarcodeFormat.PDF_417,
        PHARMA_CODE = ZXing.BarcodeFormat.PHARMA_CODE,
        PLESSEY = ZXing.BarcodeFormat.PLESSEY,
        QR_CODE = ZXing.BarcodeFormat.QR_CODE,
        RSS_14 = ZXing.BarcodeFormat.RSS_14,
        RSS_EXPANDED = ZXing.BarcodeFormat.RSS_EXPANDED,
        UPC_A = ZXing.BarcodeFormat.UPC_A,
        UPC_E = ZXing.BarcodeFormat.UPC_E,
        UPC_EAN_EXTENSION = ZXing.BarcodeFormat.UPC_EAN_EXTENSION
    }

    public enum BarcodeFormatSpire : long
    {
        CODABAR = Spire.Barcode.BarCodeType.Codabar,
        CODE11 = Spire.Barcode.BarCodeType.Code11,
        CODE128 = Spire.Barcode.BarCodeType.Code128,
        CODE39 = Spire.Barcode.BarCodeType.Code39,
        CODE39EXTENDED = Spire.Barcode.BarCodeType.Code39Extended,
        CODE93 = Spire.Barcode.BarCodeType.Code93,
        CODE93EXTENDED = Spire.Barcode.BarCodeType.Code93Extended,
        CODE25 = Spire.Barcode.BarCodeType.Code25,
        EAN14 = Spire.Barcode.BarCodeType.EAN14,
        EAN13 = Spire.Barcode.BarCodeType.EAN13,
        EAN8 = Spire.Barcode.BarCodeType.EAN8,
        EAN128 = Spire.Barcode.BarCodeType.EAN128,
        INTERLEAVED25 = Spire.Barcode.BarCodeType.Interleaved25,
        QRCODE = Spire.Barcode.BarCodeType.QRCode
    }

    public class BarcodeFontFactory
    {
        public static string QRCODE(string CodeValue)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
            AsciiQRCode qrCode = new AsciiQRCode(qrCodeData);
            string qrCodeAsAsciiArt = qrCode.GetGraphic(1);
            return qrCodeAsAsciiArt;
        }

        public static string EAN13AddOn(string CodeValue)
        {
            int checksum = 0;
            bool tableA;
            string addOn = string.Empty;

            if (Regex.IsMatch(CodeValue, @"^\d{2}$") || Regex.IsMatch(CodeValue, @"^\d{5}$"))
            {
                if (CodeValue.Length == 2)
                {
                    checksum = 10 + Convert.ToInt32(CodeValue) % 4;
                }
                else
                {
                    for (int i = 0; i <= 4; i += 2)
                    {
                        checksum += Convert.ToInt32(CodeValue.Substring(i, 1));
                    }
                    checksum = (checksum * 3 + Convert.ToInt32(CodeValue.Substring(1, 1)) * 9 + Convert.ToInt32(CodeValue.Substring(3, 1)) * 9) % 10;
                }

                addOn = "[";

                for (int i = 0; i < CodeValue.Length; i++)
                {
                    tableA = false;
                    switch (i)
                    {
                        case 0:
                            if ((checksum >= 4 && checksum <= 9) || checksum == 10 || checksum == 11)
                            {
                                tableA = true;
                            }
                            break;
                        case 1:
                            if (checksum == 1 || checksum == 2 || checksum == 3 || checksum == 5 || checksum == 6 || checksum == 9 || checksum == 10 || checksum == 12)
                            {
                                tableA = true;
                            }
                            break;
                        case 2:
                            if (checksum == 0 || checksum == 2 || checksum == 3 || checksum == 6 || checksum == 7 || checksum == 8)
                            {
                                tableA = true;
                            }
                            break;
                        case 3:
                            if (checksum == 0 || checksum == 1 || checksum == 3 || checksum == 4 || checksum == 8 || checksum == 9)
                            {
                                tableA = true;
                            }
                            break;
                        case 4:
                            if (checksum == 0 || checksum == 1 || checksum == 2 || checksum == 4 || checksum == 5 || checksum == 7)
                            {
                                tableA = true;
                            }
                            break;
                    }

                    if (tableA)
                    {
                        addOn += (char)(65 + Convert.ToInt32(CodeValue.Substring(i, 1)));
                    }
                    else
                    {
                        addOn += (char)(75 + Convert.ToInt32(CodeValue.Substring(i, 1)));
                    }

                    if ((CodeValue.Length == 2 && i == 0) || (CodeValue.Length == 5 && i < 4))
                    {
                        addOn += (char)92;
                    }
                }
            }

            return addOn;
        }

        public static string CODE128(string CodeValue)
        {
            int checksum = 0, mini, dummy;
            bool tableB = true;
            string result = string.Empty;

            if (CodeValue.Length > 0)
            {
                for (int i = 0; i < CodeValue.Length; i++)
                {
                    char c = CodeValue[i];
                    if ((c < 32 || c > 126) && c != 203)
                    {
                        return string.Empty;
                    }
                }

                result = string.Empty;
                int index = 0;

                while (index < CodeValue.Length)
                {
                    if (tableB)
                    {
                        mini = (index == 0 || index + 3 == CodeValue.Length) ? 4 : 6;
                        mini--;

                        if (index + mini < CodeValue.Length)
                        {
                            while (mini >= 1)
                            {
                                char c = CodeValue[index + mini];
                                if (c < '0' || c > '9')
                                {
                                    break;
                                }
                                mini--;
                            }
                        }

                        if (mini < 0)
                        {
                            if (index == 0)
                            {
                                result += (char)210;
                            }
                            else
                            {
                                result += (char)204;
                            }
                            tableB = false;
                        }
                        else
                        {
                            if (index == 0)
                            {
                                result += (char)209;
                            }
                        }
                    }

                    if (!tableB)
                    {
                        mini = 2;
                        mini--;

                        if (index + mini < CodeValue.Length)
                        {
                            while (mini >= 1)
                            {
                                char c = CodeValue[index + mini];
                                if (c < '0' || c > '9')
                                {
                                    break;
                                }
                                mini--;
                            }
                        }

                        if (mini < 0)
                        {
                            dummy = int.Parse(CodeValue.Substring(index, 2));
                            dummy = dummy < 95 ? dummy + 32 : dummy + 105;
                            result += (char)dummy;
                            index += 2;
                        }
                        else
                        {
                            result += (char)205;
                            tableB = true;
                        }
                    }

                    if (tableB)
                    {
                        result += CodeValue[index];
                        index++;
                    }
                }

                for (int i = 0; i < result.Length; i++)
                {
                    dummy = result[i];
                    dummy = dummy < 127 ? dummy - 32 : dummy - 105;
                    if (i == 0)
                    {
                        checksum = dummy;
                    }
                    checksum = (checksum + i * dummy) % 103;
                }

                checksum = checksum < 95 ? checksum + 32 : checksum + 105;
                result += (char)checksum + (char)211;
            }

            return result;
        }

        public static string EAN128b(string CodeValue)
        {
            int checksum = 0, mini, dummy;
            bool tableB = true;
            string chaine = CodeValue;
            string ean128 = string.Empty;

            if (chaine.Length > 0)
            {
                for (int i = 0; i < chaine.Length; i++)
                {
                    char c = chaine[i];
                    if ((c < 32 || c > 126) && c != 203 && c != 207)
                    {
                        return string.Empty;
                    }
                }

                ean128 = string.Empty;
                int index = 0;

                while (index < chaine.Length)
                {
                    if (tableB)
                    {
                        mini = (index == 0 || index + 3 == chaine.Length) ? 4 : 6;
                        mini--;

                        if (index + mini < chaine.Length)
                        {
                            while (mini >= 0)
                            {
                                char c = chaine[index + mini];
                                if ((c < '0' || c > '9') && c != 207)
                                {
                                    break;
                                }
                                mini--;
                            }
                        }

                        if (mini < 0)
                        {
                            if (index == 0)
                            {
                                ean128 += (char)210;
                            }
                            else
                            {
                                ean128 += (char)204;
                            }
                            tableB = false;
                        }
                        else
                        {
                            if (index == 0)
                            {
                                ean128 += (char)209;
                            }
                        }
                    }

                    if (!tableB)
                    {
                        if (chaine[index] == 207)
                        {
                            ean128 += chaine[index];
                            index++;
                        }
                        else
                        {
                            mini = 2;
                            mini--;

                            if (index + mini < chaine.Length)
                            {
                                while (mini >= 0)
                                {
                                    char c = chaine[index + mini];
                                    if (c < '0' || c > '9')
                                    {
                                        break;
                                    }
                                    mini--;
                                }
                            }

                            if (mini < 0)
                            {
                                dummy = int.Parse(chaine.Substring(index, 2));
                                dummy = dummy < 95 ? dummy + 32 : dummy + 105;
                                ean128 += (char)dummy;
                                index += 2;
                            }
                            else
                            {
                                ean128 += (char)205;
                                tableB = true;
                            }
                        }
                    }

                    if (tableB)
                    {
                        ean128 += chaine[index];
                        index++;
                    }
                }

                for (int i = 0; i < ean128.Length; i++)
                {
                    dummy = ean128[i];
                    dummy = dummy < 127 ? dummy - 32 : dummy - 105;
                    if (i == 0)
                    {
                        checksum = dummy;
                    }
                    checksum = (checksum + i * dummy) % 103;
                }

                checksum = checksum < 95 ? checksum + 32 : checksum + 105;
                ean128 += (char)checksum + (char)211;
            }

            return ean128;
        }

        public static string EAN128(string CodeValue)
        {
            int checksum = 0, mini, dummy;
            string result = string.Empty;
            string chaine = CodeValue;
            bool tableB = true;

            if (chaine.Length > 0)
            {
                for (int i = 0; i < chaine.Length; i++)
                {
                    char c = chaine[i];
                    if ((c < 32 || c > 126) && c != 203 && c != 207)
                    {
                        return string.Empty;
                    }
                }

                result = string.Empty;
                int index = 0;

                while (index < chaine.Length)
                {
                    if (tableB)
                    {
                        mini = (index == 0 || index + 3 == chaine.Length) ? 4 : 6;
                        mini--;

                        if (index + mini < chaine.Length)
                        {
                            while (mini >= 0)
                            {
                                char c = chaine[index + mini];
                                if ((c < '0' || c > '9') && c != 207)
                                {
                                    break;
                                }
                                mini--;
                            }
                        }

                        if (mini < 0)
                        {
                            if (index == 0)
                            {
                                result += (char)210;
                            }
                            else
                            {
                                result += (char)204;
                            }
                            tableB = false;
                        }
                        else
                        {
                            if (index == 0)
                            {
                                result += (char)209;
                            }
                        }
                    }

                    if (!tableB)
                    {
                        if (chaine[index] == 207)
                        {
                            result += chaine[index];
                            index++;
                        }
                        else
                        {
                            mini = 2;
                            mini--;

                            if (index + mini < chaine.Length)
                            {
                                while (mini >= 1)
                                {
                                    char c = chaine[index + mini];
                                    if (c < '0' || c > '9')
                                    {
                                        break;
                                    }
                                    mini--;
                                }
                            }

                            if (mini < 0)
                            {
                                dummy = int.Parse(chaine.Substring(index, 2));
                                dummy = dummy < 95 ? dummy + 32 : dummy + 105;
                                result += (char)dummy;
                                index += 2;
                            }
                            else
                            {
                                result += (char)205;
                                tableB = true;
                            }
                        }
                    }

                    if (tableB)
                    {
                        result += chaine[index];
                        index++;
                    }
                }

                for (int i = 0; i < result.Length; i++)
                {
                    dummy = result[i];
                    dummy = dummy < 127 ? dummy - 32 : dummy - 105;
                    if (i == 0)
                    {
                        checksum = dummy;
                    }
                    checksum = (checksum + i * dummy) % 103;
                }

                checksum = checksum < 95 ? checksum + 32 : checksum + 105;
                result += (char)checksum + (char)211;
            }

            return result;
        }

        public static string Code25I(string CodeValue, bool key = false)
        {
            int checksum = 0, dummy;
            string result = string.Empty;

            if (CodeValue.Length > 0)
            {
                foreach (char c in CodeValue)
                {
                    if (c < '0' || c > '9')
                        return string.Empty;
                }

                if (key)
                {
                    for (int i = CodeValue.Length - 1; i >= 0; i -= 2)
                    {
                        checksum += int.Parse(CodeValue[i].ToString());
                    }
                    checksum *= 3;
                    for (int i = CodeValue.Length - 2; i >= 0; i -= 2)
                    {
                        checksum += int.Parse(CodeValue[i].ToString());
                    }
                    CodeValue += ((10 - checksum % 10) % 10).ToString();
                }

                if (CodeValue.Length % 2 != 0)
                    return string.Empty;

                for (int i = 0; i < CodeValue.Length; i += 2)
                {
                    dummy = int.Parse(CodeValue.Substring(i, 2));
                    dummy = dummy < 94 ? dummy + 33 : dummy + 101;
                    result += (char)dummy;
                }

                result = (char)201 + result + (char)202;
            }

            return result;
        }

        public static string Code39(string CodeValue)
        {
            string result = "";
            CodeValue = CodeValue.ToUpper();
            if (CodeValue.Length > 0)
            {
                foreach (char c in CodeValue)
                {
                    if (!(char.IsLetterOrDigit(c) || " $%+-".Contains(c)))
                        return "";
                }
                result = "*" + CodeValue + "*";
            }
            return result;
        }

        public static string EAN8(string CodeValue)
        {
            string _CodeValue = CodeValue;
            string CodeBarre = "";

            if (!long.TryParse(_CodeValue, out _)) return "";

            switch (_CodeValue.Length)
            {
                case 7:
                    _CodeValue += EAN8CheckDigit(_CodeValue);
                    break;
                case 8:
                    if (EAN8CheckDigit(_CodeValue).ToString() != _CodeValue.Substring(7, 1))
                        return "";
                    break;
                default:
                    return "";
            }

            CodeBarre = ":";
            for (int i = 0; i < 4; i++)
                CodeBarre += (char)(65 + int.Parse(_CodeValue[i].ToString()));

            CodeBarre += "*";
            for (int i = 4; i < 8; i++)
                CodeBarre += (char)(97 + int.Parse(_CodeValue[i].ToString()));

            CodeBarre += "+";
            return CodeBarre;
        }

        public static string EAN8CheckDigit(string Barcode)
        {
            int TotalOdd = 0, TotalEven = 0, Total;
            Barcode = Barcode.Trim();

            for (int i = 0; i < Barcode.Length; i += 2)
                TotalOdd += int.Parse(Barcode[i].ToString());

            TotalOdd *= 3;

            for (int i = 1; i < Barcode.Length; i += 2)
                TotalEven += int.Parse(Barcode[i].ToString());

            Total = TotalOdd + TotalEven;
            int Mod10CheckDigit = 10 - (Total % 10);
            return (Mod10CheckDigit == 10 ? 0 : Mod10CheckDigit).ToString();
        }

        public static string EAN13(string CodeValue)
        {
            string _CodeValue = CodeValue;
            string CodeBarre = "";

            if (!long.TryParse(_CodeValue, out _)) return "";

            switch (_CodeValue.Length)
            {
                case 12:
                    _CodeValue += EAN13CheckDigit(_CodeValue);
                    break;
                case 13:
                    if (EAN13CheckDigit(_CodeValue).ToString() != _CodeValue.Substring(12, 1))
                        return "";
                    break;
                default:
                    return "";
            }

            CodeBarre = _CodeValue[0] + ((char)(65 + int.Parse(_CodeValue[1].ToString()))).ToString();
            int first = int.Parse(_CodeValue[0].ToString());

            for (int i = 2; i < 7; i++)
            {
                bool tableA = false;
                switch (i)
                {
                    case 2: tableA = first >= 0 && first <= 3; break;
                    case 3: tableA = first == 0 || first == 4 || first == 7 || first == 8; break;
                    case 4: tableA = first == 0 || first == 1 || first == 4 || first == 5 || first == 9; break;
                    case 5: tableA = first == 0 || first == 2 || first == 5 || first == 6 || first == 7; break;
                    case 6: tableA = first == 0 || first == 3 || first == 6 || first == 8 || first == 9; break;
                }

                CodeBarre += (char)(tableA ? 65 + int.Parse(_CodeValue[i].ToString()) : 75 + int.Parse(_CodeValue[i].ToString()));
            }

            CodeBarre += "*";
            for (int i = 7; i < 13; i++)
                CodeBarre += (char)(97 + int.Parse(_CodeValue[i].ToString()));

            CodeBarre += "+";
            return CodeBarre;
        }

        public static string EAN13CheckDigit(string Barcode)
        {
            int X = 0, Y = 0, j = 11;
            try
            {
                for (int i = 1; i <= 12; i++)
                {
                    if (i % 2 == 0)
                        X += int.Parse(Barcode[j].ToString());
                    else
                        Y += int.Parse(Barcode[j].ToString());
                    j--;
                }
                int Z = X + (3 * Y);
                return ((10 - (Z % 10)) % 10).ToString();
            }
            catch
            {
                return "";
            }
        }
    }

#if NETFRAMEWORK
    [SecuritySafeCritical]
#endif
    public static class BarcodeImageFactory
    {
#if NETFRAMEWORK
        [SecuritySafeCritical]
        public static MemoryStream GenerateBarcodeAsMemoryStreamSpire(
            string BarcodeType,
            string Text,
            int Width = 300,
            int Height = 150,
            int Margin = 5,
            bool NoPadding = false,
            bool PureBarcode = false,
            bool GS1Format = false)
        {
            try
            {
                Spire.Barcode.BarcodeSettings settings = new Spire.Barcode.BarcodeSettings
                {
                    Type = (Spire.Barcode.BarCodeType)Enum.Parse(typeof(Spire.Barcode.BarCodeType), BarcodeType, true),
                    Data = Text,
                    ShowText = false,
                    X = 2,
                    HasBorder = false,
                    ImageWidth = Width,
                    ImageHeight = Height,
                    TopMargin = Margin,
                    BottomMargin = Margin,
                    LeftMargin = Margin,
                    RightMargin = Margin
                };

                Spire.Barcode.BarCodeGenerator generator = new Spire.Barcode.BarCodeGenerator(settings);
                System.Drawing.Image barcodeImage = generator.GenerateImage();
                MemoryStream stream = new MemoryStream();
                barcodeImage.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la generazione del barcode con Spire.Barcode: " + ex.Message, ex);
            }
        }

        [SecuritySafeCritical]
        public static byte[] GenerateBarcodeAsByteArraySpire(
            string BarcodeType,
            string Text,
            int Width = 300,
            int Height = 150,
            int Margin = 5,
            bool NoPadding = false,
            bool PureBarcode = false,
            bool GS1Format = false)
        {
            try
            {
                Spire.Barcode.BarcodeSettings settings = new Spire.Barcode.BarcodeSettings
                {
                    Type = (Spire.Barcode.BarCodeType)Enum.Parse(typeof(Spire.Barcode.BarCodeType), BarcodeType, true),
                    Data = Text,
                    ShowText = false,
                    X = 2,
                    HasBorder = false,
                    ImageWidth = Width,
                    ImageHeight = Height,
                    TopMargin = Margin,
                    BottomMargin = Margin,
                    LeftMargin = Margin,
                    RightMargin = Margin
                };

                Spire.Barcode.BarCodeGenerator generator = new Spire.Barcode.BarCodeGenerator(settings);
                System.Drawing.Image barcodeImage = generator.GenerateImage();
                MemoryStream stream = new MemoryStream();
                barcodeImage.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la generazione del barcode con Spire.Barcode: " + ex.Message, ex);
            }
        }
#endif

#if !NETFRAMEWORK
        // FIX #1: barcodeImage.Save() era commentato — lo stream veniva restituito vuoto.
        // Ora l'immagine viene salvata correttamente tramite SKBitmap.Encode().
        public static MemoryStream GenerateBarcodeAsMemoryStreamSpire(
            string BarcodeType,
            string Text,
            int Width = 300,
            int Height = 150,
            int Margin = 5,
            bool NoPadding = false,
            bool PureBarcode = false,
            bool GS1Format = false)
        {
            try
            {
                Spire.Barcode.BarcodeSettings settings = new Spire.Barcode.BarcodeSettings
                {
                    Type = (Spire.Barcode.BarCodeType)Enum.Parse(typeof(Spire.Barcode.BarCodeType), BarcodeType, true),
                    Data = Text,
                    ShowText = false,
                    X = 2,
                    HasBorder = false,
                    ImageWidth = Width,
                    ImageHeight = Height,
                    TopMargin = Margin,
                    BottomMargin = Margin,
                    LeftMargin = Margin,
                    RightMargin = Margin
                };

                Spire.Barcode.BarCodeGenerator generator = new Spire.Barcode.BarCodeGenerator(settings);
                SKBitmap barcodeImage = GenerateBarcodeAsBitmap(BarcodeType, Text, Width, Height, Margin, NoPadding, PureBarcode, GS1Format);
                MemoryStream stream = new MemoryStream();
                using (var data = barcodeImage.Encode(SKEncodedImageFormat.Png, 100))
                {
                    data.SaveTo(stream);
                }
                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la generazione del barcode con Spire.Barcode: " + ex.Message, ex);
            }
        }
#endif

        public static string TestZXingTrust()
        {
            var zxingAssembly = typeof(ZXing.BarcodeWriterPixelData).Assembly;
            return zxingAssembly.IsFullyTrusted
                ? "ZXing è FullTrust"
                : "ZXing NON è FullTrust";
        }

        public static string TestSpireTrust()
        {
            var spirerAssembly = typeof(Spire.Barcode.BarCodeGenerator).Assembly;
            return spirerAssembly.IsFullyTrusted
                ? "Spire.Barcode è FullTrust"
                : "Spire.Barcode NON è FullTrust";
        }

        public static string TestTrust()
        {
            return AppDomain.CurrentDomain.IsFullyTrusted
                ? "FullTrust attivo"
                : "ATTENZIONE: parzialmente attendibile";
        }

        public static string Test(string TestString, string TestString2 = "")
        {
            return "TestString=" + TestString + "; TestString2=" + TestString2;
        }

        public static string TestGenerate(string BarcodeType, string Text, int Width = 300, int Height = 150, int Margin = 5, bool NoPadding = false, bool PureBarcode = false, bool GS1Format = false)
        {
            try
            {
                byte[] byteArray = GenerateBarcodeAsByteArray(BarcodeType, Text, Width, Height, Margin, NoPadding, PureBarcode, GS1Format);
                return "Bytes = " + byteArray.Length.ToString();
            }
            catch (Exception ex)
            {
                return "Errore " + ex.Message;
            }
        }

        /// <summary>
        /// Genera un barcode del tipo specificato e restituisce l'immagine risultante (PNG) come Byte[].
        /// </summary>
#if NETFRAMEWORK
        [SecuritySafeCritical]
        public static byte[] GenerateBarcodeAsByteArray(string BarcodeType, string Text, int Width = 300, int Height = 150, int Margin = 5, bool NoPadding = false, bool PureBarcode = false, bool GS1Format = false)
        {
            if (string.IsNullOrWhiteSpace(BarcodeType))
                throw new ArgumentException("Il tipo di barcode non può essere vuoto.", nameof(BarcodeType));

            if (!Enum.TryParse<BarcodeFormat>(BarcodeType, true, out BarcodeFormat format))
                throw new ArgumentException($"Il tipo di barcode '{BarcodeType}' non è valido.", nameof(BarcodeType));

            try
            {
                var writer = new BarcodeWriterPixelData
                {
                    Format = (ZXing.BarcodeFormat)format,
                    Options = new EncodingOptions
                    {
                        Width = Width,
                        Height = Height,
                        PureBarcode = PureBarcode,
                        NoPadding = NoPadding,
                        Margin = Margin,
                        GS1Format = GS1Format
                    }
                };

                var result = writer.Write(Text);
                Bitmap bitmap = new Bitmap(result.Width, result.Height);

                const int bpp = 32;
                for (int y = 0; y < result.Height; y++)
                {
                    for (int x = 0; x < result.Width; x++)
                    {
                        int i = ((y * result.Width) + x) * (bpp / 8);
                        byte r = result.Pixels[i];
                        byte g = result.Pixels[i + 1];
                        byte b = result.Pixels[i + 2];
                        Color color = Color.FromArgb(r, g, b);
                        bitmap.SetPixel(x, y, color);
                    }
                }

                if (bitmap == null)
                    throw new Exception("Bitmap restituito da ZXing è null.");

                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la generazione del barcode: " + ex.Message + "\nStack:\n" + ex.StackTrace, ex);
            }
        }
#endif

#if !NETFRAMEWORK
        public static byte[] GenerateBarcodeAsByteArray(string BarcodeType, string Text, int Width = 300, int Height = 150, int Margin = 5, bool NoPadding = false, bool PureBarcode = false, bool GS1Format = false)
        {
            MemoryStream stream = GenerateBarcodeAsMemoryStream(BarcodeType, Text, Width, Height, Margin, NoPadding, PureBarcode, GS1Format);
            return stream.ToArray();
        }
#endif

#if NETFRAMEWORK
        [SecuritySafeCritical]
        public static Bitmap GenerateBarcodeAsBitmapWrapper(string BarcodeType, string Text, int Width = 300, int Height = 150, int Margin = 5, bool NoPadding = false, bool PureBarcode = false, bool GS1Format = false)
        {
            return GenerateBarcodeAsBitmap(BarcodeType, Text, Width, Height, Margin, NoPadding, PureBarcode, GS1Format);
        }
#endif

        /// <summary>
        /// Genera un barcode e restituisce il risultato come Bitmap (System.Drawing su .NET Framework, SKBitmap su .NET 8+).
        /// </summary>
#if NETFRAMEWORK
        [SecuritySafeCritical]
        public static Bitmap GenerateBarcodeAsBitmap(string BarcodeType, string Text, int Width = 300, int Height = 150, int Margin = 5, bool NoPadding = false, bool PureBarcode = false, bool GS1Format = false)
        {
            if (string.IsNullOrWhiteSpace(BarcodeType))
                throw new ArgumentException("Il tipo di barcode non può essere vuoto.", nameof(BarcodeType));

            if (!Enum.TryParse<BarcodeFormat>(BarcodeType, true, out BarcodeFormat format))
                throw new ArgumentException($"Il tipo di barcode '{BarcodeType}' non è valido.", nameof(BarcodeType));

            var writer = new BarcodeWriterPixelData
            {
                Format = (ZXing.BarcodeFormat)format,
                Options = new EncodingOptions
                {
                    Width = Width,
                    Height = Height,
                    PureBarcode = PureBarcode,
                    NoPadding = NoPadding,
                    Margin = Margin,
                    GS1Format = GS1Format
                }
            };

            var result = writer.Write(Text);
            Bitmap bmp = new Bitmap(result.Width, result.Height);

            const int bpp = 32;
            for (int y = 0; y < result.Height; y++)
            {
                for (int x = 0; x < result.Width; x++)
                {
                    int i = ((y * result.Width) + x) * (bpp / 8);
                    byte r = result.Pixels[i];
                    byte g = result.Pixels[i + 1];
                    byte b = result.Pixels[i + 2];
                    Color color = Color.FromArgb(r, g, b);
                    bmp.SetPixel(x, y, color);
                }
            }

            return bmp;
        }
#endif

#if !NETFRAMEWORK
        // FIX #2: aggiunto Renderer esplicito per ZXing.Net con SkiaSharp.
        // Senza di esso, BarcodeWriter<SKBitmap> non saprebbe come produrre l'output.
        public static SKBitmap GenerateBarcodeAsBitmap(string BarcodeType, string Text, int Width = 300, int Height = 150, int Margin = 5, bool NoPadding = false, bool PureBarcode = false, bool GS1Format = false)
        {
            if (string.IsNullOrWhiteSpace(BarcodeType))
                throw new ArgumentException("Il tipo di barcode non può essere vuoto.", nameof(BarcodeType));

            if (!Enum.TryParse<BarcodeFormat>(BarcodeType, true, out BarcodeFormat format))
                throw new ArgumentException($"Il tipo di barcode '{BarcodeType}' non è valido.", nameof(BarcodeType));

            var writer = new BarcodeWriter<SKBitmap>
            {
                Format = (ZXing.BarcodeFormat)format,
                Options = new EncodingOptions
                {
                    Width = Width,
                    Height = Height,
                    PureBarcode = PureBarcode,
                    NoPadding = NoPadding,
                    Margin = Margin,
                    GS1Format = GS1Format
                },
                Renderer = new ZXing.SkiaSharp.Rendering.SKBitmapRenderer()
            };

            return writer.Write(Text);
        }
#endif

#if NETFRAMEWORK
        public static string GetBase64(System.Drawing.Image image)
        {
            using (MemoryStream m = new MemoryStream())
            {
                image.Save(m, ImageFormat.Png);
                byte[] imageBytes = m.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        private static Bitmap ConvertMatrixToBitmap(BitMatrix matrix)
        {
            int width = matrix.Width;
            int height = matrix.Height;
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var color = matrix[x, y] ? Color.Black : Color.White;
                    bmp.SetPixel(x, y, color);
                }
            }

            return bmp;
        }
#endif

        /// <summary>
        /// Genera un barcode e restituisce l'immagine PNG come MemoryStream.
        /// </summary>
#if NETFRAMEWORK
        [SecuritySafeCritical]
        public static MemoryStream GenerateBarcodeAsMemoryStream(string BarcodeType, string Text, int Width = 300, int Height = 150, int Margin = 5, bool NoPadding = false, bool PureBarcode = false, bool GS1Format = false)
        {
            if (string.IsNullOrWhiteSpace(BarcodeType))
                throw new ArgumentException("Il tipo di barcode non può essere vuoto.", nameof(BarcodeType));

            if (!Enum.TryParse<BarcodeFormat>(BarcodeType, true, out BarcodeFormat format))
                throw new ArgumentException($"Il tipo di barcode '{BarcodeType}' non è valido.", nameof(BarcodeType));

            try
            {
                var writer = new BarcodeWriterPixelData
                {
                    Format = (ZXing.BarcodeFormat)format,
                    Options = new EncodingOptions
                    {
                        Width = Width,
                        Height = Height,
                        PureBarcode = PureBarcode,
                        NoPadding = NoPadding,
                        Margin = Margin,
                        GS1Format = GS1Format
                    }
                };

                var result = writer.Write(Text);
                Bitmap bitmap = new Bitmap(result.Width, result.Height);

                const int bpp = 32;
                for (int y = 0; y < result.Height; y++)
                {
                    for (int x = 0; x < result.Width; x++)
                    {
                        int i = ((y * result.Width) + x) * (bpp / 8);
                        byte r = result.Pixels[i];
                        byte g = result.Pixels[i + 1];
                        byte b = result.Pixels[i + 2];
                        Color color = Color.FromArgb(r, g, b);
                        bitmap.SetPixel(x, y, color);
                    }
                }

                if (bitmap == null)
                    throw new Exception("Bitmap restituito da ZXing è null.");

                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la generazione del barcode: " + ex.Message + "\nStack:\n" + ex.StackTrace, ex);
            }
        }
#endif

#if !NETFRAMEWORK
        public static MemoryStream GenerateBarcodeAsMemoryStream(string BarcodeType, string Text, int Width = 300, int Height = 150, int Margin = 5, bool NoPadding = false, bool PureBarcode = false, bool GS1Format = false)
        {
            if (string.IsNullOrWhiteSpace(BarcodeType))
                throw new ArgumentException("Il tipo di barcode non può essere vuoto.", nameof(BarcodeType));

            if (!Enum.TryParse<BarcodeFormat>(BarcodeType, true, out BarcodeFormat format))
                throw new ArgumentException($"Il tipo di barcode '{BarcodeType}' non è valido.", nameof(BarcodeType));

            try
            {
                SKBitmap bitmap = GenerateBarcodeAsBitmap(BarcodeType, Text, Width, Height, Margin, NoPadding, PureBarcode, GS1Format);
                MemoryStream stream = new MemoryStream();
                using (var data = bitmap.Encode(SKEncodedImageFormat.Png, 100))
                {
                    data.SaveTo(stream);
                }
                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la generazione del barcode.", ex);
            }
        }
#endif
    }
}