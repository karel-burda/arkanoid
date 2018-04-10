using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;

using Arkanoid.Model;
using Arkanoid.Model.Misc;
using Arkanoid.Presenters;

namespace Arkanoid.Model
{

    /// <summary>
    /// Enumeration defining level bounds - their locations
    /// </summary>
    public enum BoundaryLocation
    {
        None = 0,
        Top,
        Right,
        Bottom,
        Left
    };


    /// <summary>
    /// This is an abstraction of the game level
    /// Each child has to implement its own level configuration (brick locations, etc.)
    /// </summary>
    abstract partial class AbstractLevel
    {

        // List of objects that each level contains
        public List<AbstractObject> Objects { get; protected set; }

        // Reference to the bat
        public Bat Bat { get; protected set; }

        // Reference to the ball
        public Ball Ball { get; protected set; } 

        // Identification of the level
        public String LevelID { get; protected set; }

        // Inner timer used for updating the Level visuals and computing physics
        protected Timer TimerForUpdate { get; set; }
    }

    /// <summary>
    /// Methods
    /// </summary>
    abstract partial class AbstractLevel
    {

        // Construction
        public AbstractLevel()
        {
        }

        /// <summary>
        /// Init takes care of setting the Level up and position the bat itself
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {

            // Create new list of objects
            this.Objects = new List<AbstractObject>();

            // Create bat and set up the level
            if (this.SetUp() && this.CreateBat() && this.CreateBall())
            {
                // Start a timer
                this.SetLevelTimer();
                this.StartLevelTimer();

                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Creates a bat and position it
        /// </summary>
        /// <returns>Success/failure</returns>
        public bool CreateBat()
        {

            this.Bat = new Bat(0, 0, 100, 20);

            if (this.Bat == null)
            {
                return false;
            }

            // Center the bat
            this.Bat.X = ScreenManager.GetCanvasWidth()/2 - this.Bat.Width/2;

            // Minus some margin
            this.Bat.Y = ScreenManager.GetCanvasHeight() - this.Bat.Height - 3.0;         

            return true;
        }

        /// <summary>
        /// Creates a ball and positions it onto the bat
        /// </summary>
        /// <returns>Success/failure</returns>
        public bool CreateBall()
        {

            this.Ball = new Ball(0, 0, 20, 20);

            if (this.Ball == null)
            {
                return false;
            }

            // Place the ball
            this.CenterTheBallOnTheBat();

            return true;
        }

        /// <summary>
        /// Callback called whenever ball falls down :)
        /// </summary>
        public void BallReachedBottomBoundsCallback()
        {
            
            // Restart the Ball position
            Game.Instance.Level.CenterTheBallOnTheBat();
            Game.Instance.Level.Ball.IsStickedOnTheBat = true;
        }

        /// <summary>
        /// Helper method that centers the ball on the Bat 
        /// </summary>
        public void CenterTheBallOnTheBat()
        {
            Game.Instance.Level.Ball.X = (Game.Instance.Level.Bat.X + Game.Instance.Level.Bat.Width / 2.0 - Game.Instance.Level.Ball.Width / 2.0);
            Game.Instance.Level.Ball.Y = (Game.Instance.Level.Bat.Y - Game.Instance.Level.Ball.Height);

            // Render it
            Game.Instance.Level.Ball.View.Render(Game.Instance.Level.Ball.X, Game.Instance.Level.Ball.Y);
        }


        /// <summary>
        /// Freezes the visuals
        /// </summary>
        public void Freeze()
        {
            this.Ball.Moveable = false;
            this.Bat.Moveable = false;
            this.StopLevelTimer();
        }

        /// <summary>
        /// Resume all visuals
        /// </summary>
        public void UnFreeze()
        {
            this.Ball.Moveable = true;
            this.Bat.Moveable = true;
            this.StartLevelTimer();
        }

        /// <summary>
        /// Renders all the contents of Level to the Canvas
        /// </summary>
        public void Render()
        {

            // Render objects
            if (this.Objects != null)
            {
                foreach (AbstractObject obj in this.Objects)
                {
                    obj.View.Render(obj.X, obj.Y);
                }
            }

            // Render the Bat
            if (this.Bat != null)
            {
                this.Bat.View.Render(this.Bat.X, this.Bat.Y);
            }

            // Render the Ball
            if (this.Ball != null)
            {
                this.Ball.View.Render(this.Ball.X, this.Ball.Y);
            }
        }

        /// <summary>
        /// Start a timer for moving of the ball (It should run on the GUI thread)
        /// Should be stopped automatically
        /// </summary>
        public void StartLevelTimer()
        {
            // Create new or just resume actual one 
            if (this.TimerForUpdate != null)
            {
                this.TimerForUpdate.Start();
            }
        }

        /// <summary>
        /// Sets the timer for Ball moving, or create a brand new
        /// </summary>
        public void SetLevelTimer()
        {

            if (this.TimerForUpdate != null)
            {
                this.TimerForUpdate.Stop();
                this.TimerForUpdate.Enabled = false;
                this.TimerForUpdate = null;
            }

            this.TimerForUpdate = new Timer();
            this.TimerForUpdate.Tick += new EventHandler(UpdateTick);
            this.TimerForUpdate.Interval = AbstractLevel.GetTimeIntervalForTicking();
            this.TimerForUpdate.Enabled = true;
        }

        /// <summary>
        /// Simply stops a timer for ball moving
        /// </summary>
        public void StopLevelTimer()
        {

            if (this.TimerForUpdate != null)
            {
                this.TimerForUpdate.Stop();
            }
        }

        /// <summary>
        /// The callback is called by the timer in each interval
        /// We update the Level's visual and other things
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void UpdateTick(object source, EventArgs e)
        {

            // Update the ball
            this.Ball.Move(this.Ball.CurrentDirection);

            // Now check for collisions
            Game.Instance.PhysicsHandler.HandleCollisions();
        }

        /// <summary>
        /// Helper static method for getting the time inteval (frequency)
        /// </summary>
        /// <returns>How often should timer call the callback (for updating the level) in seconds</returns>
        /// <seealso cref="StartMoving"/>
        public static int GetTimeIntervalForTicking()
        {
            // Just insert some literal constant here
            return (20);
        }

        /// <summary>
        /// Is called when some level object is hit
        /// </summary>
        public virtual void ObjectWasHitCallback()
        {
            // Increment score
            Game.Instance.Settings.Score++;
        }

        /// <summary>
        /// This methods implements a set up of bricks, and other related work to each level of the game
        /// <returns>Success/failure</returns>
        /// </summary>
        public abstract bool SetUp();

        /// <summary>
        /// Returns default speed of the ball for each Level
        /// </summary>
        /// <returns></returns>
        public abstract double GetDefaultBallSpeed();


        /// <summary>
        /// Returns default speed of the bat for each Level
        /// </summary>
        /// <returns></returns>
        public abstract double GetDefaultBatSpeed();
    }
}
