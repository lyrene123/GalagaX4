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
        protected Animation animation;
        protected Player target;

        public Enemies() : base()
        {
            this.animation = null;
        }

        public Enemies(Point point, Image image, Canvas canvas) 
            : base(point, image, canvas)
        {
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

        public void playerCollision()
        {
            double currentX = Canvas.GetLeft(this.image);
            double currentY = Canvas.GetTop(this.image);
            double playerX = Canvas.GetLeft(this.target.GetImage());
            double playerY = Canvas.GetTop(this.target.GetImage());

            Rect current = new Rect(currentX, currentY, this.image.Width - 5, this.image.Height - 5);
            Rect player = new Rect(playerX, playerY, this.image.Width - 5, this.image.Height - 5);

            if (current.IntersectsWith(player))
            {
                target.Die();
            }
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
