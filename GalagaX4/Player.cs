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
    /// <summary>
    /// The Player class which extends the GameObject Class defines 
    /// all attributes and behavior of the player in a galaga game. 
    /// </summary>
    class Player : GameObject
    {
        static double coldDown;
        Bullet bullet;
        //Sound Effects of the bullet and explosion of the player
        public GameSound shootSoundEffect = new GameSound(@"pack://application:,,,/GalagaX4;Component/audio/Firing.wav", true);
        public GameSound explosionSoundEffect = new GameSound(@"pack://application:,,,/GalagaX4;Component/audio/Explosion.wav", true);
        int currentLevel;
        /// <summary>
        /// The static ColdDown method gets or sets the value 
        /// of the coldDown of type Double.
        /// </summary>
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
        /// <summary>
        /// The Player constructor sets the position of the game element,
        /// the image and the game canvas from the values inputed into the constructor
        /// </summary>
        /// <param name="point">Position of the game elements of type Point</param>
        /// <param name="image">The image of the game elements </param>
        /// <param name="canvas">The game canvas in which the game elements is in</param>
        /// <param name="speed">The Frequency value of type Double</param>
        public Player(Point point, Image image, Canvas canvas
            , double speed) : base(point, image, canvas)
        {
            this.shipLives = new List<Image>();
            this.speed = speed;
            setDisplayLives();
            setDisplayPoints();
            updatePoints();
        }
        /// <summary>
        /// The Player Constructor without parameters to give access
        /// to useful methods provided by this Class.
        /// </summary>
        public Player() { }
        /// <summary>
        /// The GetLives methods return the number of lives of the player.
        /// </summary>
        /// <returns>lives of type int</returns>
        public int GetLives()
        {
            return this.lives;
        }
        /// <summary>
        /// The getEnemiesSize method returns the number of elements contained 
        /// in the list of enemies.
        /// </summary>
        /// <returns>List of type enemies the number of elements contained in the list of enemies.</returns>
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
        /// <summary>
        /// The getEnemiesList method returns the list of enemies created.
        /// </summary>
        /// <returns>List of enemies of type Enemies</returns>
        public List<Enemies> getEnemiesList()
        {
            return this.enemies;
        }
        /// <summary>
        /// The updateCurrentLevel method updates the level of the game.
        /// </summary>
        /// <param name="level">A value of type int indicating the level of the game</param>
        public void updateCurrentLevel(int level)
        {
            this.currentLevel = level;
        }
        /// <summary>
        /// The getCurrentLevel returns the current level of the game.
        /// </summary>
        /// <returns>currentlevel A value of type int indicating the level of the game</returns>
        public int getCurrentLevel()
        {
            return this.currentLevel;
        }
        /// <summary>
        /// The setDisplayLives displays the number of lives on the screen in form of the player image.
        /// </summary>
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
        /// <summary>
        /// The setDisplayPoints method display the number of points gained by the player
        /// after killing enemies.
        /// </summary>
        public void setDisplayPoints()
        {
            this.displayPoints = new TextBlock();
            canvas.Children.Add(this.displayPoints);
            Canvas.SetLeft(this.displayPoints, 750);
            Canvas.SetTop(this.displayPoints, 10);
            this.displayPoints.Foreground = new SolidColorBrush(Colors.White);
            this.displayPoints.FontSize = 20;
        }
        /// <summary>
        /// The updatePoints method updates the points gained by the player.
        /// </summary>
        public void updatePoints()
        {
            this.displayPoints.Text = " x "+this.points;
        }
        /// <summary>
        /// The addCoins method adds points to the player and also invokes the updatePoints method.
        /// </summary>
        /// <param name="morePoints">points A value of type int</param>
        public void addCoins(int morePoints)
        {
            this.points += morePoints;
            updatePoints();
        }
        /// <summary>
        /// The getCoins method returns the number of points.
        /// </summary>
        /// <returns>points A value of type int</returns>
        public int getCoins()
        {
            return this.points;
        }
        /// <summary>
        /// The addLife method adds life to player and sets the image on the screen.
        /// </summary>
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
        /// <summary>
        /// The SetEnemyTarget method sets the List of enemies.
        /// </summary>
        /// <param name="enemies"></param>
        public void SetEnemyTarget(List<Enemies> enemies)
        {
            this.enemies = enemies;
        }
        /// <summary>
        /// The Move method is used to move the player to the left or right according 
        /// to te user keyboard movements of left arrow or right arrow pressed. 
        /// </summary>
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
        /// <summary>
        /// The Shoot method invokes the ShootUpdate method after the player press the Space bar Keyboard
        /// and also increases the ColdDown element so that the player looses the power to shoot more than
        /// one bullet at a time.
        /// </summary>
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
        /// <summary>
        /// The ShootUpdate method updates the position of the bullet in the screen
        /// giving the illusion that the bullet is moving like a real bullet.
        /// </summary>
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
        /// <summary>
        /// The StopShootUp method invokes the the StopShootup method
        /// from the Bullet Class if the bullet is set to null.
        /// </summary>
        public void StopShootUp()
        {
            if(bullet != null)
                bullet.StopShootUp();
        }
        /// <summary>
        /// The getSpeed method returns the speed frequency.
        /// </summary>
        /// <returns>speed A value of type double</returns>
        public double GetSpeed()
        {
            return this.speed;
        }
        /// <summary>
        /// The overriden Die method initiates the animation of the explosion of
        /// the Player once shot already. The method also stops the explosion 
        /// sound effect and dispose its resources invoking the Dispose method
        /// from the GameSound Class.
        /// </summary>
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
            explosionSoundEffect.Dispose();
            live();
        }
        /// <summary>
        /// The decrementLives method decrements the lives of the player if the player
        /// gets hit.
        /// </summary>
        public void decrementLives()
        {
            if (this.shipLives.Count != 0)
            {
                this.canvas.Children.Remove(shipLives[0]);
                this.shipLives.RemoveAt(0);
            }
            this.lives--;        
        }
        /// <summary>
        /// The live method creates a new instance image of the player if the lives still greater than 0.
        /// If the lives is not greater than 0 the explosion of the soundeffect stops and it is Disposed.
        /// </summary>
        public async void live()
        {              
            if (lives > 0)
            {
                coldDown = 0;

                this.image = new Image();
                this.image.Height = 46;
                this.image.Width = 42;
                this.canvas.Children.Add(this.image);
                Canvas.SetTop(this.image, 500);
                Canvas.SetLeft(this.image, 405);
                this.SetPointX(27);
                this.SetPointY(490);
                await Task.Delay(1200);  
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
