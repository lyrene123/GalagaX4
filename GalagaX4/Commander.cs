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
    class Commander : Enemies
    {
        // 1=left 2=right
        int moveCounter = 1;
        double maxX;
        double minX;
        bool moveDown;

        int shotCounter;
        bool isShot;
        bool dead;

        DispatcherTimer timer;
        DispatcherTimer timerShoot;

        public Commander(Point point, Image image, Canvas canvas, Animation animation) 
            : base(point, image, canvas, animation)
        {
            this.isShot = false;
            this.dead = false;
        }

        public bool isShoot()
        {
            return this.isShot;

        }
        public bool IsDead()
        {
            return this.dead;
        }

        public override void Die()
        {
            dead = true;
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
            double commanderX = this.GetPoint().X;

            if(this.moveDown == true)
            {
                this.point.Y += 30;
                Canvas.SetTop(this.GetImage(), this.point.Y);
                moveDown = false;
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
        }

        public void stopMove()
        {
            this.timer.Stop();
        }

        public void stopShoot()
        {
            if(this.timerShoot != null)
            {
                this.timerShoot.Stop();
            }
            
            this.isShot = false;
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
            this.isShot = true;
            double position = Canvas.GetLeft(this.GetImage());
            double midOfImgae = this.GetImage().Width / 2;

            Image bulletPic = new Image();
            Bullet bullet = new Bullet(this.point, bulletPic, canvas);
            bullet.setPlayerTarget(this.target);
            Canvas.SetLeft(bullet.GetImage(), (position + midOfImgae - 3.5));

            bullet.ShootDown();
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
