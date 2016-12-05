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
        DispatcherTimer timerFly;
        DispatcherTimer timerShoot;

        public SpaceShip() : base()
        {
        }

        public SpaceShip(Point point, Image image, Canvas canvas)
            : base(point, image, canvas)
        {
            this.moveCounter = 1;
            this.moveDown = false;
            this.dive = false;

            this.isShooting = false;
            this.dead = false;

            this.moveDownFrequency = 35;
            this.diveFrequency = 5;
        }

        public override void Fly(double frequency)
        {
            this.flyFrequency = frequency;
            timerFly = new DispatcherTimer(DispatcherPriority.Normal);
            timerFly.Interval = TimeSpan.FromMilliseconds(frequency);
            timerFly.Tick += new EventHandler(MoveHorizontal);
            timerFly.Start();
        }

        void MoveHorizontal(Object sender, EventArgs e)
        {
            double x =this.GetPoint().X;

            if (this.point.Y <= 550)
            {
                if (moveDown == true)
                {
                    this.point.Y += this.moveDownFrequency;
                    Canvas.SetTop(this.GetImage(), this.point.Y);
                    moveDown = false;
                    playerCollision();

                    if (dive == true)
                    {
                        Random rand = new Random();
                        int randNum = rand.Next(20);
                        if (randNum % 5 == 0)
                        {
                            this.timerFly.Stop();
                            this.timerFly = new DispatcherTimer(DispatcherPriority.Render);
                            this.timerFly.Interval = TimeSpan.FromMilliseconds(120);
                            this.timerFly.Tick += new EventHandler(this.updateMoveDown);
                            this.timerFly.Start();
                        }
                    }
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
                this.point.Y += this.diveFrequency;
                Canvas.SetTop(this.GetImage(), this.point.Y);
                playerCollision();
            }
            else
            {
                canvas.Children.Remove(this.image);
                returnToTheTop();
                dive = false;
                this.timerFly.Stop();
                Fly(this.flyFrequency);
            }
        }

        public bool isShoot()
        {
            return this.isShooting;
        }

        public bool IsDead()
        {
            return this.dead;
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
            this.isShooting = true;

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

        public void restartMove()
        {
            this.timerFly.Start();
        }

        public void StopShoot()
        {
            if (this.timerShoot != null)
            {
                this.timerShoot.Stop();
            }
        }

        public void restartShoot()
        {
            if (this.timerShoot != null)
            {
                this.timerShoot.Start();
            }
        }

        public override void Die()
        {
            this.dead = true;
            this.target.addPoints(200);
            //this.isShot = true;
            BitmapImage[] explosions =
            {
                UtilityMethods.LoadImage("pics/explosions/enemiesExp0.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp1.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp2.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp3.png")
            };
            this.stopMove();
            this.StopShoot();
            Animation explode = new Animation(this.image, explosions, false, canvas);
            Animation.Initiate(explode, 40);

        }
    }
}
