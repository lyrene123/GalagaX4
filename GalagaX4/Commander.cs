﻿using System;
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
    class Commander : Enemies
    {

        int shotCounter;
        bool isShot;
        DispatcherTimer timerFly;
        DispatcherTimer timerShoot;

        public Commander(Point point, Image image, Canvas canvas, Animation animation)
            : base(point, image, canvas, animation)
        {
            this.moveCounter = 1;
            this.moveDown = false;
            this.dive = false;

            this.isShot = false; //if shot once already
            this.dead = false; //if destroyed already
            this.isShooting = false; //if shooting already
        }

        public bool isShoot()
        {
            return this.isShooting;

        }

        public bool IsDead()
        {
            return this.dead;
        }

        public override void Die()
        {
            this.dead = true;
            this.target.addPoints(250);
            //this.isShot = true;
            BitmapImage[] explosions =
           {
                UtilityMethods.LoadImage("pics/explosions/enemiesExp0.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp1.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp2.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp3.png")
            };
            this.stopMove();
            this.stopShoot();

            this.animation = new Animation(this.image, explosions, false, canvas);
            Animation.Initiate(this.animation, 40);
        }

        public override void Fly(double frequency)
        {
            Animation.Initiate(this.animation, 500);
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
            double commanderX = this.GetPoint().X;

            if (this.point.Y <= 550)
            {
                if (this.moveDown == true)
                {
                    this.point.Y += 20;
                    Canvas.SetTop(this.GetImage(), this.point.Y);
                    moveDown = false;
                    playerCollision();

                    if (dive == true)
                    {
                        Random rand = new Random();
                        int randNum = rand.Next(20);
                        if (randNum % 10 == 0)
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
                    if (commanderX >= minX)
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
                    if (commanderX <= maxX)
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
                this.point.Y += 3;
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
            this.timerFly.Stop();
        }

        public void stopShoot()
        {
            if (this.timerShoot != null)
            {
                this.timerShoot.Stop();
            }
        }

        public override void Shoot(double frequency)
        {
            this.timerShoot = new DispatcherTimer(DispatcherPriority.Render);
            this.timerShoot.Interval = TimeSpan.FromSeconds(frequency);
            this.timerShoot.Tick += new EventHandler(Shoot);
            this.timerShoot.Start();
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

        public void changeImage()
        {
            this.animation.Stop();
            BitmapImage[] commanderImages = { UtilityMethods.LoadImage("pics/commanderGreen0.png"),
                    UtilityMethods.LoadImage("pics/commanderGreen1.png") };
            this.animation = new Animation(this.image, commanderImages, true);
            Animation.Initiate(animation, 500);
        }

        public int getShotValue()
        {
            return this.shotCounter;
        }

        public void addShotValue()
        {
            this.shotCounter++;
        }

        public bool getIsShot()
        {
            return this.isShot;
        }

        public void setIsShot(bool isShot)
        {
            this.isShot = isShot;
        }

    }
}
