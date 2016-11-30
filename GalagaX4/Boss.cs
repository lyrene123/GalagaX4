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

namespace Galaga
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
            if (hitCounter == 5)
            {
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

                this.stopMove();
                this.animation = new Animation(this.image, explosions, false, canvas);
                Animation.Initiate(this.animation, 40);
            }
            else
            {

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
            double position = Canvas.GetLeft(this.GetImage());
            double midOfImage = this.GetImage().Width / 2;
            double midOfHeight = this.GetImage().Height / 2;

            Image bulletPic = new Image();
            Bullet bullet = new Bullet(this.point, bulletPic, canvas);

            Canvas.SetLeft(bullet.GetImage(), (position + midOfImage + midOfHeight));
            bullet.ShootRightSide("pics/fireball.png");

            Image bulletPic1 = new Image();
            Bullet bullet1 = new Bullet(this.point, bulletPic1, canvas);

            Canvas.SetLeft(bullet1.GetImage(), (position + midOfImage + midOfHeight));
            bullet1.ShootLeftSide("pics/fireball.png");

            Image bulletPic2 = new Image();
            Bullet bullet2 = new Bullet(this.point, bulletPic2, canvas);

            Canvas.SetLeft(bullet2.GetImage(),(position + midOfImage + midOfHeight));
            bullet2.ShootDown("pics/fireball.png");


        }

        private void updateMoveTimer_Tick(object sender, EventArgs e)
        {
            double bossX = this.point.X;
            double bossY = this.point.Y;

            if (moveCounter == 1)
            {
                if (bossX >= minX)
                {
                    this.point.X -= 10;
                    Canvas.SetLeft(this.image, this.point.X);
                }

                if (bossY >= minY)
                {
                    this.point.Y -= 10;
                    Canvas.SetTop(this.image, this.point.Y);
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
                    Canvas.SetRight(this.image, this.point.X);
                }
                if (bossY <= maxY)
                {
                    this.point.Y += 10;
                    Canvas.SetTop(this.image, this.point.Y);
                }
                else
                {
                    moveCounter = 1;
                }
            }
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
