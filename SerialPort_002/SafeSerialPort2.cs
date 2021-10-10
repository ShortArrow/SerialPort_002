﻿using System;
using System.IO.Ports;	//SerialPort
using System.IO;        //Stream
using System.Text;

namespace SerialPort_002 {
    public class SafeSerialPort2 : SerialPort {
        private static Stream openedBaseStream;
        private const string defaultPortName = "";
        private const int baudRate = 9600;
        private const int dataBits = 8;
        private const string newLine = "/r";
        public SafeSerialPort2() : base(defaultPortName, baudRate, Parity.None, dataBits, StopBits.One) {
            base.Handshake = Handshake.None;
            base.Encoding = Encoding.UTF8;
            base.NewLine = newLine;
            openedBaseStream = null;
        }
        
        public new void Open() {
            base.Open();
            openedBaseStream = BaseStream;
        }

        protected override void Dispose(bool disposing) {
            try {
                if (openedBaseStream != null) openedBaseStream.Close();
                openedBaseStream = null;
            }
            catch (UnauthorizedAccessException) { }

            base.Dispose(disposing);
        }
    }
}