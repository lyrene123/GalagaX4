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
        public GameSound explosionSoundEffect = new GameSound(@"pack://application:,,,/GalagaX4;Component/audio/Explosion.wav", true);
        int currentLevel;

        public static double ColdDown
        {
            get { return coldDown; }
            set { coldDown = value; }
        }

        double speed;
        List<Enemies> enemies;

        int lives = 3;
        List<Image> shipLives;
        Image newLife;

        TextBlock displayPoints;
        int points = 0;
        
        public Player(Point point, Image image, Canvas canvas
            , double speed) : base(point, image, canvas)
        {
            this.shipLives = new List<Image>();
            this.speed = speed;
            setDisplayLives();
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

        public List<Enemies> getEnemiesList()
        {
            return this.enemies;
        }

        public void updateCurrentLevel(int level)
        {
            this.currentLevel = level;
        }

        public int getCurrentLevel()
        {
            return this.currentLevel;
        }

        public void setDisplayLives()
        {
           // this.shipLives = new List<Image>(3);
            int spaceX = 0;
            for(int i = 0; i < 3; i++)
            {
                shipLives.Add(new Image());
                shipLives[i].Width = 34;
                shipLives[i].Height = 26;
                this.canvas.Children.Add(shipLives[i]);
                Canvas.SetLeft(shipLives[i], 745 + spaceX);
                Canvas.SetTop(shipLives[i], 585);
                spaceX += 30;
                shipLives[i].Source = UtilityMethods.LoadImage("pics/galaga_ship.png");
            }
        }

        public void setDisplayPoints()
        {
            this.displayPoints = new TextBlock();
            canvas.Children.Add(this.displayPoints);
            Canvas.SetLeft(this.displayPoints, 750);
            Canvas.SetTop(this.displayPoints, 10);
            this.displayPoints.Foreground = new SolidColorBrush(Colors.White);
            this.displayPoints.FontSize = 20;
        }

        public void updatePoints()
        {
            this.displayPoints.Text = " x "+this.points;
        }

        public void addPoints(int morePoints)
        {
            this.points += morePoints;
            updatePoints();
        }

        public void addLife()
        {       
            if (this.lives == 2)
            {
                this.newLife = new Image();
                this.shipLives.Insert(0, newLife);
                newLife.Width = 34;
                newLife.Height = 26;
                canvas.Children.Add(newLife);
                double posX = Canvas.GetLeft(shipLives[1]);
                Canvas.SetLeft(newLife, posX - 30);
                Canvas.SetTop(newLife, 585);
                newLife.Source = UtilityMethods.LoadImage("pics/galaga_ship.png");
            }
            else
            {
                this.newLife = new Image();
                this.shipLives.Insert(0, newLife);
                newLife.Width = 34;
                newLife.Height = 26;
                canvas.Children.Add(newLife);
                double posX = Canvas.GetLeft(shipLives[shipLives.Count-1]);
                Canvas.SetLeft(newLife, posX - 30);
                Canvas.SetTop(newLife, 585);
                newLife.Source = UtilityMethods.LoadImage("pics/galaga_ship.png");
            }
            this.lives++;
            this.points = this.points - 2000;
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
            explosionSoundEffect.playSound();
            //await Task.Delay(1000);
            live();
        }

        public void decrementLives()
        {
            if (this.shipLives.Count != 0)
            {
                this.canvas.Children.Remove(shipLives[0]);
                this.shipLives.RemoveAt(0);
            }

            /*if(this.lives == 3)
            {
                this.canvas.Children.Remove(shipLives[0]);
            }
            else if(this.lives == 2)
            {
                this.canvas.Children.Remove(shipLives[1]);
            }
            else
            {
                this.canvas.Children.Remove(shipLives[2]);
            }*/
            this.lives--;        
        }

        public async void live()
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
                await Task.Delay(1500);  
                this.image.Source = UtilityMethods.LoadImage("pics/galaga_ship.png");
            }
            else
            {
                explosionSoundEffect.StopSound();
                explosionSoundEffect.Dispose(); 
            }
        }
    }
}
