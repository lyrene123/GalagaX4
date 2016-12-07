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
    /// The Commander class which implements the Enemies interface defines 
    /// a concrete implementation of the an Enemies type. The Commander class
    /// contains the attributes and behavior of a Commander enemy in a galaga game
    /// </summary>
    class Commander : Enemies
    {
        bool isShot; //boolean to check if the commander is shot once already
        DispatcherTimer timerFly; //timer for moving
        DispatcherTimer timerShoot; //timer for shooting

        /// <summary>
        /// The Commander constructor initializes the attributes of the Commander
        /// class and the attributes inherited by the Enemies interface. The Commander
        /// class takes as input a position, an image of a commander, the game canvas,
        /// the animation for the commander.
        /// </summary>
        /// <param name="point">Position of type Point</param>
        /// <param name="image">an image of a commander</param>
        /// <param name="canvas">the game canvas</param>
        /// <param name="animation">instance of the Animation class</param>
        public Commander(Point point, Image image, Canvas canvas, Animation animation)
            : base(point, image, canvas, animation)
        {
            this.moveCounter = 1;
            this.moveDown = false;
            this.dive = false;

            this.isShot = false; 
            this.dead = false; 
            this.isShooting = false; 

            this.diveFrequency = 5; //default value if not set
            this.moveDownFrequency = 35; //default value if not set
        }

        /// <summary>
        /// The isShoot method returns a boolean if the commander
        /// has been shot once already or not
        /// </summary>
        /// <returns>a boolean true or false</returns>
        public bool isShoot()
        {
            return this.isShooting;
        }

        /// <summary>
        /// The isDead method returns a boolean if the commander has already 
        /// been destroyed or not
        /// </summary>
        /// <returns>a boolean true or false</returns>
        public bool IsDead()
        {
            return this.dead;
        }

        /// <summary>
        /// The overriden Die method initiates the animation of the explosion of
        /// the commander once shot twice already. The method also increases
        /// the points of the player if the commander is eliminated and stops the
        /// timer for moving and shooting of the commander. 
        /// </summary>
        public override void Die()
        {
            this.dead = true;
            this.target.addCoins(250);
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

        /// <summary>
        /// The overridden Fly method takes as input a double for the frequency
        /// to call the startFly method for the moving of the commander on the screen
        /// and the method initiates the animation of the commander's wings as well
        /// </summary>
        /// <param name="frequency">double value for the speed of the moving</param>
        public override void Fly(double frequency)
        {
            Animation.Initiate(this.animation, 500);
            startFly(frequency);
        }

        /// <summary>
        /// The startFly method takes as input a double value for frequency of the
        /// moving of the commander on the screen and initiates the timer for moving
        /// with that frequency value
        /// </summary>
        /// <param name="frequency">double value for the speed of the moving</param>
        private void startFly(double frequency)
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
        /// moving of the commander on the screen by making sure that the commander doesn't
        /// move pass the limits of the screen while checking for player collision. 
        /// The method handles as well the random dive sequence of the commander
        /// if the boolean isDive is set to true
        /// </summary>
        /// <param name="sender">object raising the updateMoveHorizontal event</param>
        /// <param name="e">The updateMoveHorizontal event raised</param>
        private void updateMoveHorizontal(object sender, EventArgs e)
        {
            double commanderX = this.GetPoint().X;

            //keep moving until the commander hits the bottom
            if (this.point.Y <= 550)
            {
                //--------------------MOVE DOWN SEQUENCE----------------
                if (this.moveDown == true)
                {
                    this.point.Y += this.moveDownFrequency;
                    Canvas.SetTop(this.GetImage(), this.point.Y);
                    moveDown = false;
                    playerCollision();

                    //if the dive boolean is set to true do the following
                    if (dive == true)
                    {
                        Random rand = new Random();
                        int randNum = rand.Next(20); //generate random number
                        if (randNum % 3 == 0) //if multiple of 3 then make commander dive
                        {
                            this.timerFly.Stop();
                            this.timerFly = new DispatcherTimer(DispatcherPriority.Render);
                            this.timerFly.Interval = TimeSpan.FromMilliseconds(120);
                            this.timerFly.Tick += new EventHandler(this.updateMoveDown);
                            this.timerFly.Start();
                        }
                    }
                }

                //-------------------MOVE LEFT/RIGHT SEQUENCE----------------
                if (moveCounter == 1)// 1 = move left
                {
                    if (commanderX >= minX) //if commander can still move left
                    {
                        this.point.X -= 10;
                        Canvas.SetLeft(this.GetImage(), this.point.X);
                    }
                    else //if commander cannot move to the left any further : 
                    { 
                        moveCounter = 2; // 2 = move right 
                        moveDown = true;
                    }
                }
                else //2 = move right
                {
                    if (commanderX <= maxX) //if commander can still move right :
                    {
                        this.point.X += 10;
                        Canvas.SetLeft(this.GetImage(), this.point.X);
                    }
                    else //if commander cannot move to the right any further
                    {
                        moveCounter = 1;
                        moveDown = true;
                    }
                }
                playerCollision(); //always check for player collision at every movement
            }
            else //if the commander reached the bottom
            {
                canvas.Children.Remove(this.image);
                returnToTheTop(); //return to the top and restart going down
            }
        }

        /// <summary>
        /// The updateMoveDown event handler method initiates the dive sequence of the 
        /// commander. The updateMoveDown method is called by the startFly method with a timer
        /// </summary>
        /// <param name="sender">object that raised the updateMoveDown event</param>
        /// <param name="e">The updateMoveDown event </param>
        private void updateMoveDown(object sender, EventArgs e)
        {
            //keep diving down until commander reaches the bottom
            if (this.point.Y <= 550)
            {
                this.point.Y += this.diveFrequency;
                Canvas.SetTop(this.GetImage(), this.point.Y);
                playerCollision();
            }
            else //if commander reached the bottom, return to the top and continue the normal movement
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
        /// of the commander's wings
        /// </summary>
        public void stopMove()
        {
            this.timerFly.Stop();
            this.animation.Stop();
        }

        /// <summary>
        /// The restartMove method starts the timer for the moving and the timer
        /// for the animation of the commanders' wings
        /// </summary>
        public void restartMove()
        {
            this.timerFly.Start();
            this.animation.Start();
        }

        /// <summary>
        /// The stopShoot method stops the timer for the shooting
        /// </summary>
        public void stopShoot()
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
        /// The overridden Shoot method initiates the timer for the shooting 
        /// of the commander with a specific frequency and calls the Shoot method event 
        /// handler 
        /// </summary>
        /// <param name="frequency"></param>
        public override void Shoot(double frequency)
        {
            this.timerShoot = new DispatcherTimer(DispatcherPriority.Render);
            this.timerShoot.Interval = TimeSpan.FromSeconds(frequency);
            this.timerShoot.Tick += new EventHandler(Shoot);
            this.timerShoot.Start();
        }

        /// <summary>
        /// Shoot method event handler sets the isShooting boolean to true
        /// and creates a Bullet instance and calls the shootDown method of the 
        /// Bullet class with the right position depending of the commander's
        /// position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Shoot(Object sender, EventArgs e)
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
        /// The changeImage method changes the image of the commander once
        /// it is shot once as an indication to the user and initiates a new animation
        /// for that new image.
        /// </summary>
        public void changeImage()
        {
            this.animation.Stop();
            BitmapImage[] commanderImages = { UtilityMethods.LoadImage("pics/commanderGreen0.png"),
                    UtilityMethods.LoadImage("pics/commanderGreen1.png") };
            this.animation = new Animation(this.image, commanderImages, true);
            Animation.Initiate(animation, 500);
        }

        /// <summary>
        /// The getIsShot method returns the boolean true or false if the 
        /// commander is shot once already
        /// </summary>
        /// <returns>boolean true or false</returns>
        public bool getIsShot()
        {
            return this.isShot;
        }

        /// <summary>
        /// The setIsShot method sets boolean value of isShot attribute
        /// </summary>
        /// <param name="isShot">boolean true or false</param>
        public void setIsShot(bool isShot)
        {
            this.isShot = isShot;
        }
    }
}
