﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Interop;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;

namespace Projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Socket client;
        private BitmapSource CopyScreen()
        {
            using (var screenBmp = new Bitmap(
                (int)SystemParameters.PrimaryScreenWidth,
                (int)SystemParameters.PrimaryScreenHeight,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (var bmpGraphics = Graphics.FromImage(screenBmp))
                {
                    bmpGraphics.CopyFromScreen(0, 0, 0, 0, screenBmp.Size);
                    return Imaging.CreateBitmapSourceFromHBitmap(
                        screenBmp.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
            }
        }
        //C:\Users\Paweu\Desktop
        public static void SaveClipboardImageToFile(string filePath, BitmapSource bmp1)
        {
            BitmapSource image = bmp1;
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }
        public void przesyl()
        {
          //  while (true)
         //   {
                BitmapSource bmp = CopyScreen();
                bmp.Freeze();
                Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new ThreadStart(delegate
                {
                    this.image.Source = bmp;
                }));

                SaveClipboardImageToFile("C:\\Users\\Paweu\\Desktop\\1.jpg", bmp);
                int sent;
                Bitmap bmp1 = new Bitmap("C:\\Users\\Paweu\\Desktop\\1.jpg");

                MemoryStream ms = new MemoryStream();
                // Save to memory using the Jpeg format
                bmp1.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                // read to end
                byte[] bmpBytes = ms.ToArray();
                bmp1.Dispose();
                ms.Close();

                sent = SendVarData(client, bmpBytes);
         //   }
        }

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

        private static int SendVarData(Socket s, byte[] data)
        {
            int total = 0;
            int size = data.Length;
            int dataleft = size;
            int sent;

            byte[] datasize = new byte[4];
            datasize = BitConverter.GetBytes(size);
            sent = s.Send(datasize);

            while (total < size)
            {
                sent = s.Send(data, total, dataleft, SocketFlags.None);
                total += sent;
                dataleft -= sent;
            }
            return total;
        }

        string ip_odbiorcy = GetLocalIPAddress();
        string ip_nadawcy = GetLocalIPAddress();
        //Thread t;
        public MainWindow()
        {
            InitializeComponent();
            label5.Content = "ADRES IP: " + GetLocalIPAddress();

        }

        byte[] data = new byte[10000];
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);

        Socket newsock = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

        
        bool dupy = false;

        private void button_Click_1(object sender, RoutedEventArgs e)
        {

            if (dupy == false)
            { 
                newsock.Bind(ipep);
                dupy = true;
            }
            newsock.Listen(60);
            Console.WriteLine("Waiting for a client...");

            client = newsock.Accept();
            IPEndPoint newclient = (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("Connected with {0} at port {1}",
                            newclient.Address, newclient.Port);
            Thread thread = new Thread(new ThreadStart(przesyl));
            thread.Start();
            
            

        }



        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }



        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != "")
            {
                ip_nadawcy = textBox.Text;
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
