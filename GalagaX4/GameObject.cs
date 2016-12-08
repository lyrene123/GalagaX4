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
    /// <summary>
    /// The GameObject abstract class defines the general attributes and 
    /// behavior of an object of the game galaga
    /// </summary>
    public abstract class GameObject
    {
        protected Point point; //position of the game element on the canvas
        protected Image image; //image of the game element
        protected Canvas canvas; //the game canvas in which the game element is in

        /// <summary>
        /// The no parameter GameObject sets the position, image and canvas to
        /// default values
        /// </summary>
        public GameObject()
        {
            this.point.X = 0;
            this.point.Y = 0;
            this.image = null;
            this.canvas = null;
        }

        /// <summary>
        /// The GameObject constructor sets the position of the game element,
        /// the image and the game canvas from the values inputed into the constructor
        /// </summary>
        /// <param name="point">Position of the game elements of type Point</param>
        /// <param name="image">The image of the game elements </param>
        /// <param name="canvas">The game canvas in which the game elements is in</param>
        public GameObject(Point point, Image image, Canvas canvas)
        {
            this.point.X = point.X;
            this.point.Y = point.Y;

            this.image = image;
            this.canvas = canvas;

        }

        //abtract method for destroying any game elements
        public abstract void Die();

        /// <summary>
        /// GetPoint method returns the position of the game elements
        /// </summary>
        /// <returns>instance of type Point</returns>
        public Point GetPoint()
        {
            return this.point;
        }

        /// <summary>
        /// GetImage method returns the image of the game element
        /// </summary>
        /// <returns>instance of type Image</returns>
        public Image GetImage()
        {
            return this.image;
        }

        /// <summary>
        /// GetCanvas method returns the canvas in which the game element
        /// is in
        /// </summary>
        /// <returns>instance of type Canvas</returns>
        public Canvas GetCanvas()
        {
            return this.canvas;
        }

        /// <summary>
        /// SetPoint method sets the position of the position game element
        /// </summary>
        /// <param name="point">instance of type Point</param>
        public void SetPoint(Point point)
        {
            this.point = point;
        }

        /// <summary>
        /// SetPointX method sets the X position of the game element
        /// </summary>
        /// <param name="x">double value</param>
        public void SetPointX(double x)
        {
            this.point.X = x;
        }

        /// <summary>
        /// SetPointY method sets the Y position of the game element
        /// </summary>
        /// <param name="y">double value</param>
        public void SetPointY(double y)
        {
            this.point.Y = y;
        }

        /// <summary>
        /// SetImage method sets the image of the game element
        /// </summary>
        /// <param name="image">instance of type Image</param>
        public void SetImage(Image image)
        {
            this.image = image;
        }

        /// <summary>
        /// SetImageSource method sets the source of the image of 
        /// the game elements
        /// </summary>
        /// <param name="path">string value for the path of the image</param>
        public void SetImageSource(string path)
        {
            this.image.Source = UtilityMethods.LoadImage(path);
        }

        /// <summary>
        /// SetCanvas method sets the canvas in which the game element
        /// is located
        /// </summary>
        /// <param name="canvas">instance of type Canvas</param>
        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }
    }
}
