using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;

using Arkanoid.Model;
using Arkanoid.Model.Misc;
using Arkanoid.Presenters;
using Arkanoid.Views;

namespace Arkanoid.Model
{
    /// <summary>
    /// A ball object
    /// </summary>
    partial class Ball : AbstractObject
    {

        // Is ball still on the Bat
        public bool IsStickedOnTheBat { get; set; }

        // Denotes current direction of the Ball
        public Direction CurrentDirection { get; set; }
    }

    /// <summary>
    /// Methods
    /// </summary>
    partial class Ball : AbstractObject
    {
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Ball(double x, double y, double width, double height)
        :base(x, y, width, height)
        {
            this.Moveable = true;
            this.Speed = Game.Instance.Level.GetDefaultBallSpeed();
            this.IsStickedOnTheBat = true;
            this.View = new BallView(this.Width, this.Height);
        }

        /// <summary>
        /// Elevates ball from the Bat
        /// </summary>
        public void ElevateBallFromTheBat()
        {

            if (!this.IsStickedOnTheBat)
            {
                return;
            }

            // Is game paused?
            if (!Game.Instance.Settings.IsRunning)
            {
                return;
            }

            // Start to the North :)
            this.StartMoving(Direction.N);
        }

        /// <summary>
        /// Starts the moving itself of the Ball
        /// </summary>
        /// <param name="startingDirection">pointed direction</param>
        public void StartMoving(Direction startingDirection)
        {

            this.IsStickedOnTheBat = false;
            this.CurrentDirection = startingDirection;
        }

        // Overriden Move
        public override void Move(Direction direction)
        {

            if (!this.Moveable)
            {
                return;
            }

            // If the Ball is on the bat, we need to edit the speed factor
            if (this.IsStickedOnTheBat)
            {
                this.Speed = Game.Instance.Level.Bat.Speed;

                // If the bat is already on some borders, do not move with the ball
                if (Game.Instance.Level.Bat.IsStickedToBounds)
                {
                    return;
                }

                // We cannot move to other directions when ball is sticked to the bat
                if (direction != Direction.N && direction != Direction.E && direction != Direction.W)
                {
                    return;
                }
            }
            // Set default Speed factor back 
            else
            {
                this.Speed = Game.Instance.Level.GetDefaultBallSpeed();
            }

            // Reflect direction
            if (direction == Direction.None)
            {
                return;
            }
            else if (direction == Direction.N)
            {
                this.MoveBy(0.0, -GameSettings.GetMovingUnit());
            }
            else if (direction == Direction.NE)
            {
                this.MoveBy(+GameSettings.GetMovingUnit(), -GameSettings.GetMovingUnit());
            }
            else if (direction == Direction.E)
            {
                this.MoveBy(+GameSettings.GetMovingUnit(), 0.0);
            }
            else if (direction == Direction.SE)
            {
                this.MoveBy(+GameSettings.GetMovingUnit(), +GameSettings.GetMovingUnit());
            }
            else if (direction == Direction.S)
            {
                this.MoveBy(0.0, +GameSettings.GetMovingUnit());
            }
            else if (direction == Direction.SW)
            {
                this.MoveBy(-GameSettings.GetMovingUnit(), +GameSettings.GetMovingUnit());
            }
            else if (direction == Direction.W)
            {
                this.MoveBy(-GameSettings.GetMovingUnit(), 0.0);
            }
            else if (direction == Direction.NW)
            {
                this.MoveBy(-GameSettings.GetMovingUnit(), -GameSettings.GetMovingUnit());
            }
        }

        // Overriden callback
        public override void BoundaryReachedCallback(BoundaryLocation location)
        {
            base.BoundaryReachedCallback(location);

            // If the ball reached lower bounds, pass the message to the Level
            if (location == BoundaryLocation.Bottom)
            {
                Game.Instance.Level.BallReachedBottomBoundsCallback();
                return;
            }

            // Else the ball reached other bounds...
            this.CurrentDirection = PhysicsManager.GetOppositeDirectionTo(this.CurrentDirection, location);
            this.Move(this.CurrentDirection);
        }


        /// <summary>
        /// Resolves the situation when Ball is coming directly from N direction (for instance, in the beginning of the game)
        /// </summary>
        /// <remarks>
        /// Like this:
        /// 
        ///      °
        ///      |
        ///      |
        ///      |
        ///      |
        ///  =========
        /// </remarks>
        /// <returns>Direction</returns>
        public Direction GetDirectionWhenCommingToBatFromNorth()
        {
            // Declare some vars at first
            const uint parts = 5;
            double partWidth = Game.Instance.Level.Bat.Width / (double)parts;
            double centerBallX = (this.X + this.Width / 2);
            double leftBoundX = Game.Instance.Level.Bat.X + partWidth * 2.0;
            double rightBoundX = Game.Instance.Level.Bat.X + partWidth * 3.0;

            // Hit the part more to the left
            if (centerBallX < leftBoundX)
            {
                return Direction.NW;
            }
            // More to the Right
            else if (centerBallX > rightBoundX)
            {
                return Direction.NE;
            }

            // Somewhere near the center
            return Direction.N;
        }
    }
}
