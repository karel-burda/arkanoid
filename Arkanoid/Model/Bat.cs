using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Arkanoid.Model.Misc;
using Arkanoid.Presenters;
using Arkanoid.Views; 

namespace Arkanoid.Model
{
    /// <summary>
    /// A bat represents the thing that is used to move with the ball :)
    /// </summary>
    partial class Bat : AbstractObject
    {
    }

    /// <summary>
    /// Methods
    /// </summary>
    partial class Bat : AbstractObject
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Bat(double x, double y, double width, double height)
        :base(x, y, width, height)
        {
            this.Moveable = true;
            this.Speed = Game.Instance.Level.GetDefaultBatSpeed();
            this.View = new BatView(this.Width, this.Height);
        }

        // Overriden Move
        public override void Move(Direction direction)
        {

            // Reflect direction of the Bat (can move just left or right)
            if (direction == Direction.E)
            {
                base.MoveBy(+GameSettings.GetMovingUnit(), 0.0);
            }
            else if (direction == Direction.W)
            {
                base.MoveBy(-GameSettings.GetMovingUnit(), 0.0);
            }

            // Move also the Ball?
            if (Game.Instance.Level.Ball.IsStickedOnTheBat)
            {
                Game.Instance.Level.Ball.Move(direction);
            }
        }


        // Overriden callback
        public override void BoundaryReachedCallback(BoundaryLocation location)
        {

            if (this.IsStickedToBounds)
            {
                return;
            }

            // If bat still contains the Ball...
            if (Game.Instance.Level.Ball.IsStickedOnTheBat)
            {
                // Might be neccessary to center the Ball again
                Game.Instance.Level.CenterTheBallOnTheBat();
                // And render it
                Game.Instance.Level.Ball.View.Render(Game.Instance.Level.Ball.X, Game.Instance.Level.Ball.Y);
            }

            base.BoundaryReachedCallback(location);
        }
    }
}
