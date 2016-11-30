using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GalagaX4
{
    //testing 101
    abstract class Enemies : GameObject
    {
        //test
        //protected int frequency;
        protected Animation animation;
        protected Player target;

        public Enemies() : base()
        {
           // this.frequency = 0;
            this.animation = null;
        }

        public Enemies(Point point, Image image, Canvas canvas
            , Animation animation) : base(point, image, canvas)
        {
            if (image == null || canvas == null || animation == null)
            {
                throw new NullReferenceException();
            }

            //this.frequency = shootFrequency;
            this.animation = new Animation(animation.getImage()
                , animation.getBitmapImageArray(), animation.IsRepeated());
        }

        public void setTarget(Player player)
        {
            this.target = player;
        }

        /*
        public int getShootFrequency()
        {
            return this.frequency;
        }

        public void setShootFrequency(int shootFrequency)
        {
            this.frequency = shootFrequency;
        }
        */
        public abstract void Fly(double frequency);

        public abstract void Shoot(double frequency);

    }
}
