using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Management;

namespace HisDemo.UI
{
    public class MCABarcodeReader : Component
    {
        public MCABarcodeReader()
        {

        }

        public MCABarcodeReader(IContainer container) : this()
        {
            container.Add(this);
        }

        /// <summary>
        /// Searches for serial ports which have a known scanner type attaced.
        /// </summary>
        /// <returns>The COM-Port ID selected.</returns>
        protected string FindAutoSerialPort()
        {
            // Get WMI Object information for Serial Ports
            using (var searcher = new ManagementObjectSearcher(@"SELECT DeviceID,PNPDeviceID FROM Win32_SerialPort"))
            {
                var collection = searcher.Get();
                foreach(var device in collection)
                {
                    // The COM Port name
                    var port = (string)device.GetPropertyValue("DeviceID");
                    // The Plug-n-Play expanded Name (contains USB VID/PID etc.)
                    var pnp = device.GetPropertyValue("PNPDeviceID");
                    if (pnp != null)
                    {
                        string pnpId = pnp.ToString();
                        // Zebra scanner in CDC mode ends with USB_CDC_SYMBOL_SCANNER:
                        if (pnpId.Contains("USB_CDC_SYMBOL_SCANNER"))
                            return port;
                    }
                }
            }
            return null;
        }

        public bool Connect()
        {
            Disconnect();
            try
            {
                serialPort = new SerialPort();
                serialPort.PortName = FindAutoSerialPort();
                serialPort.ReadTimeout = 1;
                serialPort.ReceivedBytesThreshold = 2;
                serialPort.Encoding = Encoding.ASCII;
                serialPort.NewLine = "\r\n";

                serialPort.DataReceived += SerialPort_DataReceived;

                serialPort.Open();
                if (!serialPort.IsOpen)
                {
                    serialPort.Dispose();
                    return false;
                }
                return true;
            } catch
            {
                return false;
            }
        }

        public class QRCodeFilterEventArgs : EventArgs
        {
            public string QRData;
        }

        public delegate void BarcodeReceivedHandler(object mcaBarcodeId);
        public delegate void BarcodeInvalidHandler(string rawData);

        public event BarcodeReceivedHandler BarcodeReceived;
        public event BarcodeInvalidHandler BarcodeInvalid;
        public event EventHandler<QRCodeFilterEventArgs> QRCodeFilter;

        public bool AcceptData = true;

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!AcceptData)
            {
                // Clear out any queued up data (scanning while blocked thread)
                serialPort.DiscardInBuffer();
                return;
            }
            MCABarcodeID id = null;
            string raw = null;
            try
            {
                raw = serialPort.ReadLine();
                // Handle AIM Code (Zebra Scanner)
                if (raw.Length > 3 && raw[0] == ']')
                {
                    // ]cm -> c=Code type, m=Modifier
                    // Codes: A=Code39, C=Code128, d=DataMatrix, Q=QRCode...
                    // Modifiers for Q: 0=Mode1, 2=Mode2/Micro, 3=Mode2withECI, (4..6)=Mode 2 Variants
                    char AIMCode = raw[1];
                    char AIMModifier = raw[2];

                    // For now, only allow QR Codes
                    if (AIMCode == 'Q')
                    {
                        string qrData = raw.Substring(3).TrimEnd('\r', '\n');
                        var ea = new QRCodeFilterEventArgs()
                        {
                            QRData = qrData
                        };
                        QRCodeFilter?.Invoke(this, ea);
                        id = new MCABarcodeID(ea.QRData);
                    }
                }
            } 
            catch (MCABarcodeID.InvalidMCABarcodeIDException)
            {
            }
            catch (TimeoutException)
            {
                // Ignore very unlikely but possible I/O errors on serial stream
                return;
            }
            if (id == null)
            {
                BarcodeInvalid?.Invoke(raw);
            }
            else
            {
                BarcodeReceived?.Invoke(id);
            }
            // Clear out any queued up data (scanning while blocked thread)
            serialPort.DiscardInBuffer();
        }

        private static readonly byte[] beepSequenceBels = { 7, 7, 7, 7, 7, 7, 7, 7, 7, 7 };
        /// <summary>
        /// Make the scanner beep (if possible).
        /// </summary>
        /// <param name="count">Number of beeps to send, limited to 10</param>
        /// <returns>True if sucessfully sent beeps.</returns>
        public bool Beep(int count = 1)
        {
            if (count > beepSequenceBels.Length) 
                count = beepSequenceBels.Length;
            if (count < 1)
                return false;

            try
            {
                serialPort.Write(beepSequenceBels, 0, count);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Disconnect()
        {
            if (serialPort != null)
            {
                serialPort.Dispose();
                serialPort = null;
            }
        }

        protected SerialPort serialPort = null;

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disconnect();
            }
            base.Dispose(disposing);
        }
    }
}
