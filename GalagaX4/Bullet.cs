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
    /// <summary>
    /// The Bullet class defines the attributes and the different behaviors
    /// possible for a bullet shot by player and enemies in a galaga game. 
    /// The Bullet class defines the shoot down, up, left and right also
    /// takes care of the collision of a bullet with a player or an enemy.
    /// The Bullet class extends from GameObject abstract class as concret 
    /// implementation
    /// </summary>
    class Bullet : GameObject
    {
        //different timers depending on the direction of the shooting
        DispatcherTimer timerShootUp;
        DispatcherTimer timerShootDown;
        DispatcherTimer timerShootRightSide;
        DispatcherTimer timerShootLeftSide;

        List<Enemies> enemies;
        static List<Bullet> bullets = new List<Bullet>();
        Player player;

        /// <summary>
        /// The Bullet no parameter method sets all attributes to default value
        /// </summary>
        public Bullet() : base()
        {
        }

        /// <summary>
        /// The Bullet class takes as input a position for the bullet, an image, the
        /// game canvas and sets these values and adds the bullet image to the game canvas
        /// </summary>
        /// <param name="point">Point position for the bullet</param>
        /// <param name="image">Image for the bullet</param>
        /// <param name="canvas">Game canvas</param>
        public Bullet(Point point, Image image, Canvas canvas) : base(point, image, canvas)
        {
            this.canvas.Children.Add(image);
            bullets.Add(this);
        }

        /// <summary>
        /// The static getBulletList method returns the list of bullets
        /// present in the game canvas
        /// </summary>
        public static List<Bullet> getBulletList
        {
            get{ return bullets; }
        }

        /// <summary>
        /// The setEnemyTarget method sets the list of enemies of the player
        /// as target
        /// </summary>
        /// <param name="enemies">List of type enemies</param>
        public void setEnemyTarget(List<Enemies> enemies)
        {
            this.enemies = enemies;
        }

        /// <summary>
        /// The setPlayerTarget method sets the player as target for the 
        /// enemies
        /// </summary>
        /// <param name="player">Object of type Player</param>
        public void setPlayerTarget(Player player)
        {
            this.player = player;
        }

        /// <summary>
        /// The ShootUp method initiates the timer for the shoot up sequence
        /// of the bullet and calls the event handler method for shoot up
        /// </summary>
        public void ShootUp()
        {
            this.timerShootUp = new DispatcherTimer(DispatcherPriority.Normal);
            this.timerShootUp.Interval = TimeSpan.FromMilliseconds(1);
            this.timerShootUp.Start();
            this.timerShootUp.Tick += new EventHandler(ShootUp);
        }

        /// <summary>
        /// The ShootUp method handler handles the shoot up sequence of 
        /// a bullet and it is called by the ShootUp method
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">The ShootUp event </param>
        void ShootUp(Object sender, EventArgs e)
        {
            //move bullet up until it reaches to the top
            if (this.point.Y >= 0)
            {
                this.point.Y -= 3;
                this.image.Source = UtilityMethods.LoadImage("pics/bullet.png");
                Canvas.SetTop(this.image, this.point.Y);
                
                //check for player collision
                if (enemies != null)
                {
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        OnCollision(enemies[i]);
                    }
                }
            }
            else //stop bullet movement and remove it if it reaches the top
            {
                StopShootUp(); //stop bullet up
                this.Die();
            }
        }

        /// <summary>
        /// The ShootDown method initiates the timer for shooting down
        /// and calls the event handler method for shooting down
        /// </summary>
        /// <param name="path">image path of the bullet</param>
        public void ShootDown(String path)
        {
            this.image.Source = UtilityMethods.LoadImage(path);

            this.timerShootDown = new DispatcherTimer(DispatcherPriority.Normal);
            this.timerShootDown.Interval = TimeSpan.FromMilliseconds(1);
            this.timerShootDown.Start();
            this.timerShootDown.Tick += new EventHandler(ShootDown);
        }

        /// <summary>
        /// The ShootDown method handler handles the shoot up sequence of 
        /// a bullet and it is called by the ShootDown method
        /// </summary>
        /// <param name="sender">The object raising the event</param>
        /// <param name="e">The ShootDown event</param>
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

        /// <summary>
        /// The ShootLeftSide method initiates the timerShootLeftSide timer to call
        /// the ShootLeftSide method handler and takes as input a string path for an 
        /// image of the bullet
        /// </summary>
        /// <param name="path"></param>
        public void ShootLeftSide(String path)
        {
            this.image.Source = UtilityMethods.LoadImage(path);
            this.timerShootLeftSide = new DispatcherTimer(DispatcherPriority.Normal);
            this.timerShootLeftSide.Interval = TimeSpan.FromMilliseconds(1);
            timerShootLeftSide.Tick += new EventHandler(ShootLeftSide);
        }

        /// <summary>
        /// The ShootLeftSide method event handler handles the shooting at the left
        /// side and it is called by the ShootLeftSide method
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">The event ShootLeftSide raised </param>
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

        /// <summary>
        /// The ShootRightSide method takes as input a string path for 
        /// the bullet image and initiates the timer for the timerShootRightSide
        /// timer which calls the ShootRightSide event handler method
        /// </summary>
        /// <param name="path"></param>
        public void ShootRightSide(String path)
        {
            this.image.Source = UtilityMethods.LoadImage(path);
            timerShootRightSide = new DispatcherTimer(DispatcherPriority.Normal);
            timerShootRightSide.Interval = TimeSpan.FromSeconds(1);
            timerShootRightSide.Tick += new EventHandler(ShootRightSide);
        }

        /// <summary>
        /// The ShootRightSide method event handler handles the shooting of
        /// the bullet the right side
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">The ShootRightSide event</param>
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

        /// <summary>
        /// StopShootLeft method stops the timerShootLeftSide timer
        /// </summary>
        public void StopShootLeft()
        {
            if (this.timerShootLeftSide != null)
            {
                this.timerShootLeftSide.Stop();
            }
        }

        /// <summary>
        /// restartShootLeft method starts the timerShootLeftSide timer
        /// </summary>
        public void restartShootLeft()
        {
            if (this.timerShootLeftSide != null)
            {
                this.timerShootLeftSide.Start();
            }
        }

        /// <summary>
        /// StopShootRight method stops the timerShootRightSide timer
        /// </summary>
        public void StopShootRight()
        {
            if (this.timerShootRightSide != null)
            {
                this.timerShootRightSide.Stop();
            }
        }

        /// <summary>
        /// restartShootRight method starts the timerShootRightSide timer
        /// </summary>
        public void restartShootRight()
        {
            if (this.timerShootRightSide != null)
            {
                this.timerShootRightSide.Start();
            }
        }

        /// <summary>
        /// StopShootDown method stops the timerShootDown timer
        /// </summary>
        public void StopShootDown()
        {
            if (this.timerShootDown != null)
            {
                this.timerShootDown.Stop();
            }
        }

        /// <summary>
        /// restartShootDown method starts the timerShootDown timer
        /// </summary>
        public void restartShootDown()
        {
            if (this.timerShootDown != null)
            {
                this.timerShootDown.Start();
            }
        }

        /// <summary>
        /// StopShootUp method stop the timerShootUp timer
        /// </summary>
        public void StopShootUp()
        {
            if (this.timerShootUp != null)
            {
                this.timerShootUp.Stop();
            }
        }

        /// <summary>
        /// restartShootUp method starts the timerShootUp timer
        /// </summary>
        public void restartShootUp()
        {
            if (this.timerShootUp != null)
            {
                this.timerShootUp.Start();
            }
        }

        /// <summary>
        /// the overridden Die method removes the bullet from the canvas
        /// and removes from the list of Bullets present on the canvas
        /// </summary>
        public override void Die()
        {
            canvas.Children.Remove(this.image); //remove bullet instance
            bullets.Remove(this);
        }

        /// <summary>
        /// OnCollision method takes as input an object of type GameObject
        /// and checks if the current of bullet touches that GameObject instance
        /// </summary>
        /// <param name="gameObject">instance of GameObject</param>
        public void OnCollision(GameObject gameObject)
        {
            //position x and y of gameObject and bullet
            double gameObjectX = Canvas.GetLeft(gameObject.GetImage());
            double gameObjectY = Canvas.GetTop(gameObject.GetImage());
            double bulletX = Canvas.GetLeft(this.image);
            double bulletY = Canvas.GetTop(this.image);

            //draw rectangle for bullet and gameObject
            Rect gameObjectRect = new Rect(gameObjectX, gameObjectY, gameObject.GetImage().Width - 5
                , gameObject.GetImage().Height - 5);
            Rect bulletRect = new Rect(bulletX, bulletY, this.GetImage().ActualWidth
                , this.GetImage().ActualHeight);

            if (gameObject.GetImage().IsLoaded == true) //if gameObject exists only
            {
                if (bulletRect.IntersectsWith(gameObjectRect)) //check intersection
                {
                    //if gameObject is type Commander do this: 
                    if (gameObject.GetType() == typeof(Commander))
                    {
                        //call the method collision specialized for the commander
                        OnCollisionCommander((Commander)gameObject);
                        StopShootUp(); //stop player bullet
                        Die(); //remove player bullet
                    }
                    else
                    {
                        destroy(gameObject); //remove gameObject
                    }
                }
            }
        }

        /// <summary>
        /// OnCollisionCommander method takes as input a commander 
        /// and checks if the commander has been shot once or twice. If 
        /// once, then then change the image of the commander. If twice,
        /// then eliminate the commander from canvas
        /// </summary>
        /// <param name="commander">Commander object</param>
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

        /// <summary>
        /// The destroy method takes as input a gameObject
        /// and eliminates that gameObject from the canvas
        /// </summary>
        /// <param name="gameObject">object of type GameObject</param>
        public void destroy(GameObject gameObject)
        {
            //if gameObject is type of Enemies, do the following:
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
