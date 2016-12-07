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
    /// <summary>
    /// The SpaceShip class which implements the Enemies interface defines 
    /// a concrete implementation of the an Enemies type. The SpaceShip class
    /// contains the attributes and behavior of the spaceShip enemy in a galaga game
    /// </summary>
    class SpaceShip : Enemies
    {
        DispatcherTimer timerFly;
        DispatcherTimer timerShoot;
        /// <summary>
        /// The SpaceShip Constructor without parameters 
        /// to access useful methods in the class.
        /// </summary>
        public SpaceShip() : base()
        {
        }
        /// <summary>
        /// The SpaceShip constructor initializes the attributes of the SpaceShip 
        /// class and the attributes inherited by the Enemies interface. The SpaceShip 
        /// class takes as input a position, an image of a player and the game canvas.
        /// </summary>
        /// <param name="point">Position of type Point</param>
        /// <param name="image">an image of a player</param>
        /// <param name="canvas">the game canvas</param>
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
        /// <summary>
        /// The overridden Fly method takes as input a double for the frequency
        /// to call the MoveHorizontal method for the moving of the spaceShip enemy on the screen.
        /// </summary>
        /// <param name="frequency">double value for the speed of the moving</param>
        public override void Fly(double frequency)
        {
            this.flyFrequency = frequency;
            timerFly = new DispatcherTimer(DispatcherPriority.Normal);
            timerFly.Interval = TimeSpan.FromMilliseconds(frequency);
            timerFly.Tick += new EventHandler(MoveHorizontal);
            timerFly.Start();
        }
        /// <summary>
        /// The MoveHorizontal event handler method is called by the Fly method
        /// with a timer that constantly calls this method. This method handles the horizontal
        /// moving of the spaceShip enemy on the screen by making sure that this enemy doesn't
        /// move pass the limits of the screen while checking for Player collision. 
        /// </summary>
        /// <param name="sender">object raising the MoveHorizontal event</param>
        /// <param name="e">The MoveHorizontal event raised</param>
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
        /// <summary>
        /// The updateMoveDown event handler method initiates the dive sequence of the 
        /// spaceShip enemy. The updateMoveDown method is called by the MoveHorizontal method with a timer
        /// </summary>
        /// <param name="sender">object that raised the updateMoveDown event</param>
        /// <param name="e">The updateMoveDown event </param>
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
        /// <summary>
        /// The isShoot method return the state of the enemy shootings.
        /// </summary>
        /// <returns>True or false depending if the enemy is shooting or not</returns>
        public bool isShoot()
        {
            return this.isShooting;
        }
        /// <summary>
        /// The isDead method return the state of the enemy.
        /// </summary>
        /// <returns>True or false depending if the enemy is still alive</returns>
        public bool IsDead()
        {
            return this.dead;
        }
        /// <summary>
        /// The overridden Shoot method initiates the timer for the shooting 
        /// of the SpaceShip enemy with a specific frequency and calls the Shoot method event 
        /// handler 
        /// </summary>
        /// <param name="frequency">double value for the speed of the moving</param>
        public override void Shoot(double frequency)
        {
            timerShoot = new DispatcherTimer(DispatcherPriority.Normal);
            timerShoot.Interval = TimeSpan.FromSeconds(frequency);
            timerShoot.Tick += new EventHandler(Shoot);
            timerShoot.Start();
        }
        /// <summary>
        /// Shoot method event handler sets the isShooting boolean to true
        /// and creates a Bullet instance and calls the shootDown method of the 
        /// Bullet class with the right position depending of the SpaceShip's
        /// position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// The stopMove method stops the timer for the moving and the timer for the SpaceShip enemy.
        /// </summary>
        public void stopMove()
        {
            this.timerFly.Stop();
        }
        /// <summary>
        /// The restartMove method starts the timer for the moving and the timer
        /// for SpaceShip enemy.
        /// </summary>
        public void restartMove()
        {
            this.timerFly.Start();
        }
        /// <summary>
        /// The stopShoot method stops the timer for the shooting
        /// </summary>
        public void StopShoot()
        {
            if (this.timerShoot != null)
            {
                this.timerShoot.Stop();
            }
        }
        /// <summary>
        /// The restartShoot method starts the timer for the shooting
        /// </summary>
        public void restartShoot()
        {
            if (this.timerShoot != null)
            {
                this.timerShoot.Start();
            }
        }

        /// <summary>
        /// The overriden Die method initiates the animation of the explosion of
        /// the SpaceShip enemy once shot already. The method also increases
        /// the points of the player if the spaceShip is eliminated and stops the
        /// timer for moving and shooting. 
        /// </summary>
        public override void Die()
        {
            this.dead = true;
            this.target.addCoins(200);
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
