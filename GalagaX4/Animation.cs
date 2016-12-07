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
    /// moving in a certain pathern defined by the user. The illusion created 
    /// The brain perceives the group of images as a single changing scene.
    /// </summary>
    class Animation
    {
        int index = 0;
        DispatcherTimer timer;
        BitmapImage[] bitmapImagesArray;
        Image image;
        bool repeated;
        Canvas canvas;

        public Animation()
        {
            this.image = null;
            this.bitmapImagesArray = new BitmapImage[0];
            repeated = false;
        }

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

        public Image getImage()
        {
            return this.image;
        }

        public void setImage(Image image)
        {
            this.image = image;
        }

        public BitmapImage[] getBitmapImageArray()
        {
            BitmapImage[] newBitmap = new BitmapImage[this.bitmapImagesArray.Length];
            for (int i = 0; i < newBitmap.Length; i++)
            {
                newBitmap[i] = this.bitmapImagesArray[i];
            }
            return newBitmap;
        }

        public void setBitmapImageArray(BitmapImage[] newBitmap)
        {
            this.bitmapImagesArray = new BitmapImage[newBitmap.Length];
            for (int i = 0; i < newBitmap.Length; i++)
            {
                this.bitmapImagesArray[i] = newBitmap[i];
            }

        }

        public bool IsRepeated()
        {
            return this.repeated;
        }

        public void setRepeated(bool repeated)
        {
            this.repeated = repeated;
        }

        public void Stop()
        {
            this.timer.Stop();
        }

        public void Start()
        {
            this.timer.Start();
        }

        public static void Initiate(Animation animation, double animationSpeed)
        {
            animation.Initiate(animationSpeed);
        }

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
