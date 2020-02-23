using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TerminalApp
{
    /// <summary>
    /// Serial communication class 
    /// </summary>
    class SerialCom
    {
        SerialPort port;
        ISerialCommCallback iSerialCommCallback;
        private delegate void SetTextDeleg(string text);
        MainWindow context;

        /// <summary>
        /// Assign values in iSerialCommCallback and context 
        /// </summary>
        /// <param name="mContext"></param>
        public SerialCom(MainWindow mContext)
        {
            iSerialCommCallback= (ISerialCommCallback)mContext;
            context = mContext;
        }
        /// <summary>
        /// Connect with serial port, define below parameters to estabilish communication
        /// Register the SerialDataReceivedEventHandler that will handle DataRecieved event of serialPort object 
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudrate"></param>
        /// <param name="parity"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBit"></param>
        /// <returns></returns>
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

        /// <summary>
        ///  Disconnect port when communication is done
        /// </summary>
        public void disconnect()
        {
            if (port != null && port.IsOpen)
            {
                port.Close();
            }
        }

        /// <summary>
        /// This method is called when data recieved on Serial port
        /// Call Dispatcher.Invoke to display recieved data on UI 
        /// serialComm_DataReceived is method to which data is passed to display it on UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void serialPort_DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            //Thread.Sleep(100);
            try
            {
                string data = port.ReadExisting();
                context.Dispatcher.Invoke(new SetTextDeleg(serialComm_DataReceived), new object[] { data });
            }
            catch (Exception ex)
            {
            }

        }

        /// <summary>
        /// Get list of all available port names
        /// </summary>
        /// <returns></returns>
        public string[] getAllPortNames() {
            return SerialPort.GetPortNames();
        } 


        /// <summary>
        /// This method will pass data to ISerialCommCallback to Display recieved datya on UI as UI elements are not accissble in this class
        /// </summary>
        /// <param name="data"></param>
        private void serialComm_DataReceived(string data)
        {
            if (port != null)
            {
                var bytes = Encoding.UTF8.GetBytes(data);
                var hexString = BitConverter.ToString(bytes).Replace("-","");
                iSerialCommCallback.setData(hexString);
            }
        }

        /// <summary>
        /// This method is to send bytes over open port
        /// </summary>
        /// <param name="bytes"></param>
        public void sendBytes(byte[] bytes) {
            try
            {
                port.Write(bytes, 0, bytes.Length);
            }

            catch (Exception ex) { }
        }

    }
}
