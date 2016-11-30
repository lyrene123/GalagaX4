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
        DispatcherTimer timer;
        List<Enemies> enemies;
        Player player;

        public Bullet() : base()
        {
           
        }

        public Bullet(Point point, Image image, Canvas canvas) : base(point, image, canvas)
        {
            this.canvas.Children.Add(image);

            timer = new DispatcherTimer(DispatcherPriority.Normal);
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Start();
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
            timer.Tick += new EventHandler(ShootUp);
        }

        void ShootUp(Object sender, EventArgs e)
        {
            if (this.point.Y >= 0)
            {
                this.point.Y -= 10;
                this.image.Source = UtilityMethods.LoadImage("pics/bullet.png");
                Canvas.SetTop(this.image, this.point.Y);

                for (int i = 0; i < enemies.Count; i++)
                {
                    OnCollision(enemies[i]);
                }

            }
            else
            {
                stopMove();
                this.canvas.Children.Remove(this.image);
                for(int i = 0; i<this.enemies.Count(); i++)
                {
                    if(enemies[i].GetType() == typeof(Commander))
                    {
                        Commander commander = (Commander)enemies[i];

                        if (commander.getShotValue() > 1 && commander.getIsShot() == false)
                        {
                            commander.setIsShot(true);
                            commander.changeImage();
                        }
                    }
                }
            }
        }

        public void ShootDown()
        {
            timer.Tick += new EventHandler(ShootDown);
        }

        void ShootDown(Object sender, EventArgs e)
        {
            if (this.point.Y <= 600)
            {
                this.point.Y += 10;
                this.image.Source = UtilityMethods.LoadImage("pics/bulletFlip180.png");
                Canvas.SetTop(this.GetImage(), this.point.Y);

                OnCollision(this.player);
            }
            else
            {
                stopMove();
                this.canvas.Children.Remove(this.GetImage());

            }
        }

        public void stopMove()
        {
            timer.Stop();
           // timer = null;
        }

        public override void Die()
        {
            canvas.Children.Remove(this.image);
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

            if (gameObject.GetImage().IsLoaded == true)
            {
                if (bulletRect.IntersectsWith(gameObjectRect))
                {
                    if (gameObject.GetType() == typeof(Commander))
                    {
                        OnCollisionCommander((Commander)gameObject);
                        this.Die();
                    }
                    else
                    {
                        destroy(gameObject);
                    }
                }
            }
        }

        public void OnCollisionCommander(Commander commander)
        {
            commander.addShotValue();
            if (commander.getIsShot() == true)
            {
                destroy(commander);
            }        
        }

        public void destroy(GameObject gameObject)
        {
            this.stopMove(); //bullet gone 
            this.Die(); //remove bullet image
            if (gameObject is Enemies)
            {
                Enemies defeated = (Enemies)gameObject;
                enemies.Remove(defeated); //remove enemy from list
            }
            gameObject.Die(); //player or enemy gone
        }
    }
}
