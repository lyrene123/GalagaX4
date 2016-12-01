using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GalagaX4
{
    public abstract class GameObject
    {
        protected Point point;
        protected Image image;
        protected Canvas canvas;

        public GameObject()
        {
            this.point.X = 0;
            this.point.Y = 0;
            this.image = null;
            this.canvas = null;
        }

        public GameObject(Point point, Image image, Canvas canvas)
        {
            this.point.X = point.X;
            this.point.Y = point.Y;

            this.image = image;
            this.canvas = canvas;

        }

        public abstract void Die();

        public Point GetPoint()
        {
            return this.point;
        }

        public Image GetImage()
        {
            return this.image;
        }

        public Canvas GetCanvas()
        {
            return this.canvas;
        }

        public void SetPoint(Point point)
        {
            this.point = point;
        }

        public void SetPointX(double x) 
        {
            this.point.X = x;
        }

        public void SetPointY(double y)
        {
            this.point.Y = y;
        }

        public void SetImage(Image image)
        {
            this.image = image;
        }

        public void SetImageSource(string path)
        {
            this.image.Source = UtilityMethods.LoadImage(path);
        }

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }
    }
}
