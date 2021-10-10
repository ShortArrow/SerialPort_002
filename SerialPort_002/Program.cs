using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Diagnostics;

namespace SerialPort_002 {
    class Program {
        bool readingnow = false;
        string receivedData = "null";
        private bool isRunning_ = false;
        private string message_;
        private bool isNewMessageReceived_ = false;

        static void Main(string[] args) {
            

            Console.WriteLine("\tMarshalByRefObject Class Test: " +
                              "Default AppDomain <- Test Domain");
            {
                var info = AppDomain.CurrentDomain.SetupInformation;
                var testDomain = AppDomain.CreateDomain("Test Domain", null, info);
                var t = typeof(SerialPortAppDomain);
                var o = (SerialPortAppDomain)testDomain.CreateInstanceAndUnwrap(t.Assembly.FullName, t.FullName);
                o.Open();
                o.OnDataReceived += O_OnDataReceived;
                //testDomain.DoCallBack(new CrossAppDomainDelegate(o.Open()));
                //AppDomain.Unload(testDomain);
            }
            Console.WriteLine();
            Console.Write("\tPush Any Key\n\t>>");
            Console.ReadKey();
        }

        private static void O_OnDataReceived(string ReceivedData) {
                try {
                    var data = ReceivedData.Split(new string[] { "E" }, System.StringSplitOptions.None);
                    if (data.Length < 2) return;
                    //Debug.Log(data[0]);
                    Debug.WriteLine(data[0]);
                }
                catch (Exception ex) {
                    Debug.WriteLine(ex.ToString());
                }
        }
    }
}