!function (e, t) {
    "object" == typeof exports && "undefined" != typeof module
        ? module.exports = t()
        : "function" == typeof define && define.amd
            ? define(t)
            : (e = "undefined" != typeof globalThis ? globalThis : e || self).WebSerialBarcodeScannerCore = t()
}(this, (function () {
    "use strict";

    // ── EventEmitter ────────────────────────────────────────────────────────────
    class e {
        constructor() {
            this._events = {};
        }
        on(e, t) {
            this._events[e] = this._events[e] || [];
            this._events[e].push(t);
        }
        emit(e, ...t) {
            let s = this._events[e];
            s && s.forEach((e => {
                setTimeout((() => e(...t)), 0);
            }));
        }
    }

    // ── WebSerialBarcodeScannerCore ───────────────────────────────────────────────
    // Versione ridotta: gestisce solo la comunicazione seriale.
    // Non esegue nessuna decodifica di simbologia (AIM, GS1, UPC, ecc.).
    // L'evento 'barcode' espone: { value: string, bytes: Uint8Array[] }
    return class {
        #options;  // opzioni porta seriale
        #state;    // stato runtime (emitter, port, reader, timeout)

        constructor(t) {
            this.#options = Object.assign({
                baudRate: 9600,
                bufferSize: 255,
                dataBits: 8,
                flowControl: "none",
                parity: "none",
                stopBits: 1
            }, t);

            this.#state = {
                emitter: new e,
                port: null,
                reader: null,
                timeout: null
            };

            navigator.serial.addEventListener("disconnect", (e => {
                this.#state.port == e.target && this.#state.emitter.emit("disconnected");
            }));
        }

        // Apre il selettore porta e connette
        async connect() {
            try {
                let e = await navigator.serial.requestPort();
                e && await this.open(e);
            } catch (e) {
                console.log("Could not connect! " + e);
            }
        }

        // Riconnette all'ultimo dispositivo noto tramite vendorId/productId
        async reconnect(e) {
            if (!e.vendorId || !e.productId) return;
            let t = (await navigator.serial.getPorts()).filter((t => {
                let s = t.getInfo();
                return s.usbVendorId == e.vendorId && s.usbProductId == e.productId;
            }));
            1 == t.length && await this.open(t[0]);
        }

        // Disconnette e rilascia le risorse
        async disconnect() {
            if (this.#state.port) {
                this.#state.reader.releaseLock();
                await this.#state.port.close();
                this.#state.port = null;
                this.#state.reader = null;
                this.#state.timeout = null;
                this.#state.emitter.emit("disconnected");
            }
        }

        // Apre la porta e avvia il loop di lettura
        async open(e) {
            this.#state.port = e;
            await this.#state.port.open(this.#options);

            let t = this.#state.port.getInfo();
            this.#state.emitter.emit("connected", {
                type: "serial",
                vendorId: t.usbVendorId || null,
                productId: t.usbProductId || null
            });

            let s = [];
            for (; e.readable;) {
                this.#state.reader = e.readable.getReader();
                try {
                    for (; ;) {
                        const { value: e, done: t } = await this.#state.reader.read();
                        if (this.#state.timeout) {
                            clearTimeout(this.#state.timeout);
                            this.#state.timeout = null;
                        }
                        if (t) {
                            this.#state.reader.releaseLock();
                            break;
                        }
                        e && s.push(...e);
                        this.#state.timeout = setTimeout((() => {
                            this.#processFrame(s);
                            s = [];
                        }), 300);
                    }
                } catch (e) {
                    console.log("error", e);
                    s = [];
                }
            }
        }

        // Processa un frame ricevuto: emette 'barcode' con value (stringa grezza) e bytes
        #processFrame(e) {
            let s = {
                value: String.fromCharCode.apply(null, e),
                bytes: [new Uint8Array(e)]
            };

            // Rimuove terminatori di riga
            if (s.value.endsWith("\n")) s.value = s.value.slice(0, -1);
            if (s.value.endsWith("\r")) s.value = s.value.slice(0, -1);

            this.#state.emitter.emit("barcode", s);
        }

        addEventListener(e, t) {
            this.#state.emitter.on(e, t);
        }
    };
}));

