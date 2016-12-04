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

        DispatcherTimer timerFly; //timer for moving

        public Bug(Point point, Image image, Canvas canvas, Animation animation)
            : base(point, image, canvas, animation)
        {
            this.moveCounter = 1;
            this.moveDown = false;
            this.dive = false;
        }

        public override void Fly(double frequency)
        {
            Animation.Initiate(this.animation, 200);
            startFly(frequency);
        }

        public void startFly(double frequency)
        {
            this.timerFly = new DispatcherTimer(DispatcherPriority.Render);
            this.timerFly.Interval = TimeSpan.FromMilliseconds(frequency);
            this.timerFly.Tick += new EventHandler(this.updateMoveHorizontal);
            this.timerFly.Start();
        }

        private void updateMoveHorizontal(object sender, EventArgs e)
        {
            double beeX = this.GetPoint().X;

            if (this.point.Y <= 550)
            {
                //move down
                if (this.moveDown == true)
                {
                    this.point.Y += 35;
                    Canvas.SetTop(this.GetImage(), this.point.Y);
                    this.moveDown = false;
                    playerCollision();

                    if (dive == true)
                    {
                        Random rand = new Random();
                        int randNum = rand.Next(20);
                        if (randNum % 2 == 0)
                        {
                            this.timerFly.Stop();
                            this.timerFly = new DispatcherTimer(DispatcherPriority.Render);
                            this.timerFly.Interval = TimeSpan.FromMilliseconds(120);
                            this.timerFly.Tick += new EventHandler(this.updateMoveDown);
                            this.timerFly.Start();
                        }
                    }
                }

                //move horizontal
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
                playerCollision();
            }
            else
            {
                canvas.Children.Remove(this.image);
                returnToTheTop();
            }
        }

        private void updateMoveDown(object sender, EventArgs e)
        {
            if (this.point.Y <= 550)
            {
                this.point.Y += 8;
                Canvas.SetTop(this.GetImage(), this.point.Y);
                playerCollision();
            }
            else
            {
                canvas.Children.Remove(this.image);
                returnToTheTop();
                dive = false;
                this.timerFly.Stop();
                this.timerFly = new DispatcherTimer(DispatcherPriority.Render);
                this.timerFly.Interval = TimeSpan.FromMilliseconds(120);
                this.timerFly.Tick += new EventHandler(this.updateMoveHorizontal);
                this.timerFly.Start();
            }
        }

        public void stopMove()
        {
            this.timerFly.Stop(); //stop moving
            this.animation.Stop(); //stop animation
        }

        public override void Shoot(double frequency)
        {
            //bugs do not implement shooting
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
