using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Nowe_Odbieranie
{
    public partial class Form1 : Form
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
        public Form1()
        {
            InitializeComponent();
            label1.Text = label1.Text + GetLocalIPAddress();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                short port = short.Parse(textBox1.Text);
                new Form2(port).Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Błędny port");
            }
        }
    }
}
