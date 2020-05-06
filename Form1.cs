using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_KeyDown(object sender, KeyEventArgs args)
        {
            CaptureScreen();
            var bmp = (Bitmap)Image.FromFile("TestColor.png");
            var white = (Bitmap)Image.FromFile("white.png");
            Color pixelColor = bmp.GetPixel(0, 0);
            Color whiteColor = white.GetPixel(0, 0);
            //white
            //if (args.KeyValue.ToString() == "116")
            //{
            //    Point pointToScreen = PointToScreen(System.Windows.Forms.Cursor.Position);
            //    Program.FirstPos = pointToScreen;
            //    System.Media.SystemSounds.Asterisk.Play();
            //}
            //else if (args.KeyValue.ToString() == "117")
            //{
            //    Point pointToScreen = PointToScreen(System.Windows.Forms.Cursor.Position);
            //    Program.SecondPos = pointToScreen;
            //    System.Media.SystemSounds.Asterisk.Play();
            //    CaptureScreen();
            //}
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
                captureBitmap.Save(@"teszt.jpg", ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static RectangleF GetRectangle(PointF p1, PointF p2)
        {
            float top = Math.Min(p1.Y, p2.Y);
            float bottom = Math.Max(p1.Y, p2.Y);
            float left = Math.Min(p1.X, p2.X);
            float right = Math.Max(p1.X, p2.X);

            RectangleF rect = RectangleF.FromLTRB(left, top, right, bottom);

            return rect;
        }
    }
}
