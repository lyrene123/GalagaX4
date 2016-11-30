using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GalagaX4
{
    class Player : GameObject
    {
        double speed;
        List<Enemies> enemies;

        TextBlock displayLives;
        int lives = 3;

        TextBlock displayPoints;
        int points = 0;

        public Player(Point point, Image image, Canvas canvas
            , double speed) : base(point, image, canvas)
        {
            this.speed = speed;
            setDisplayLives();
            updateLives();
            setDisplayPoints();
            updatePoints();        
        }

        public void setDisplayLives()
        {
            this.displayLives = new TextBlock();
            canvas.Children.Add(this.displayLives);
            this.displayLives.Text = "LIVES: ";
            this.displayLives.Foreground = new SolidColorBrush(Colors.Red);
            this.displayLives.FontSize = 20;
        }

        public void updateLives()
        {
            this.displayLives.Text = "LIVES: " + this.lives;
        }

        public void setDisplayPoints()
        {
            this.displayPoints = new TextBlock();
            canvas.Children.Add(this.displayPoints);
            Canvas.SetLeft(this.displayPoints, 600);
            this.displayPoints.Text = "COINS: ";
            this.displayPoints.Foreground = new SolidColorBrush(Colors.Red);
            this.displayPoints.FontSize = 20;
        }

        public void updatePoints()
        {
            this.displayPoints.Text = "COINS: " + this.points;
        }

        public void addPoints(int morePoints)
        {
            this.points = morePoints;
            updatePoints();
        }

        public void SetEnemyTarget(List<Enemies> enemies)
        {
            this.enemies = enemies;
        }

        public void Move(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    this.point.X -= 1;
                    break;
                case Key.Right:
                    this.point.X += 1;
                    break;
            }

            this.point.X = UtilityMethods.Clamp(this.point.X, 2, 52);
            Canvas.SetLeft(this.image, this.point.X * speed);
        }

        public void Shoot(object sender, KeyEventArgs e)
        {
            if (this.image.IsLoaded == true)
            {
                if (e.Key == Key.Space)
                {
                    Shoot();
                }
            }
        }

        void Shoot()
        {
            double position = Canvas.GetLeft(this.GetImage());
            double midOfImage = this.GetImage().Width / 2;

            Image bulletPic = new Image();
            Bullet bullet = new Bullet(this.point, bulletPic, canvas);
            bullet.setEnemyTarget(enemies);
            Canvas.SetLeft(bullet.GetImage(), position + midOfImage - 3.5);

            bullet.ShootUp();
        }

        public double GetSpeed()
        {
            return this.speed;
        }

        public override void Die()
        {
            lives--; //lives xx xx

            BitmapImage[] explosions =
            {
                UtilityMethods.LoadImage("pics/explosions/explosion0.png"),
                UtilityMethods.LoadImage("pics/explosions/explosion1.png"),
                UtilityMethods.LoadImage("pics/explosions/explosion2.png"),
                UtilityMethods.LoadImage("pics/explosions/explosion3.png")
            };

            Animation animation = new Animation(this.image, explosions, false, canvas);
            Animation.Initiate(animation, 100);
            live();

        }

        public void live()
        {
            if (lives>0)
            {              
               this.image = new Image();
                this.image.Height = 46;
                this.image.Width = 42;
                this.canvas.Children.Add(this.image);
                Canvas.SetTop(this.image, 500);
                Canvas.SetLeft(this.image, 405);
                this.SetPointX(27);
                this.SetPointY(490);         
                this.image.Source = UtilityMethods.LoadImage("pics/galaga_ship.png");
            }

            updateLives();
        }
    }
}
