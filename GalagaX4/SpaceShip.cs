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
    class SpaceShip : Enemies
    {
        // 1=left 2=right
        int moveCounter = 1;
        double maxX;
        double minX;
        bool moveDown = false;

        DispatcherTimer timerFly;
        DispatcherTimer timerShoot;

        bool shoot;

        public SpaceShip() : base()
        {
            //timer = new DispatcherTimer(DispatcherPriority.Normal);
            /*timer.Interval = TimeSpan.FromSeconds(frequency);
            timer.Start();*/
            shoot = false;
        }

        public SpaceShip(Point point, Image image, Canvas canvas
            , Animation animaion) : base(point, image, canvas, animaion)
        {
            shoot = false;
        }

        public override void Fly(double frequency)
        {
            //for moving:
            this.minX = this.GetPoint().X - 130;
            this.maxX = this.GetPoint().X + 130;

            timerFly = new DispatcherTimer(DispatcherPriority.Normal);
            timerFly.Interval = TimeSpan.FromMilliseconds(frequency);
            timerFly.Tick += new EventHandler(MoveHorizontal);
            timerFly.Start();
        }

        void MoveHorizontal(Object sender, EventArgs e)
        {
            double x = Canvas.GetLeft(this.GetImage());

            if(moveDown == true)
            {
                this.point.Y += 8;
                Canvas.SetTop(this.GetImage(), this.point.Y);
                moveDown = false;
            }

            if (moveCounter == 1)
            {
                if (x >= minX)
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
                if (x <= maxX)
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

           // x += 10;
            //Canvas.SetLeft(this.GetImage(), x);
        }

        public bool IsShoot()
        {
            return this.shoot;
        }


        public override void Shoot(double frequency)
        {

            timerShoot = new DispatcherTimer(DispatcherPriority.Normal);
            timerShoot.Interval = TimeSpan.FromSeconds(frequency);
            timerShoot.Tick += new EventHandler(Shoot);
            timerShoot.Start();
        }

        void Shoot(Object sender, EventArgs e)
        {

            this.shoot = true;

            double position = Canvas.GetLeft(this.GetImage());
            double midOfImgae = this.GetImage().Width / 2;

            Image bulletPic = new Image();
            Bullet bullet = new Bullet(this.point, bulletPic, canvas);
            bullet.setPlayerTarget(this.target);
            Canvas.SetLeft(bullet.GetImage(), (position + midOfImgae - 3.5));

            bullet.ShootDown("pics/bulletFlip180.png");
        }

        public void stopMove()
        {
            this.timerFly.Stop();
        }

        public void StopShoot()
        {
            this.timerShoot.Stop();
            this.shoot = false;
        }

        public override void Die()
        {
            this.target.addPoints(200);
            BitmapImage[] explosions =
            {
                UtilityMethods.LoadImage("pics/explosions/enemiesExp0.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp1.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp2.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp3.png")
            };
            this.stopMove();
            Animation explode = new Animation(this.image, explosions, false, canvas);
            Animation.Initiate(explode, 40);
        }

    }
}
