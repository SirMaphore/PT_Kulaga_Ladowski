using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace Nowe_Odbieranie
{
    public partial class Form2 : Form
    {
        public short port;
        private TcpClient sender;
        private TcpListener listener;
        private Thread listening;
        private Thread receiving;
        private NetworkStream data;
        private void startListening()
        {
            while (!sender.Connected)
            {
                listener.Start();
                sender = listener.AcceptTcpClient();
            }
            receiving.Start();
        }
        private void stopListening()
        {
            listener.Stop();
            sender = null;
            if (listening.IsAlive)
                listening.Abort();
            if (receiving.IsAlive)
                receiving.Abort();
        }
        private void startReceiving()
        {
            BinaryFormatter bf = new BinaryFormatter();
            while (sender.Connected)
            {
                data = sender.GetStream();
                pictureBox1.Image = (Image)bf.Deserialize(data);
            }
        }
        public Form2(short Port)
        {
            port = Port;
            sender = new TcpClient();
            listening = new Thread(startListening);
            receiving = new Thread(startReceiving);
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            listener = new TcpListener(IPAddress.Any, port);
            listening.Start();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            stopListening();
        }
    }
}
