using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GalagaX4
{
    /// <summary>
    /// The UtilityMethods Class defines two methods that perform common
    /// functions in the Game such as Loading Images and defining the boundaries
    /// on the screen to avoid the objects to exceed the limites of the Canvas.
    /// </summary>
    class UtilityMethods
    {
        /// <summary>
        /// The LoadImage method creates a BitmapImage Object 
        /// using the supplied relative Uri(Relative path of the Image)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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
        /// <summary>
        /// The Clamp method define the boundaries of the objects 
        /// preventing movements that exceeds the limit provided.
        /// </summary>
        /// <param name="val"> The base value to be checked</param>
        /// <param name="min"> The minimun value that is not supposed to be exceeded</param>
        /// <param name="max"> The max value that is not supposed to be exceeded</param>
        /// <returns></returns>
        public static double Clamp(double val, double min, double max)
        {
            if (val < min) return min;
            else if (val > max) return max;
            return val;
        }
    }
}
