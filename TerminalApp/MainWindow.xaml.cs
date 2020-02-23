using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TerminalApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,ISerialCommCallback
    {
        SerialCom serialCom = null;
        
        public MainWindow()
        {
            InitializeComponent();
            serialCom = new SerialCom(this);
            string[] baudrate = { "1200", "4800", "9600", "19200", "38400", "115200", "921600"};
            cmb_baudrate.SelectedItem = "9600";
            foreach (string bdrate in baudrate)
            {
                cmb_baudrate.Items.Add(bdrate);
            }

            string[] ports = serialCom.getAllPortNames();
            foreach (string comport in ports)
            {
                cmb_port.Items.Add(comport);
            }
            cmb_port.SelectedIndex = 0;
        }
        
        private void txtSendBytes_Click(object sender, RoutedEventArgs e)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(txtSendBytes.Text);
            serialCom.sendBytes(bytes);
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e) {
            if (btn_connect.Content.ToString().ToUpper().Equals("CONNECT"))
            {
               
                bool status = serialCom.connect(cmb_port.Text, Int32.Parse(cmb_baudrate.Text), Parity.None, 8, StopBits.One);
                if (status)
                {
                    btn_connect.Content = "Disconnect";
                    cmb_port.IsEnabled = false;
                    cmb_baudrate.IsEnabled = false;
                    btn_scan.IsEnabled = false;
                }
                else { 
                    MessageBox.Show("Issue in connection, other application may used the port");
                }
            }
            else {
                btn_connect.Content = "Connect";
                cmb_port.IsEnabled = true;
                cmb_baudrate.IsEnabled = true;
                btn_scan.IsEnabled = true;
                serialCom.disconnect();
            }
        }

        private void btnScan_Click(object sender, RoutedEventArgs e) {
            string[] ports = serialCom.getAllPortNames();
            cmb_port.Items.Clear();
            foreach (string comport in ports)
            {
                cmb_port.Items.Add(comport);
            }
            cmb_port.SelectedIndex = 0;
        }

        private void btnClearText_Click(object sender, RoutedEventArgs e) {
            lbl_recieveBytes.Text = "";
        }

        public void setData(string data)
        {
            lbl_recieveBytes.Text += data + "\n";
        }
    }
}
