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
        static double coldDown;
        Bullet bullet;
        public GameSound shootSoundEffect = new GameSound(@"pack://application:,,,/GalagaX4;Component/audio/Firing.wav", true); 
        

        public static double ColdDown
        {
            get { return coldDown; }
            set { coldDown = value; }
        }

        double speed;
        List<Enemies> enemies;

        TextBlock displayLives;
        int lives = 3;
        Image[] hearts;

        TextBlock displayPoints;
        int points = 0;
        
        public Player(Point point, Image image, Canvas canvas
            , double speed) : base(point, image, canvas)
        {
            this.speed = speed;
            setDisplayLives();
           // updateLives();
            setDisplayPoints();
            updatePoints();
        }
        public Player() { }
        public int GetLives()
        {
            return this.lives;
        }

        public int getEnemiesSize()
        {
           if(this.enemies == null)
            {
                return 0;
            }
           else
            {
                return this.enemies.Count;
            }
        }

        public void setDisplayLives()
        {
            this.hearts = new Image[3];
            int spaceX = 0;
            for(int i = 0; i < hearts.Length; i++)
            {
                hearts[i] = new Image();
                hearts[i].Width = 34;
                hearts[i].Height = 26;
                this.canvas.Children.Add(hearts[i]);
                Canvas.SetLeft(hearts[i], 10 + spaceX);
                Canvas.SetTop(hearts[i], 10);
                spaceX += 30;
                hearts[i].Source = UtilityMethods.LoadImage("pics/heart.png");
            }
        }

       /* public void updateLives()
        {
            this.displayLives.Text = "LIVES: " + this.lives;
        }*/

        public void setDisplayPoints()
        {
            this.displayPoints = new TextBlock();
            canvas.Children.Add(this.displayPoints);
            Canvas.SetLeft(this.displayPoints, 760);
            Canvas.SetTop(this.displayPoints, 10);
            this.displayPoints.Foreground = new SolidColorBrush(Colors.Red);
            this.displayPoints.FontSize = 20;
        }

        public void updatePoints()
        {
            this.displayPoints.Text = ""+this.points;
        }

        public void addPoints(int morePoints)
        {
            this.points += morePoints;
            updatePoints();
        }

        public void SetEnemyTarget(List<Enemies> enemies)
        {
            this.enemies = enemies;
        }

        public void Move()
        {
            if (Keyboard.IsKeyDown(Key.Left))
            {
                this.point.X -= 1;
            }
            if (Keyboard.IsKeyDown(Key.Right))
            {
                this.point.X += 1;
            }
            
            this.point.X = UtilityMethods.Clamp(this.point.X, 2, 52);
            Canvas.SetLeft(this.image, this.point.X * speed);
        }

        public void Shoot()
        {            
            if (this.image.IsLoaded == true)
            {
                if (Keyboard.IsKeyDown(Key.Space))
                {
                    ShootUpdate();
                    coldDown++;
                }
            }
        }

        void ShootUpdate()
        {
            double position = Canvas.GetLeft(this.GetImage());
            double midOfImage = this.GetImage().Width / 2;

            Image bulletPic = new Image();
            bullet = new Bullet(this.point, bulletPic, canvas);
            bullet.setEnemyTarget(enemies);
            Canvas.SetLeft(bullet.GetImage(), position + midOfImage - 3.5);
            bullet.ShootUp();
            shootSoundEffect.playSound();
        }

        public void StopShootUp()
        {
            if(bullet != null)
                bullet.StopShootUp();
        }

        public double GetSpeed()
        {
            return this.speed;
        }

        public override void Die()
        {
            decrementLives();

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

        public void decrementLives()
        {
            if(this.lives == 3)
            {
                this.canvas.Children.Remove(hearts[2]);
            }
            else if(this.lives == 2)
            {
                this.canvas.Children.Remove(hearts[1]);
            }
            else
            {
                this.canvas.Children.Remove(hearts[0]);
            }
            this.lives--;
            
        }

        public void live()
        {
            if (lives > 0)
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
            else
            {
                Image gameOver = new Image();
                gameOver.Height = 200;
                gameOver.Width = 250;
                this.canvas.Children.Add(gameOver);
                Canvas.SetTop(gameOver, 200);
                Canvas.SetLeft(gameOver, 300);
                gameOver.Source = UtilityMethods.LoadImage("pics/gameOver.png");
            }
            //updateLives();
        }
    }
}
