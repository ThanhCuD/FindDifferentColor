using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class MockPoint
    {
        public List<Point> lst;
        private List<int> lst2x2X = new List<int> { 636, 728 };
        private List<int> lst2x2Y = new List<int> { 448, 540 };
        private List<int> lst3x3X = new List<int> { 590, 686, 772 };
        private List<int> lst3x3Y = new List<int> { 400, 500, 600 };
        private List<int> lst4x4X = new List<int> { 576, 647, 716, 787 };
        private List<int> lst4x4Y = new List<int> { 389, 460, 526, 600 };
        private List<int> lst5X = new List<int> { 566, 623, 683, 737, 786 };
        private List<int> lst5Y = new List<int> { 386, 440, 496, 554, 605 };
        private List<int> lst6X = new List<int> { 575, 605, 650, 698, 741, 791 };
        private List<int> lst6Y = new List<int> { 387, 427, 471, 516, 566, 611 };
        private List<int> lst7X = new List<int> { 567, 595, 633, 675, 715, 754, 782 };
        private List<int> lst7Y = new List<int> { 385, 417, 455, 495, 533, 574, 606 };
        public MockPoint(int num)
        {
            lst = new List<Point>();
            var arr1 = new List<int>();
            var arr2 = new List<int>();
            if (num <4)
            {
                arr1 = lst2x2X;
                arr2 = lst2x2Y;
            }
            else if (num <= 10)
            {
                arr1 = lst3x3X;
                arr2 = lst3x3Y;
            }
            else if (num <= 20)
            {
                arr1 = lst4x4X;
                arr2 = lst4x4Y;
            }
            else if (num <= 34)
            {
                arr1 = lst5X;
                arr2 = lst5Y;
            }
            else if(num<=47)
            {
                arr1 = lst6X;
                arr2 = lst6Y;
            }
            else if (num <= 75)
            {
                arr1 = lst7X;
                arr2 = lst7Y;
            }
            foreach (var i in arr1)
            {
                foreach (var j in arr2)
                {
                    lst.Add(new Point(i, j));
                }
            }
        }
    }
}
