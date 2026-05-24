using System;

namespace Passero.Framework.Controls

{
    /// <summary>
    /// Event bus statico per propagare gli eventi del BarcodeReader.
    /// Supporta la modalità esclusiva: solo il listener registrato come
    /// esclusivo riceve l'evento; se nessuno è esclusivo l'evento è broadcast.
    /// </summary>
    public static class SerialBarcodeReaderHub
    {
        /// <summary>
        /// Evento broadcast: ricevuto da tutti i sottoscrittori,
        /// solo se nessun listener esclusivo è attivo.
        /// </summary>
        public static event EventHandler<SerialBarcodeReader.BarcodeScannedEventArgs> BarcodeScanned;

        // Handler esclusivo corrente (null = nessuno)
        private static EventHandler<SerialBarcodeReader.BarcodeScannedEventArgs> _exclusiveHandler;

        /// <summary>
        /// Acquisisce il diritto esclusivo di ricevere l'evento.
        /// Sostituisce qualsiasi lock esclusivo precedente.
        /// </summary>
        public static void AcquireExclusive(EventHandler<SerialBarcodeReader.BarcodeScannedEventArgs> handler)
        {
            _exclusiveHandler = handler;
        }

        /// <summary>
        /// Rilascia il lock esclusivo. Se il handler passato non corrisponde
        /// a quello attivo, la chiamata è ignorata.
        /// </summary>
        public static void ReleaseExclusive(EventHandler<SerialBarcodeReader.BarcodeScannedEventArgs> handler)
        {
            if (_exclusiveHandler == handler)
                _exclusiveHandler = null;
        }

        /// <summary>
        /// Chiamato da chi ospita il controllo per propagare l'evento.
        /// </summary>
        public static void RaiseBarcodeScanned(object sender, SerialBarcodeReader.BarcodeScannedEventArgs e)
        {
            if (_exclusiveHandler != null)
                // Modalità esclusiva: solo il listener registrato riceve l'evento
                _exclusiveHandler.Invoke(sender, e);
            else
                // Modalità broadcast: tutti i sottoscrittori ricevono l'evento
                BarcodeScanned?.Invoke(sender, e);
        }
    }
}