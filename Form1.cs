using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        bool start = false;
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
            timer1.Interval = 650;
            timer1.Enabled = true;
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
            label1.Text += curent.X + ", ";
            label3.Text += curent.Y + ", ";

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
            start = !start;
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

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (num >4)
            {
                CalculationAndDo();
            }
            else
            {
                if (start == true)
                {
                    var moc = new MockPoint(num);
                    Dictionary<Point, Color> dic = new Dictionary<Point, Color>();
                    var hashSet = new HashSet<Color>();
                    foreach (var item in moc.lst)
                    {
                        var point = item;
                        var color = GetColorAt(point);
                        dic.Add(point, color);
                    }
                    var dicDistin = dic.Values.Distinct().ToList();
                    foreach (var item in dicDistin)
                    {
                        if (dic.Values.Where(_ => _ == item).Count() == 1)
                        {
                            var find = dic.FirstOrDefault(_ => _.Value == item);
                            MouseOperations.SetCursorPosition(find.Key.X, find.Key.Y);
                            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
                            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
                            num++;
                            label1.Text = num.ToString();
                            break;
                        }
                    }
                }
            }
        }

        private void CalculationAndDo()
        {
            if (start == true)
            {
                var flgNextCanbeChooseColor = false;
                var moc = new MockPoint(num);
                Color colorCompare = Color.Black;
                Dictionary<Point, Color> dic = new Dictionary<Point, Color>();
                var hashSet = new HashSet<Color>();
                foreach (var item in moc.lst)
                {
                    var point = item;
                    var color = GetColorAt(point);
                    if (flgNextCanbeChooseColor && colorCompare != color)
                    {
                        MouseOperations.SetCursorPosition(point.X, point.Y);
                        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
                        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
                        num++;
                        label1.Text = num.ToString();
                        break;
                    }
                    if (!dic.Values.Contains(color))
                    {
                        dic.Add(point, color);
                    }
                    else // chua
                    {
                        if (dic.Count() == 1)
                        {
                            colorCompare = dic.Values.First();
                            flgNextCanbeChooseColor = true;
                        }
                        else if(dic.Count() == 2) // 2 mau khac nhau, them 1 mau moi
                        {
                            var cl1 = dic.Values.First();
                            if( cl1 == color)
                            {
                                var find = dic.Keys.Last();
                                MouseOperations.SetCursorPosition(find.X, find.Y);
                                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
                                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
                                num++;
                                label1.Text = num.ToString();
                                break;
                            }
                            else
                            {
                                var find = dic.Keys.First();
                                MouseOperations.SetCursorPosition(find.X, find.Y);
                                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
                                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
                                num++;
                                label1.Text = num.ToString();
                                break;
                            }
                        }
                    }

                }
            }
        }
    }
}
