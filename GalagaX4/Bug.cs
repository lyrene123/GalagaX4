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
    /// The Bug class which implements the Enemies interface defines 
    /// a concrete implementation of the an Enemies type. The Bug class
    /// contains the attributes and behavior of the bugs in a galaga game
    /// </summary>
    class Bug : Enemies
    {

        DispatcherTimer timerFly; //timerRandomShoot for moving

        /// <summary>
        /// The Bug constructor initializes the attributes of the Bug
        /// class and the attributes inherited by the Enemies interface. The Bug
        /// class takes as input a position, an image of a commander, the game canvas,
        /// the animation for the bugs.
        /// </summary>
        /// <param name="point">Position of type Point</param>
        /// <param name="image">an image of a bug</param>
        /// <param name="canvas">the game canvas</param>
        /// <param name="animation">instance of the Animation class</param>
        public Bug(Point point, Image image, Canvas canvas, Animation animation)
            : base(point, image, canvas, animation)
        {
            this.moveCounter = 1;
            this.moveDown = false;
            this.dive = false;
            this.moveDownFrequency = 35;
            this.diveFrequency = 5;
        }
        /// <summary>
        /// The overridden Fly method takes as input a double for the frequency
        /// to call the startFly method for the moving of the bugs on the screen
        /// and the method initiates the animation of the bugs's wings as well
        /// </summary>
        /// <param name="frequency">double value for the speed of the moving</param>
        public override void Fly(double frequency)
        {
            Animation.Initiate(this.animation, 200);
            startFly(frequency);
        }
        /// <summary>
        /// The startFly method takes as input a double value for frequency of the
        /// moving of the bug on the screen and initiates the timer for moving
        /// with that frequency value
        /// </summary>
        /// <param name="frequency">double value for the speed of the moving</param>

        public void startFly(double frequency)
        {
            this.flyFrequency = frequency;
            this.timerFly = new DispatcherTimer(DispatcherPriority.Render);
            this.timerFly.Interval = TimeSpan.FromMilliseconds(frequency);
            this.timerFly.Tick += new EventHandler(this.updateMoveHorizontal);
            this.timerFly.Start();
        }
        /// <summary>
        /// The updateMoveHorizontal event handler method is called by the startFly method
        /// with a timer that constantly calls this method. This method handles the horizontal
        /// moving of the bug on the screen by making sure that the bug doesn't
        /// move pass the limits of the screen while checking for player collision. 
        /// The method handles as well the random dive sequence of the bug
        /// if the boolean dive is set to true
        /// </summary>
        /// <param name="sender">object raising the updateMoveHorizontal event</param>
        /// <param name="e">The updateMoveHorizontal event raised</param>
        private void updateMoveHorizontal(object sender, EventArgs e)
        {
            double beeX = this.GetPoint().X;

            if (this.point.Y <= 500)
            {
                //move down
                if (this.moveDown == true)
                {
                    this.point.Y += this.moveDownFrequency;
                    Canvas.SetTop(this.GetImage(), this.point.Y);
                    this.moveDown = false;
                    playerCollision();

                    //random dive
                    if (dive == true)
                    {
                        Random rand = new Random();
                        int randNum = rand.Next(20);
                        if (randNum % 2 == 0)
                        {
                            this.timerFly.Stop();
                            this.timerFly = new DispatcherTimer(DispatcherPriority.Render);
                            this.timerFly.Interval = TimeSpan.FromMilliseconds(120);
                            this.timerFly.Tick += new EventHandler(this.updateMoveDown);
                            this.timerFly.Start();
                        }
                    }
                }

                //move horizontal
                if (moveCounter == 1)
                {
                    if (beeX >= minX)
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
                    if (beeX <= maxX)
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
        /// bug. The updateMoveDown method is called by the startFly method with a timer
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
        /// The stopMove method stops the timer for the moving and the timer for the animation
        /// of the bug's wings
        /// </summary>
        public void stopMove()
        {
            this.timerFly.Stop(); //stop moving
            this.animation.Stop(); //stop animation
        }
        /// <summary>
        /// The restartMove method starts the timer for the moving and the timer
        /// for the animation of the bug' wings
        /// </summary>
        public void restartMove()
        {
            this.timerFly.Start();
            this.animation.Start();
        }
        /// <summary>
        /// The overridden Shoot method initiates the timer for the shooting 
        /// of the bug with a specific frequency and calls the Shoot method event 
        /// handler 
        /// </summary>
        /// <param name="frequency">double value for the speed of the moving</param>
        public override void Shoot(double frequency)
        {
            //bugs do not implement shooting
        }
        /// <summary>
        /// The overriden Die method initiates the animation of the explosion of
        /// the bug once shot already. The method also increases
        /// the points of the player if the bug is eliminated and stops the
        /// timer for moving and shooting of the bug. 
        /// </summary>
        public override void Die()
        {
            this.target.addCoins(100);
            BitmapImage[] explosions =
            {
                UtilityMethods.LoadImage("pics/explosions/enemiesExp0.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp1.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp2.png"),
                UtilityMethods.LoadImage("pics/explosions/enemiesExp3.png")
            };

            this.stopMove();
            this.animation = new Animation(this.image, explosions, false, canvas);
            Animation.Initiate(this.animation, 40);
        }
    }
}
