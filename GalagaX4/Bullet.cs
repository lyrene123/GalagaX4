using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GalagaX4
{
    class Bullet : GameObject
    {
        DispatcherTimer timerShootUp;
        DispatcherTimer timerShootDown;
        DispatcherTimer timerShootRightSide;
        DispatcherTimer timerShootLeftSide;

        List<Enemies> enemies;
        static List<Bullet> bullets = new List<Bullet>();
        Player player;

        public Bullet() : base()
        {

        }

        public Bullet(Point point, Image image, Canvas canvas) : base(point, image, canvas)
        {
            this.canvas.Children.Add(image);
            bullets.Add(this);
        }

        public static List<Bullet> getBulletList
        {
            get{ return bullets; }
        }

        public void setEnemyTarget(List<Enemies> enemies)
        {
            this.enemies = enemies;
        }

        public void setPlayerTarget(Player player)
        {
            this.player = player;
        }

        public void ShootUp()
        {
            this.timerShootUp = new DispatcherTimer(DispatcherPriority.Normal);
            this.timerShootUp.Interval = TimeSpan.FromMilliseconds(1);
            this.timerShootUp.Start();
            this.timerShootUp.Tick += new EventHandler(ShootUp);
        }

        void ShootUp(Object sender, EventArgs e)
        {
            if (this.point.Y >= 0)
            {
                this.point.Y -= 3;
                this.image.Source = UtilityMethods.LoadImage("pics/bullet.png");
                Canvas.SetTop(this.image, this.point.Y);
                
                if (enemies != null)
                {
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        OnCollision(enemies[i]);
                    }
                }
            }
            else
            {
                StopShootUp(); //stop bullet up
                this.Die();
            }
        }


        public void ShootDown(String path)
        {
            this.image.Source = UtilityMethods.LoadImage(path);

            this.timerShootDown = new DispatcherTimer(DispatcherPriority.Normal);
            this.timerShootDown.Interval = TimeSpan.FromMilliseconds(1);
            this.timerShootDown.Start();
            this.timerShootDown.Tick += new EventHandler(ShootDown);
        }

        void ShootDown(Object sender, EventArgs e)
        {
            if (this.point.Y <= 600)
            {
                this.point.Y += 3;
                Canvas.SetTop(this.GetImage(), this.point.Y);

                OnCollision(this.player);
            }
            else
            {
                StopShootDown(); //stop bullet down
                Die(); //remove bullet down
            }
        }

        public void ShootLeftSide(String path)
        {
            this.image.Source = UtilityMethods.LoadImage(path);
            this.timerShootLeftSide = new DispatcherTimer(DispatcherPriority.Normal);
            this.timerShootLeftSide.Interval = TimeSpan.FromMilliseconds(1);
            timerShootLeftSide.Tick += new EventHandler(ShootLeftSide);
        }
        void ShootLeftSide(Object sender, EventArgs e)
        {
            if (this.point.Y <= 600)
            {
                this.point.Y += 10;
                this.point.X -= 10;
                Canvas.SetTop(this.GetImage(), this.point.Y);
                Canvas.SetLeft(this.GetImage(), this.point.X);
                OnCollision(this.player);

            }
            else
            {
                StopShootLeft();
                this.canvas.Children.Remove(this.GetImage());
            }
        }

        public void ShootRightSide(String path)
        {
            this.image.Source = UtilityMethods.LoadImage(path);
            timerShootRightSide = new DispatcherTimer(DispatcherPriority.Normal);
            timerShootRightSide.Interval = TimeSpan.FromSeconds(1);
            timerShootRightSide.Tick += new EventHandler(ShootRightSide);
        }
        void ShootRightSide(Object sender, EventArgs e)
        {
            if (this.point.Y <= 600)
            {
                this.point.Y += 10;
                this.point.X += 10;
                Canvas.SetTop(this.GetImage(), this.point.Y);
                Canvas.SetLeft(this.GetImage(), this.point.X);
                OnCollision(this.player);

            }
            else
            {
                StopShootRight();
                this.Die();
            }
        }

        public void StopShootLeft()
        {
            if (this.timerShootLeftSide != null)
            {
                this.timerShootLeftSide.Stop();
            }
        }

        public void restartShootLeft()
        {
            if (this.timerShootLeftSide != null)
            {
                this.timerShootLeftSide.Start();
            }
        }

        public void StopShootRight()
        {
            if (this.timerShootRightSide != null)
            {
                this.timerShootRightSide.Stop();
            }
        }

        public void restartShootRight()
        {
            if (this.timerShootRightSide != null)
            {
                this.timerShootRightSide.Start();
            }
        }

        public void StopShootDown()
        {
            if (this.timerShootDown != null)
            {
                this.timerShootDown.Stop();
            }
        }

        public void restartShootDown()
        {
            if (this.timerShootDown != null)
            {
                this.timerShootDown.Start();
            }
        }

        public void StopShootUp()
        {
            if (this.timerShootUp != null)
            {
                this.timerShootUp.Stop();
            }
        }

        public void restartShootUp()
        {
            if (this.timerShootUp != null)
            {
                this.timerShootUp.Start();
            }
        }

        public override void Die()
        {
            canvas.Children.Remove(this.image); //remove bullet instance
            bullets.Remove(this);
        }

        public void OnCollision(GameObject gameObject)
        {
            double gameObjectX = Canvas.GetLeft(gameObject.GetImage());
            double gameObjectY = Canvas.GetTop(gameObject.GetImage());
            double bulletX = Canvas.GetLeft(this.image);
            double bulletY = Canvas.GetTop(this.image);

            Rect gameObjectRect = new Rect(gameObjectX, gameObjectY, gameObject.GetImage().Width - 5
                , gameObject.GetImage().Height - 5);
            Rect bulletRect = new Rect(bulletX, bulletY, this.GetImage().ActualWidth
                , this.GetImage().ActualHeight);

            if (gameObject.GetImage().IsLoaded == true) //if exists only
            {
                if (bulletRect.IntersectsWith(gameObjectRect)) //check intersection
                {
                    if (gameObject.GetType() == typeof(Commander))
                    {
                        OnCollisionCommander((Commander)gameObject);
                        StopShootUp();
                        Die();
                    }
                    else
                    {
                        destroy(gameObject);
                    }
                }
            }
        }

        public static explicit operator Bullet(UIElement v)
        {
            throw new NotImplementedException();
        }

        public void OnCollisionCommander(Commander commander)
        {
            if (commander.getIsShot() == true)
            {
                destroy(commander);
            }
            
            if (commander.getIsShot() == false)
            {
                commander.setIsShot(true);
                commander.changeImage();
            }
        }

        public void destroy(GameObject gameObject)
        {
            if (gameObject is Enemies)
            {
                StopShootUp(); //stop player's bullet               
                Enemies defeated = (Enemies)gameObject;
                enemies.Remove(defeated); //remove enemy from list
            }
            else
            {
                StopShootDown(); //stop enemy bullet
            }
            Die();//remove bullet
            gameObject.Die(); //player or enemy gone
        }
    }
}
