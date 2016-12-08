using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GalagaX4
{
    /// <summary>
    /// The Enemies abstract class defines the general attributes and 
    /// behavior of an enemy object of the game galaga
    /// </summary>
    abstract class Enemies : GameObject
    {
        protected Animation animation;
        protected Player target;
        
        protected int moveCounter;
        protected double maxX;
        protected double minX;
        protected  bool moveDown;
        protected bool dive;

        protected bool isShooting; //if shooting already
        protected bool dead; //if destroyed already

        protected double flyFrequency;
        protected int diveFrequency;
        protected int moveDownFrequency;
        /// <summary>
        /// The no parameter Enemies sets the animation object to null.
        /// It is created just to give access to useful methods in the class.
        /// </summary>
        public Enemies() : base()
        {
            this.animation = null;
        }
        /// <summary>
        /// The Enemies constructor sets the position of the game element,
        /// the image and the game canvas from the values inputed into the constructor
        /// </summary>
        /// <param name="point">Position of the game elements of type Point</param>
        /// <param name="image">The image of the game elements </param>
        /// <param name="canvas">The game canvas in which the game elements is in</param>
        public Enemies(Point point, Image image, Canvas canvas)
            : base(point, image, canvas)
        {
            this.animation = null;
            //boundaries for moving:
            this.minX = this.GetPoint().X - 130;
            this.maxX = this.GetPoint().X + 130;
        }
        /// <summary>
        /// The Enemies overloaded constructor sets the position of the game element,
        /// the image, the animation object and the game canvas from the values inputed into the constructor
        /// </summary>
        /// <param name="point">Position of the game elements of type Point</param>
        /// <param name="image">The image of the game elements </param>
        /// <param name="canvas">The game canvas in which the game elements is in</param>
        /// <param name="animation">The animation Object to create the illusion of flying</param>
        public Enemies(Point point, Image image, Canvas canvas
            , Animation animation) : base(point, image, canvas)
        {
            if (image == null || canvas == null || animation == null)
            {
                throw new NullReferenceException();
            }

            this.animation = new Animation(animation.getImage()
                , animation.getBitmapImageArray(), animation.IsRepeated());

            //boundaries for moving:
            this.minX = this.GetPoint().X - 130;
            this.maxX = this.GetPoint().X + 130;
        }
        /// <summary>
        /// The setTarget method sets the target to the player specified.
        /// It will be used to define collisions and certain actions related
        /// with the enemies.
        /// </summary>
        /// <param name="player"></param>
        public void setTarget(Player player)
        {
            this.target = player;
        }
        /// <summary>
        /// The setMoveCounter sets the moveCounter to a value provided.
        /// It will be used to define the number of movements of the enemies.
        /// </summary>
        /// <param name="counter"></param>
        public void setMoveCounter(int counter)
        {
            this.moveCounter = counter;
        }
        /// <summary>
        /// The setDive method sets the state of the diving movement of the enemies
        /// to true or false.
        /// </summary>
        /// <param name="dive"></param>
        public void setDive(bool dive)
        {
            this.dive = dive;
        }
        /// <summary>
        /// The setMoveDownFrequency sets the moveDownFrequency to a value provided.
        /// It will be used to define the rate frequency of move downs of the enemies.
        /// </summary>
        /// <param name="freq">An int type value to define the frequency</param>
        public void setMoveDownFrequency(int freq)
        {
            this.moveDownFrequency = freq;
        }
        /// <summary>
        /// The setDiveFrequency sets the diveFrequency  to a value provided.
        /// It will be used to define the rate frequency of dive movements of the enemies.
        /// </summary>
        /// <param name="freq">An int type value to define the frequency</param>
        public void setDiveFrequency(int freq)
        {
            this.diveFrequency = freq;
        }
        /// <summary>
        /// The playerCollision method checks for collision between the player and the enemies.
        /// If the collision occurs the method invokes the die method.
        /// </summary>
        protected void playerCollision()
        {
            double currentX = Canvas.GetLeft(this.image);
            double currentY = Canvas.GetTop(this.image);
            double playerX = Canvas.GetLeft(this.target.GetImage());
            double playerY = Canvas.GetTop(this.target.GetImage());

            Rect current = new Rect(currentX, currentY, this.image.Width - 5, this.image.Height - 5);
            Rect player = new Rect(playerX, playerY, this.image.Width - 5, this.image.Height - 5);

            if (current.IntersectsWith(player))
            {
                target.Die();
                this.Die();
            }
        }
        /// <summary>
        /// The returnToTheTop method sets the coordinate Y of the image to 10
        /// and places the image at that position.
        /// </summary>
        protected void returnToTheTop()
        {
            this.point.Y = 10;
            Canvas.SetTop(this.image, this.point.Y);
            canvas.Children.Add(this.image);
        }
        /// <summary>
        /// Abstract Fly method to be implemented in the concrete classes.
        /// </summary>
        /// <param name="frequency">An int type value to define the frequency</param>
        public abstract void Fly(double frequency);
        /// <summary>
        /// /// Abstract Shoot method to be implemented in the concrete classes.
        /// </summary>
        /// <param name="frequency">An int type value to define the frequency</param>
        public abstract void Shoot(double frequency);

    }
}
