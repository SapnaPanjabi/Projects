using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TerminalApp
{
    class SerialCom
    {
        SerialPort port;
        ISerialCommCallback iSerialCommCallback;
        private delegate void SetTextDeleg(string text);
        MainWindow context;

        public SerialCom(MainWindow mContext)
        {
            iSerialCommCallback= (ISerialCommCallback)mContext;
            context = mContext;
        }
        void serialPort_DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(10);
            try
            {
                string data = port.ReadExisting();
                context.Dispatcher.Invoke(new SetTextDeleg(si_DataReceived), new object[] { data });
            }
            catch (Exception ex)
            {
            }

        }

        public string[] getAllPortNames() {
            return SerialPort.GetPortNames();
        } 


        private void si_DataReceived(string data)
        {
            if (port != null)
            {
                var bytes = Encoding.UTF8.GetBytes(data);
                var hexString = BitConverter.ToString(bytes).Replace("-","");
                iSerialCommCallback.setData(hexString);
            }
        }

        public void sendBytes(byte[] bytes) {
            try
            {
                port.Write(bytes, 0, bytes.Length);
            }

            catch (Exception ex) { }
        }

        public bool connect(string portName, int baudrate, Parity parity, int dataBits, StopBits stopBit)
        {
            port = new SerialPort(portName, baudrate, parity, dataBits, stopBit);
            port.Encoding = System.Text.Encoding.UTF8;
            port.Handshake = Handshake.None;
            port.ReadTimeout = 10000;

            if (port != null)
            {
                try
                {
                    if (!port.IsOpen)
                    {
                        port.Open();
                    }
                    port.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataRecieved);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }

        public void disconnect()
        {
            if (port != null && port.IsOpen)
            {
                port.Close();
            }
        }
    }
}
