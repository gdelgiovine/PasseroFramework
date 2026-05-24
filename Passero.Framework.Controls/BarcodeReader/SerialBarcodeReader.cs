using System;
using System.ComponentModel;
using System.IO;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Wisej.Core;
using Wisej.Web;
//string resourceName = "Passero.Framework.Controls.BarcodeReader.webserialbarcodecore.js";

namespace Passero.Framework.Controls
{

    public partial class SerialBarcodeReader : Wisej.Web.UserControl
    {


        public string DeviceID { get; set; } = "";
        public bool IsConnected { get; set; } = false;
        private string BarcodeData = "";
        private string FullBarcodeData = "";
        // Evento pubblico che notifica la lettura del barcode e passa BarcodeData
        [Category("Barcode Reader")]
        public event EventHandler<BarcodeScannedEventArgs> OnBarcodeScanned;
        // Evento pubblico che notifica la connessione e passa DeviceID
        [Category("Barcode Reader")]
        public event EventHandler<ConnectEventArgs> OnConnect;
        // Evento pubblico che notifica la disconnessione e passa DeviceID
        [Category("Barcode Reader")]
        public event EventHandler<DisconnectEventArgs> OnDisconnect;




        public SerialBarcodeReader()
        {
            InitializeComponent();
            this.btnBarcodeReader.Dock = DockStyle.Fill;
            this.btnBarcodeReader.BackColor = System.Drawing.Color.Red;

            //// Carica il contenuto del file JS embedded come InitScript
            //this.InitScript = LoadEmbeddedScript("GDGWisejSerialBarcodeReader.webserialbarcode.js");
        }

