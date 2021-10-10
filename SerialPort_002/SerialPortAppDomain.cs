using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using System.Runtime.Remoting;
using System.Diagnostics;

namespace SerialPort_002 {
    public class SerialPortAppDomain : MarshalByRefObject {
        public delegate void SerialDataReceivedEventHandler(string message);
        public event SerialDataReceivedEventHandler OnDataReceived;
        private SerialPort serialPort;
        public string portName = "COM4";
        public bool isOpen
        {
            get
            {
                return isRunning;
            }
        }
        private Thread thread_;
        private bool isRunning = false;
        private string message;
        private bool isNewMessageReceived = false;

        private void initialize() {
            serialPort = new SafeSerialPort2();
            serialPort.PortName = portName;
        }
        private void Update() {
            while (true) {
                if (isNewMessageReceived) {
                    OnDataReceived(message);
                }
            }
        }
        private void OnDestroy() {
            Close();
        }
        public void Open() {

            if (serialPort == null) {
                initialize();
                try {
                    serialPort.Open();
                }
                catch (Exception ex) {
                    Debug.WriteLine(ex.ToString());
                }
                if (serialPort.IsOpen == true) {
                    isRunning = true;
                    thread_ = new Thread(Read);
                    thread_.Start();
                }
                else {
                    Disposer();
                    Debug.WriteLine("Open Denied");
                }
            }
            else {
                Disposer();
                Debug.WriteLine("Already Open!!");
            }
        }
        public void Close() {
            if (isRunning == true) {
                isRunning = false;
            }
            else {
                Debug.WriteLine("does not running");
            }
        }
        private void Disposer() {
            if (serialPort != null) {
                try {
                    serialPort.Dispose();
                    serialPort = null;
                }
                catch (Exception ex) {
                    Debug.WriteLine(ex.ToString());
                }
            }
            else {
                Debug.WriteLine("Already Disposed!!");
            }
        }
        private void Read() {
            while (isRunning == true && serialPort.BreakState == false) {
                try {
                    if (serialPort.ReadBufferSize > 0) {
                        message = serialPort.ReadLine();
                        isNewMessageReceived = true;
                    }
                }
                catch (Exception ex) {
                    Debug.WriteLine(ex.Message);
                }
            }
            isNewMessageReceived = false;
            Disposer();
            Debug.WriteLine("end read");
        }

    }
}