using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcfTreeView
{
    internal static class Extensions
    {

        public static Point Add(this Point self, Point other)
        {
            return new Point(self.X + other.X, self.Y + other.Y);
        }

        public static Point Subtract(this Point self, Point other)
        {
            return new Point(self.X - other.X, self.Y - other.Y);
        }
    }
}
