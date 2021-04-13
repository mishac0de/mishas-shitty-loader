using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mishas_shitty_loader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.favicon;
            textBox1.MaxLength = 50;
            this.Text = RandomString(8);
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        Point new_point;
        private void close_btn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimize_btn_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            new_point = new Point(e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - new_point.X;
                this.Top += e.Y - new_point.Y;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "enter key")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.ForeColor = Color.Silver;
                textBox1.Text = "enter key";
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            if (this.ClientRectangle.Width > 0 && this.ClientRectangle.Height > 0)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,
                                                                 Color.FromArgb(69, 69, 128),
                                                                  Color.FromArgb(42, 42, 69),
                                                                  45F))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var target = Process.GetProcessesByName("csgo").FirstOrDefault();
            if (target == null)
            {
                MessageBox.Show("open csgo.exe!");
                Application.Exit();
            }
            if (textBox1.Text != "enter key" && textBox1.Text != "")
            {
                //auth (pastebin w/passwords), you can add some shitty encryption too for sum beautiful security through obscurity
                WebRequest request = WebRequest.Create("pastebin.com/raw/...");
                WebResponse response = request.GetResponse();
                System.IO.StreamReader reader = new
                    System.IO.StreamReader(response.GetResponseStream());

                //reads one line from the raw pastebin
                string passauth = reader.ReadLine();

                if (textBox1.Text == passauth)
                {
                    MessageBox.Show("logged in! injecting...");
                    timer1.Start();
                }
            }
            else
            {
                MessageBox.Show("enter credentials!");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        { 
            //dll path
            string path = "C:\\test\\cheat.dll";
            var target = Process.GetProcessesByName("csgo").FirstOrDefault();
            WebClient wb = new WebClient();
            //this pastebin should have a direct download link
            string pastebin = "pastebin.com/raw/...";
            string rcv = wb.DownloadString(pastebin);
            WebClient wc = new WebClient();

            //creates folder
            DirectoryInfo pep = System.IO.Directory.CreateDirectory("C:\\test");

            //downloads the cheat to path
            wc.DownloadFile(rcv, path);

            //injection (https://github.com/Dewera/Lunar)
            var file = File.ReadAllBytes(path);
            var injector = new Lunar.Injection.ManualMapInjector(target) { AsyncInjection = true };
            injector.Inject(file).ToInt32();

            /*File.Delete(path); Directory.Delete("C:\\test");s*/
            timer1.Stop();
        }
    }
}
