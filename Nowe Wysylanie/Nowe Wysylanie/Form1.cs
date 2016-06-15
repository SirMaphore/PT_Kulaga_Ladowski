using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Drawing.Imaging;
using System.Runtime.Serialization.Formatters.Binary;

namespace Nowe_Wysylanie
{
    public partial class Form1 : Form
    {
        private TcpClient receiving = new TcpClient();
        private NetworkStream data;
        short port;

        private Image getImage()
        {
            Rectangle rect = Screen.PrimaryScreen.Bounds; //rozmiar obrazu ekranu
            Bitmap obraz = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb); //24 bity na pixel * wysokosc * szerokosc
            Graphics g = Graphics.FromImage(obraz);
            g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);//rysowanie

            /*Bitmap output = new Bitmap(683, 384);
            using (Graphics gs = Graphics.FromImage(output))
            {
                gs.DrawImage(obraz, 0, 0, 683, 384);
            }
            

            Bitmap screenshot = null;

            foreach (Screen screen in Screen.AllScreens)
            {
                screenshot = new Bitmap(screen.Bounds.Width,
                    screen.Bounds.Height,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                // Create a graphics object from the bitmap
                Graphics gfxScreenshot = Graphics.FromImage(screenshot);
                // Take the screenshot from the upper left corner to the right bottom corner
                gfxScreenshot.CopyFromScreen(
                    screen.Bounds.X,
                    screen.Bounds.Y,
                    0,
                    0,
                    screen.Bounds.Size,
                    CopyPixelOperation.SourceCopy);
                // Save the screenshot
            }*/


            return obraz;
        }

        private void send()
        {
            BinaryFormatter bf = new BinaryFormatter();
            data = receiving.GetStream();
            bf.Serialize(data, getImage());//zmiana obrazu na bity
        }
        public Form1()
        {
            InitializeComponent();       
        }

        private void button1_Click(object sender, EventArgs e) //polaczenie
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {                
                try
                {
                    port = short.Parse(textBox2.Text);
                    receiving.Connect(textBox1.Text, port);
                    MessageBox.Show("Połączono!");
                }
                catch (Exception)
                {
                    MessageBox.Show("Nie udało się połączyć");
                }
            }
            else
            {
                MessageBox.Show("Podaj adres IP i port");
            }
        }

        private void button2_Click(object sender, EventArgs e) //wyslij obraz
        {
            if (button2.Text.StartsWith("Wys"))
            {
                timer1.Start();
                button2.Text = "Przerwij wysyłanie";
            }
            else
            {
                timer1.Stop();
                button2.Text = "Wysyłaj obraz";
            }
            if (radioButton1.Checked)
                timer1.Interval = 65;
            if (radioButton2.Checked)
                timer1.Interval = 32;
            if (radioButton3.Checked)
                timer1.Interval = 16;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                send();
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd wysyłania");
            }
        }
    }
}
