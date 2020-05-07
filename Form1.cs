using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        static int num = 0;
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_KeyDown(object sender, KeyEventArgs args)
        {
            var curent = System.Windows.Forms.Cursor.Position;
            label1.Text = curent.X + ", " + curent.Y;
        }

        public void CaptureScreen()
        {
            try
            {
                Bitmap captureBitmap = new Bitmap(430, 430, PixelFormat.Format32bppArgb);
                //Creating a New Graphics Object
                Graphics captureGraphics = Graphics.FromImage(captureBitmap);
                //Copying Image from The Screen

                captureGraphics.CopyFromScreen(750, 480, 0, 0, Screen.PrimaryScreen.Bounds.Size,
                            CopyPixelOperation.SourceCopy);
                //Saving the Image File (I am here Saving it in My E drive).
                captureBitmap.Save(@"teszt" + num + ".jpg", ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            num++;
            try
            {
                var arr = new List<int>();
                if (num < 4)
                {
                    arr = new List<int> { 150, 280 };
                }
                else if (num < 10)
                {
                    arr = new List<int> { 100, 200, 300 };
                }
                else if (num < 20)
                {
                    arr = new List<int> { 50, 150, 250, 350 };
                }
                else if (num < 30)
                {
                    arr = new List<int> { 50, 125, 200, 275, 350 };
                }
                CaptureScreen();
                var bmp = (Bitmap)Image.FromFile(@"teszt" + num + ".jpg");
                Color pixelColor = bmp.GetPixel(0, 0);
                Dictionary<int[], Color> dic = new Dictionary<int[], Color>();
                foreach (var x in arr)
                {
                    foreach (var y in arr)
                    {
                        var ar = new int[] { x, y };
                        dic.Add(ar, bmp.GetPixel(x, y));
                    }
                }
                var dicDistin = dic.Values.Distinct();
                foreach (var item in dicDistin)
                {
                    if (dic.Values.Where(_ => _ == item).Count() == 1)
                    {
                        var find = dic.FirstOrDefault(_ => _.Value == item);
                        MouseOperations.SetCursorPosition(720, 502);
                        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
                        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
                        Point location = button1.PointToScreen(Point.Empty);

                        Process p = Process.GetCurrentProcess();
                        string name = p.ProcessName;
                        IntPtr hWnd = MouseOperations.FindWindow("From1", null);
                        if (hWnd != IntPtr.Zero)
                        {
                            MouseOperations.ShowWindow(hWnd, 500);
                            MouseOperations.SetForegroundWindow(hWnd);
                        }

                            MouseOperations.SetCursorPosition(location.X+10, location.Y+20);
                        for (int i = find.Key[0]; i < find.Key[0] + 10; i++)
                        {
                            for (int j = find.Key[1]; j < find.Key[1] + 10; j++)
                            {
                                bmp.SetPixel(i, j, Color.White);
                                
                            }
                        }
                    }
                }

                pictureBox1.Image = null;
                bmp.Save("recolor" + num + ".png", ImageFormat.Png);
                pictureBox1.Image = Image.FromFile("recolor" + num + ".png");
                pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            }
            catch
            {

            }
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            this.Activate();
        }
    }
}
