using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GalagaX4
{
    class UtilityMethods
    {
        // Testing change
        public static BitmapImage LoadImage(string path)
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(path, UriKind.Relative);
            src.EndInit();

            return src;
        }

        public static BitmapImage LoadImageFullPath(String path)
        {
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(path, UriKind.Absolute);
            src.EndInit();

            return src;
        }

        public static double Clamp(double val, double min, double max)
        {
            if (val < min) return min;
            else if (val > max) return max;
            return val;
        }
    }
}
