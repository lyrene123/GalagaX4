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
    class Bug : Enemies
    {
        //int imageCounter = 1;
        // 1=left 2=right
        int moveCounter = 1;
        double maxX;
        double minX;
        bool moveDown = false;
        
        DispatcherTimer timer;
        //Animation animation;

        public Bug(Point point, Image image, Canvas canvas, Animation animation)
            : base(point, image, canvas, animation)
        {
            //this.animation = animation;
            this.setTarget(this.target);
        }

        public override void Fly(double frequency)
        {
            Animation.Initiate(this.animation, 200);

            startFly(frequency);
        }

        public void startFly(double frequency)
        {
            //for moving:
            this.minX = this.GetPoint().X - 130;
            this.maxX = this.GetPoint().X + 130;

            this.timer = new DispatcherTimer(DispatcherPriority.Render);
            this.timer.Interval = TimeSpan.FromMilliseconds(frequency);
            this.timer.Tick += new EventHandler(this.updateMoveTimer_Tick);
            this.timer.Start();
        }
        
        private void updateMoveTimer_Tick(object sender, EventArgs e)
        {
            double beeX = this.GetPoint().X;

            if (this.point.Y <= 550)
            {
                if (this.moveDown == true)
                {
                    this.point.Y += 8;
                    Canvas.SetTop(this.GetImage(), this.point.Y);
                    this.moveDown = false;
                }

                if (moveCounter == 1)
                {
                    if (beeX >= minX)
                    {
                        this.point.X -= 10;
                        Canvas.SetLeft(this.GetImage(), this.point.X);
                    }
                    else
                    {
                        moveCounter = 2;
                        moveDown = true;
                    }
                }
                else
                {
                    if (beeX <= maxX)
                    {
                        this.point.X += 10;
                        Canvas.SetLeft(this.GetImage(), this.point.X);
                    }
                    else
                    {
                        moveCounter = 1;
                        moveDown = true;
                    }
                }
            }
            else
            {
                this.stopMove();
                canvas.Children.Remove(this.image);
            }
        }

        public void stopMove()
        {
            this.timer.Stop();
            this.animation.Stop();
        }

        public override void Shoot(double frequency)
        {
           //bugs do not implement shoot
        }

        public override void Die()
        {
            this.target.addPoints(100);
            BitmapImage[] explosions =
            {
                UtilityMethods.LoadImage("pics/explosions/enemiesExp0.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp1.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp2.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp3.png")

            };

            this.stopMove();
            this.animation = new Animation(this.image, explosions, false, canvas);
            Animation.Initiate(this.animation, 40);

        }
    }
}
