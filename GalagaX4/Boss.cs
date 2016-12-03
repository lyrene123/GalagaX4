using GalagaX4;
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
    class Boss : Enemies
    {
        int hitCounter = 0;
        double minX;
        double maxX;
        double minY;
        double maxY;
        DispatcherTimer timer;
        int moveCounter = 1;

        public Boss(Point point, Image image, Canvas canvas, Animation animation)
            : base(point, image, canvas, animation)
        {

        }



        public override void Die()
        {
            hitCounter++;

            BitmapImage[] explosions =
                      {
                UtilityMethods.LoadImage("pics/bossExplosions/explosion1.png"),
                UtilityMethods.LoadImage("pics/bossExplosions/explosion2.png"),
                UtilityMethods.LoadImage("pics/bossExplosions/explosion3.png"),
                UtilityMethods.LoadImage("pics/bossExplosions/explosion4.png"),
                UtilityMethods.LoadImage("pics/bossExplosions/explosion5.png"),
                UtilityMethods.LoadImage("pics/bossExplosions/explosion6.png"),
                UtilityMethods.LoadImage("pics/bossExplosions/explosion7.png"),
                UtilityMethods.LoadImage("pics/bossExplosions/explosion8.png"),
                UtilityMethods.LoadImage("pics/bossExplosions/explosion9.png")
            };

            if (hitCounter == 33)
            {
                this.stopMove();
                this.animation = new Animation(this.image, explosions, false, canvas);
                Animation.Initiate(this.animation, 40);
            }
            else
            {
                canvas.Children.Remove(this.image);
                live();

            }

        }

        public override void Fly(double frequency)
        {
            Animation.Initiate(this.animation, frequency);
            startFly();
        }
        public void startFly()
        {
            //for moving:
            this.minX = this.GetPoint().X - 50;
            this.maxX = this.GetPoint().X + 50;
            this.minY = this.GetPoint().Y - 50;
            this.maxY = this.GetPoint().Y + 50;


            this.timer = new DispatcherTimer(DispatcherPriority.Render);
            this.timer.Interval = TimeSpan.FromMilliseconds(120);
            this.timer.Tick += new EventHandler(this.updateMoveTimer_Tick);
            this.timer.Start();
        }

        public override void Shoot(double frequency)
        {
            timer.Interval = TimeSpan.FromSeconds(frequency);
            timer.Tick += new EventHandler(Shoot);
            timer.Start();
        }

        void Shoot(Object sender, EventArgs e)
        {
            Point newPoint = new Point();
            newPoint.X = this.point.X + 50;
            newPoint.Y = this.point.Y + 70;


            Image bulletPic1 = new Image();
            Bullet bullet1 = new Bullet(newPoint, bulletPic1, canvas);
            bullet1.setPlayerTarget(this.target);
            bullet1.ShootLeftSide("pics/fireball.png");

            newPoint.X = this.point.X + 100;
            newPoint.Y = this.point.Y + 50;
            Image bulletPic = new Image();
            Bullet bullet = new Bullet(newPoint, bulletPic, canvas);
            bullet.setPlayerTarget(this.target);
            bullet.ShootRightSide("pics/fireball.png");


            newPoint.Y = this.point.Y + 50;
            newPoint.X = this.point.X + 50;
            Image bulletPic2 = new Image();
            Bullet bullet2 = new Bullet(newPoint, bulletPic2, canvas);
            bullet2.setPlayerTarget(this.target);
            bullet2.ShootDown("pics/fireball.png");


        }

        private void updateMoveTimer_Tick(object sender, EventArgs e)
        {
            double bossX = this.GetPoint().X;
            double bossY = this.GetPoint().Y;

            if (moveCounter == 1)
            {
                if (bossX >= minX)
                {
                    this.point.X -= 10;
                    Canvas.SetLeft(this.GetImage(), this.point.X);
                }

                if (bossY >= minY)
                {
                    this.point.Y -= 10;
                    Canvas.SetTop(this.GetImage(), this.point.Y);
                }
                else
                {
                    moveCounter = 2;
                }
            }
            else
            {
                if (bossX <= maxX)
                {
                    this.point.X += 10;
                    Canvas.SetRight(this.GetImage(), this.point.X);
                }
                if (bossY <= maxY)
                {
                    this.point.Y += 10;
                    Canvas.SetTop(this.GetImage(), this.point.Y);
                }
                else
                {
                    moveCounter = 1;
                }
            }
        }

        public void live()
        {

            Animation bossAnimation;
            if (hitCounter < 34)
            {
                Random randX = new Random();
                this.point.X = randX.Next(500);
                Random randY = new Random();
                this.point.Y = randY.Next(300);

                this.image = new Image();
                this.image.Height = 120;
                this.image.Width = 140;
                this.canvas.Children.Add(this.image);
                Canvas.SetTop(this.image, this.point.Y);
                Canvas.SetLeft(this.image, this.point.X);
                //  this.setPointX(this.point.X);
                //this.setPointnY(this.point.Y);
                this.image.Source = UtilityMethods.LoadImage(SwitchImage()[0]);
                BitmapImage[] bossImages = { UtilityMethods.LoadImage(SwitchImage()[0]),
                    UtilityMethods.LoadImage(SwitchImage()[1]), UtilityMethods.LoadImage(SwitchImage()[2]),};
                bossAnimation = new Animation(image, bossImages, true);
                Animation.Initiate(bossAnimation, 200);
            }


        }


        private String[] SwitchImage()
        {

            String[] path = new string[3];

            if (hitCounter >= 1 && hitCounter < 3)
            {
                path[0] = "pics/boss/boss1.png";
                path[1] = "pics/boss/boss2.png";
                path[2] = "pics/boss/boss3.png";
            }
            else if (hitCounter >= 3 && hitCounter < 6)
            {
                path[0] = "pics/boss/wing1/boss1Wing1.png";
                path[1] = "pics/boss/wing1/boss2Wing1.png";
                path[2] = "pics/boss/wing1/boss3Wing1.png";

            }
            else if (hitCounter >= 6 && hitCounter < 9)
            {
                path[0] = "pics/boss/wing2/boss1Wing2.png";
                path[1] = "pics/boss/wing2/boss2Wing2.png";
                path[2] = "pics/boss/wing2/boss3Wing2.png";

            }
            else if (hitCounter >= 9 && hitCounter < 12)
            {
                path[0] = "pics/boss/wing3/boss1Wing3.png";
                path[1] = "pics/boss/wing3/boss2Wing3.png";
                path[2] = "pics/boss/wing3/boss3Wing3.png";

            }
            else if (hitCounter >= 12 && hitCounter < 15)
            {
                path[0] = "pics/boss/wing4/boss1Wing4.png";
                path[1] = "pics/boss/wing4/boss2Wing4.png";
                path[2] = "pics/boss/wing4/boss3Wing4.png";

            }
            else if (hitCounter >= 15 && hitCounter < 18)
            {
                path[0] = "pics/boss/wing5/boss1Wing5.png";
                path[1] = "pics/boss/wing5/boss2Wing5.png";
                path[2] = "pics/boss/wing5/boss3Wing5.png";

            }
            else if (hitCounter >= 18 && hitCounter < 21)
            {
                path[0] = "pics/boss/wing6/boss1Wing6.png";
                path[1] = "pics/boss/wing6/boss2Wing6.png";
                path[2] = "pics/boss/wing6/boss3Wing6.png";

            }
            else if (hitCounter >= 21 && hitCounter < 24)
            {
                path[0] = "pics/boss/wing7/boss1Wing7.png";
                path[1] = "pics/boss/wing7/boss2Wing7.png";
                path[2] = "pics/boss/wing7/boss3Wing7.png";

            }
            else if (hitCounter >= 24 && hitCounter < 27)
            {
                path[0] = "pics/boss/wing8/boss1Wing8.png";
                path[1] = "pics/boss/wing8/boss2Wing8.png";
                path[2] = "pics/boss/wing8/boss3Wing8.png";

            }
            else if (hitCounter >= 27 && hitCounter < 30)
            {
                path[0] = "pics/boss/wing9/boss1Wing9.png";
                path[1] = "pics/boss/wing9/boss2Wing9.png";
                path[2] = "pics/boss/wing9/boss3Wing9.png";

            }
            else if (hitCounter >= 30 && hitCounter < 33)
            {
                path[0] = "pics/boss/wing10/boss1Wing10.png";
                path[1] = "pics/boss/wing10/boss2Wing10.png";
                path[2] = "pics/boss/wing10/boss3Wing10.png";

            }


            return path;
        }

        private void fire()
        {

        }

        public void stopMove()
        {
            this.timer.Stop();
            this.animation.Stop();
        }


    }
}
