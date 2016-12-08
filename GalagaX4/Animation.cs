using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GalagaX4
{
    /// <summary>
    /// The Animation class Animation define the elements responsible for the creation
    /// of the fly movements illusion that is created by looping through a series of images,
    /// each image different from the last image giving the impression that the object is
    /// moving in a certain pathern defined by the user.
    /// </summary>
    class Animation
    {
        int index = 0;
        DispatcherTimer timer;
        BitmapImage[] bitmapImagesArray;
        Image image;
        bool repeated;
        Canvas canvas;
        /// <summary>
        /// The Animation Class Constructor without parameters Sets the image to null,
        /// and it creates a bitmapImage Array that will be used to
        /// allocate the images to buit the illusion. It also set the
        /// repeat bool to false.
        /// </summary>
        public Animation()
        {
            this.image = null;
            this.bitmapImagesArray = new BitmapImage[0];
            repeated = false;
        }
        /// <summary>
        /// The Animation Class Constructor Sets the image, the array of bitmapimages
        /// provided.
        /// </summary>
        /// <param name="image">The image intended to be animated</param>
        /// <param name="ImageSources">The Array BitmapImage that will receive a series of the Image provided</param>
        /// <param name="repeated">The bool set to true or false</param>
        public Animation(Image image, BitmapImage[] ImageSources, bool repeated)
        {
            this.image = image;
            this.repeated = repeated;
            this.bitmapImagesArray = new BitmapImage[ImageSources.Length];
            for (int i = 0; i < ImageSources.Length; i++)
            {
                this.bitmapImagesArray[i] = ImageSources[i];
            }
        }
        /// <summary>
        /// The Animation Class Overloaded Constructor Sets the image, the array of bitmapimages
        /// provided, the boolean repeated and also defines a canvas.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="ImageSources"></param>
        /// <param name="repeated"></param>
        /// <param name="canvas"></param>
        public Animation(Image image, BitmapImage[] ImageSources, bool repeated, Canvas canvas)
        {
            this.image = image;
            this.repeated = repeated;
            this.canvas = canvas;

            this.bitmapImagesArray = new BitmapImage[ImageSources.Length];
            for (int i = 0; i < ImageSources.Length; i++)
            {
                this.bitmapImagesArray[i] = ImageSources[i];
            }
        }
        /// <summary>
        /// The getImage method returns the image provided.
        /// </summary>
        /// <returns>An Image Object</returns>
        public Image getImage()
        {
            return this.image;
        }
        /// <summary>
        /// The setImage method sets the Image.
        /// </summary>
        /// <param name="image">An image Object</param>
        public void setImage(Image image)
        {
            this.image = image;
        }
        /// <summary>
        /// The getBitmapImageArray returns an array of the BitmapImage Object.
        /// </summary>
        /// <returns>An Array of BitmapImage</returns>
        public BitmapImage[] getBitmapImageArray()
        {
            BitmapImage[] newBitmap = new BitmapImage[this.bitmapImagesArray.Length];
            for (int i = 0; i < newBitmap.Length; i++)
            {
                newBitmap[i] = this.bitmapImagesArray[i];
            }
            return newBitmap;
        }
        /// <summary>
        /// The setBitmapImageArray sets an array of BitmapImage.
        /// </summary>
        /// <param name="newBitmap">An array of BitmapImage Objects</param>
        public void setBitmapImageArray(BitmapImage[] newBitmap)
        {
            this.bitmapImagesArray = new BitmapImage[newBitmap.Length];
            for (int i = 0; i < newBitmap.Length; i++)
            {
                this.bitmapImagesArray[i] = newBitmap[i];
            }

        }
        /// <summary>
        /// The IsRepeated method returns the repeated boolean.
        /// </summary>
        /// <returns>The boolean repeated variable</returns>
        public bool IsRepeated()
        {
            return this.repeated;
        }
        /// <summary>
        /// The setRepeated method sets the boolean repeats to true or false.
        /// </summary>
        /// <param name="repeated">True or false of type bool</param>
        public void setRepeated(bool repeated)
        {
            this.repeated = repeated;
        }
        /// <summary>
        /// The Stop method stops the timer of type DispatcherTimer Object from this class.
        /// </summary>
        public void Stop()
        {
            this.timer.Stop();
        }
        /// <summary>
        /// The Start method Starts the Timer of type DispatcherTimer Object from this class.
        /// </summary>
        public void Start()
        {
            this.timer.Start();
        }
        /// <summary>
        /// The Initiate method involkes the Initiate method from the Animation class with a specific speed.
        /// </summary>
        /// <param name="animation">The animation Object provided</param>
        /// <param name="animationSpeed">The double value speed provided.</param>
        public static void Initiate(Animation animation, double animationSpeed)
        {
            animation.Initiate(animationSpeed);
        }
        /// <summary>
        /// The Initiate method defines the speed of the animation for each image provided
        /// and also starts the timer that will define the frequency of each frame.
        /// </summary>
        /// <param name="animationSpeed">The double value of the speed</param>
        void Initiate(double animationSpeed)
        {
            foreach (BitmapImage image in bitmapImagesArray)
            {
                if (image == null)
                    throw new NullReferenceException("There's no such an image : " + image.ToString());
            }

            this.timer = new DispatcherTimer(DispatcherPriority.Render);
            this.timer.Interval = TimeSpan.FromMilliseconds(animationSpeed);
            this.timer.Tick += new EventHandler(UpdateEachFrame);
            this.timer.Start();
        }
        /// <summary>
        /// The UpdateEachFrame method event Handler will update
        /// the frames of the image synchronously.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void UpdateEachFrame(object sender, EventArgs e)
        {
            if (index < bitmapImagesArray.Length - 1)
            {
                image.Source = bitmapImagesArray[index];
                index++;
            }
            else
            {
                image.Source = bitmapImagesArray[index];
                if (repeated == true)
                {
                    index = 0;
                }
                else
                {
                    Stop();
                    if (this.image.IsLoaded)
                    {

                        canvas.Children.Remove(this.image);
                    }
                }
            }
        }
    }
}