// --- Parametri iniettati da C# ---
var scannerOptions = {};
if ({{ BAUD_RATE }} > 0)
scannerOptions.baudRate = {{ BAUD_RATE }};

// Filtri porta seriale (0 = nessun filtro)
var _usbVendorId = {{ USB_VENDOR_ID }};
var _usbProductId = {{ USB_PRODUCT_ID }};
var _portFilters = (_usbVendorId > 0 || _usbProductId > 0)
    ? [{ usbVendorId: _usbVendorId, usbProductId: _usbProductId }]
    : [];

// --- Istanza scanner (per ogni UserControl) ---
const barcodeScanner = new WebSerialBarcodeScannerCore(scannerOptions);

var scannerCtl = this; // this = istanza client di QUESTO UserControl
var lastUsedDevice = null;
var isOpening = false;

// Espone funzioni lato client richiamabili da altri script
this.Connect = function () {
    if (!navigator.serial) {
        console.warn('Web Serial API not supported.');
        return;
    }
    if (isOpening) {
        console.log('Already opening...');
        return;
    }
    isOpening = true;

    if (lastUsedDevice == null) {
        if (_portFilters.length > 0) {
            navigator.serial.requestPort({ filters: _portFilters })
                .then(port => barcodeScanner.open(port))
                .catch(err => {
                    console.error('connect error:', err);
                    isOpening = false;
                });
        } else {
            barcodeScanner.connect().catch(err => {
                console.error('connect error:', err);
                isOpening = false;
            });
        }
    } else {
        barcodeScanner.reconnect(lastUsedDevice).catch(err => {
            console.error('reconnect error:', err);
            isOpening = false;
        });
    }
};

this.Disconnect = function () {
    barcodeScanner.disconnect().catch(err => console.error('disconnect error:', err));
    lastUsedDevice = null;
    isOpening = false;
};

// Collega al click del controllo
scannerCtl.onClick = function () {
    scannerCtl.Connect();
};

// --- Eventi dello scanner ---
barcodeScanner.addEventListener('connected', device => {
    const logtxt = (device.vendorId ?? '?') + '.' + (device.productId ?? '?');
    console.log('Connected to device ' + logtxt);

    lastUsedDevice = device;
    isOpening = false;

    scannerCtl.WebScannerLoaded(JSON.stringify(device), function (result, error) {
        if (error) console.error('WebScannerLoaded error:', error);
        else console.log('WebScannerLoaded ok');
    });
});

barcodeScanner.addEventListener('disconnected', () => {
    console.log('Disconnected');

    scannerCtl.WebScannerUnloaded(JSON.stringify(lastUsedDevice), function (result, error) {
        if (error) console.error('WebScannerUnloaded error:', error);
        else console.log('WebScannerUnloaded ok');
    });
    lastUsedDevice = null;
    isOpening = false;
});

barcodeScanner.addEventListener('barcode', e => {
    // Sostituisce il separatore FNC1 (GS, ASCII 29) con 'é' (0xE9) prima del
    // trasporto verso C# tramite Wisej: il bridge JSON corrompe \u001d
    // perdendo il backslash, rendendo il separatore invisibile al parser C#.
    // 'é' viene correttamente normalizzato da AimBarcodeDecoder / Gs1AiParser.
    const safeValue = e.value.replace(/\x1d/g, '\xe9');

    scannerCtl.BarcodeRead(safeValue, function (result, error) {
        if (error) console.error('BarcodeRead error:', error);
        else console.log('BarcodeRead ok =>', e.value);
    });

    console.log('BARCODE:', e.value);
});