        //protected override void OnCreateControl()
        //{
        //    base.OnCreateControl();
        //    if (!DesignMode)
        //    {
        //        // Usa il JS personalizzato se valorizzato, altrimenti usa la risorsa embedded
        //        this.InitScript = !string.IsNullOrWhiteSpace(CustomBarcodeReaderJS)
        //            ? CustomBarcodeReaderJS
        //            : LoadEmbeddedScript("GDGWisejSerialBarcodeReader.webserialbarcode.js");
        //    }
        //}

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!DesignMode)
            {

                string resourceName = "Passero.Framework.Controls.BarcodeReader.webserialbarcodecore.js";

                var js = !string.IsNullOrWhiteSpace(CustomBarcodeReaderJS)
                    ? CustomBarcodeReaderJS
                    : LoadEmbeddedScript(resourceName);


                js = js.Replace("{{ BAUD_RATE }}", BaudRate.ToString())
                       .Replace("{{ GUESS_SYMBOLOGY }}", GuessSymbology.ToString().ToLower())
                       .Replace("{{ USB_VENDOR_ID }}", UsbVendorId.HasValue ? UsbVendorId.Value.ToString() : "0")
                       .Replace("{{ USB_PRODUCT_ID }}", UsbProductId.HasValue ? UsbProductId.Value.ToString() : "0");

                this.InitScript = js;
            }
        }


        /// <summary>
        /// USB Vendor ID per filtrare la dialog di selezione porta seriale.
        /// Se null, non viene applicato alcun filtro.
        /// </summary>
        [DefaultValue(null)]
        [Category("Barcode Reader")]
        [Description("USB Vendor ID per filtrare la dialog di selezione porta. Lasciare vuoto per nessun filtro.")]
        public int? UsbVendorId { get; set; } = null;

        /// <summary>
        /// USB Product ID per filtrare la dialog di selezione porta seriale.
        /// Se null, non viene applicato alcun filtro.
        /// </summary>
        [DefaultValue(null)]
        [Category("Barcode Reader")]
        [Description("USB Product ID per filtrare la dialog di selezione porta. Lasciare vuoto per nessun filtro.")]
        public int? UsbProductId { get; set; } = null;


        /// <summary>
        /// BaudRate della porta seriale. 0 = valore predefinito del dispositivo.
        /// </summary>
        [DefaultValue(9600)]
        [Category("Barcode Reader")]
        [Description("BaudRate della porta seriale. 0 = usa il valore predefinito.")]
        public int BaudRate { get; set; } = 9600;

        /// <summary>
        /// Se true, tenta di rilevare automaticamente la simbologia del barcode.
        /// </summary>
        [DefaultValue(false)]
        [Category("Barcode Reader")]
        [Description("Se true, tenta di rilevare automaticamente la simbologia del barcode.")]
        public bool GuessSymbology { get; set; } = false;



        /// <summary>
        /// Se valorizzata, sostituisce il file JS embedded come InitScript del controllo.
        /// Lasciare vuota per usare il JS predefinito.
        /// </summary>
        [DefaultValue("")]
        [Category("Barcode Reader")]
        [Description("JS personalizzato da usare come InitScript. Se vuoto viene usato il JS embedded predefinito.")]
        public string CustomBarcodeReaderJS { get; set; } = "";

        /// <summary>
        /// Legge il contenuto di una risorsa embedded dall'assembly corrente.
        /// </summary>
        private static string LoadEmbeddedScript(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new InvalidOperationException($"Risorsa embedded non trovata: '{resourceName}'");

                using (var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }

        protected override void OnWebRender(dynamic config)
        {
            base.OnWebRender((object)config);
            config.webMethods = new[] { "WebScannerLoaded", "WebScannerUnloaded", "BarcodeRead" };
            //// Passa i parametri al client JS — InitScript li legge tramite this.baudRate / this.guessSymbology
            //config.baudRate = BaudRate;
            //config.guessSymbology = GuessSymbology;
            //config.comPort = COMPort;
        }




        [WebMethod]
        public void WebScannerLoaded(string DeviceID)
        {

            this.DeviceID = DeviceID;
            this.IsConnected = true;
            this.btnBarcodeReader.BackColor = System.Drawing.Color.LightGreen;
            RaiseOnConnect(this.DeviceID);
        }

        [WebMethod]
        public void WebScannerUnloaded(string DeviceID)
        {

            this.DeviceID = DeviceID;
            this.IsConnected = false;
            RaiseOnDisconnect(this.DeviceID);
        }

        [WebMethod]

        public void BarcodeRead(string rawValue)
        {
            var result = AimBarcodeDecoder.Decode(rawValue);

            if (result.IsAimDecoded)
            {
                Console.WriteLine($"Simbologia : {result.Symbology}");
                Console.WriteLine($"Valore     : {result.Value}");
                Console.WriteLine($"AIM Prefix : {result.AimPrefix}");
                if (result.Fnc1.HasValue)
                    Console.WriteLine($"FNC1       : {result.Fnc1}");
                if (result.Model.HasValue)
                    Console.WriteLine($"QR Model   : {result.Model}");
            }
            else
            {
                Console.WriteLine($"Nessun prefisso AIM. Valore grezzo: {result.Value}");
            }

            RaiseOnBarcodeScanned(result.Value, rawValue, result);
        }


        private void btnBarcodeReader_Click(object sender, EventArgs e)
        {

            if (IsConnected == false)
            {


                this.Call("Connect");
            }
            else
            {
                this.Call("Disconnect");
                this.IsConnected = false;
                this.btnBarcodeReader.BackColor = System.Drawing.Color.Red;
                RaiseOnDisconnect(this.DeviceID);


            }

        }

        // Metodo protetto per sollevare l'evento in modo centralizzato

        protected virtual void RaiseOnBarcodeScanned(string barcodeData, string fullbarcodeData, BarcodeDecodeResult barcodeDecodeResult)
        {
            var handler = OnBarcodeScanned;
            if (handler != null)
            {
                handler(this, new BarcodeScannedEventArgs(barcodeData, fullbarcodeData, barcodeDecodeResult));
            }
        }
        // Metodo protetto per sollevare l'evento di disconnessione

        protected virtual void RaiseOnDisconnect(string deviceId)
        {
            var handler = OnDisconnect;
            if (handler != null)
            {
                handler(this, new DisconnectEventArgs(deviceId));
            }
        }


        // Metodo protetto per sollevare l'evento di connessione
        protected virtual void RaiseOnConnect(string deviceId)
        {
            var handler = OnConnect;
            if (handler != null)
            {
                handler(this, new ConnectEventArgs(deviceId));
            }
        }

        // EventArgs che trasporta il dato del barcode
        public class BarcodeScannedEventArgs : EventArgs
        {
            private const char Fnc1Char = (char)29;

            public string BarcodeData { get; }
            public string FullBarcodeData { get; }
            public BarcodeDecodeResult BarcodeDecodeResult { get; }

            /// <summary>
            /// Valore di BarcodeData senza caratteri FNC1 (ASCII 29).
            /// </summary>
            public string BarcodeDataClean =>
                BarcodeData == null ? null : BarcodeData.Replace(Fnc1Char.ToString(), string.Empty);

            /// <summary>
            /// Rappresentazione human-readable GS1 degli Application Identifiers
            /// nel formato (AI)value(AI)value...
            /// Valorizzata solo se BarcodeDecodeResult contiene AI estratti.
            /// </summary>
            public string BarcodeDataGS1
            {
                get
                {
                    if (BarcodeDecodeResult == null || !BarcodeDecodeResult.HasApplicationIdentifiers)
                        return null;

                    var sb = new System.Text.StringBuilder();
                    foreach (var ai in BarcodeDecodeResult.ApplicationIdentifiers)
                        sb.Append($"({ai.AI}){ai.Value}");

                    return sb.ToString();
                }
            }

            public BarcodeScannedEventArgs(string barcodeData, string fullbarcodeData, BarcodeDecodeResult barcodeDecodeResult)
            {
                BarcodeData = barcodeData;
                FullBarcodeData = fullbarcodeData;
                BarcodeDecodeResult = barcodeDecodeResult;
            }
        }
        // EventArgs che trasporta l'id del dispositivo connesso
        public class ConnectEventArgs : EventArgs
        {
            public string DeviceID { get; }

            public ConnectEventArgs(string deviceId)
            {
                DeviceID = deviceId;
            }
        }
        // EventArgs che trasporta l'id del dispositivo disconnesso
        public class DisconnectEventArgs : EventArgs
        {
            public string DeviceID { get; }

            public DisconnectEventArgs(string deviceId)
            {
                DeviceID = deviceId;
            }
        }
    }


}
