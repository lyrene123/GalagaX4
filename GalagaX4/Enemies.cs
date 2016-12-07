using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GalagaX4
{
    
    abstract class Enemies : GameObject
    {
        protected Animation animation;
        protected Player target;

        protected int moveCounter;
        protected double maxX;
        protected double minX;
        protected  bool moveDown;
        protected bool dive;

        protected bool isShooting; //if shooting already
        protected bool dead; //if destroyed already

        protected double flyFrequency;
        protected int diveFrequency;
        protected int moveDownFrequency;

        public Enemies() : base()
        {
            this.animation = null;
        }

        public Enemies(Point point, Image image, Canvas canvas)
            : base(point, image, canvas)
        {
            this.animation = null;
            //boundaries for moving:
            this.minX = this.GetPoint().X - 130;
            this.maxX = this.GetPoint().X + 130;
        }

        public Enemies(Point point, Image image, Canvas canvas
            , Animation animation) : base(point, image, canvas)
        {
            if (image == null || canvas == null || animation == null)
            {
                throw new NullReferenceException();
            }

            this.animation = new Animation(animation.getImage()
                , animation.getBitmapImageArray(), animation.IsRepeated());

            //boundaries for moving:
            this.minX = this.GetPoint().X - 130;
            this.maxX = this.GetPoint().X + 130;
        }

        public void setTarget(Player player)
        {
            this.target = player;
        }

        public void setMoveCounter(int counter)
        {
            this.moveCounter = counter;
        }

        public void setDive(bool dive)
        {
            this.dive = dive;
        }

        public void setMoveDownFrequency(int freq)
        {
            this.moveDownFrequency = freq;
        }

        public void setDiveFrequency(int freq)
        {
            this.diveFrequency = freq;
        }

        protected void playerCollision()
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
                this.Die();
            }
        }

        protected void returnToTheTop()
        {
            this.point.Y = 10;
            Canvas.SetTop(this.image, this.point.Y);
            canvas.Children.Add(this.image);
        }

        public abstract void Fly(double frequency);

        public abstract void Shoot(double frequency);

    }
}
