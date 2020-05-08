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
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        static int num = 1;
        // this make window can alway on top
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        //
        public Form1()
        {
            InitializeComponent();
        }
        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        public Color GetColorAt(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }
        private void button1_KeyDown(object sender, KeyEventArgs args)
        {
            var curent = System.Windows.Forms.Cursor.Position;
            label1.Text = curent.X + ", " + curent.Y;
            num++;
            label2.Text = num.ToString();
            try
            {

                var arrX = new List<int>();
                var arrY = new List<int>();
                if (num <= 4)
                {
                    arrX = new List<int> { 636, 728 };
                    arrY = new List<int> { 448, 540 };
                }
                else if (num <= 10)
                {
                    arrX = new List<int> { 591, 687, 767 };
                    arrY = new List<int> { 400, 473, 557 };
                }
                else if (num <= 20)
                {
                    arrX = new List<int> { 579, 650, 721, 782 };
                    arrY = new List<int> { 386, 459, 529, 549 };
                }
                else if (num <= 30)
                {
                    arrX = new List<int> { 579, 650, 721, 782 };
                    arrY = new List<int> { 386, 459, 529, 549 };
                }


                Dictionary<int[], Color> dic = new Dictionary<int[], Color>();
                foreach (var x in arrX)
                {
                    foreach (var y in arrY)
                    {
                        var point = new Point(x, y);
                        var color = GetColorAt(point);
                        var ar = new int[] { x, y };
                        dic.Add(ar, color);
                    }
                }
                var dicDistin = dic.Values.Distinct();
                foreach (var item in dicDistin)
                {
                    if (dic.Values.Where(_ => _ == item).Count() == 1)
                    {
                        var find = dic.FirstOrDefault(_ => _.Value == item);
                        MouseOperations.SetCursorPosition(find.Key[0], find.Key[1]);
                        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
                        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
                        Point location = button1.PointToScreen(Point.Empty);
                        //MouseOperations.SetCursorPosition(location.X + 10, location.Y + 10);
                    }
                }
            }
            catch
            {

            }
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
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }

        private void btResetLevel_Click(object sender, EventArgs e)
        {
            num = 1;
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            this.Activate();
        }
    }
}
