using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using Arkanoid.Model.Misc;
using Arkanoid.Presenters;
using Arkanoid.Views;

namespace Arkanoid.Model
{

    /// <summary>
    /// Enumeration defining possible object's direction
    /// </summary>
    public enum Direction
    {
        None = 0,
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW
    };

    /// <summary>
    /// Represents the handler that is responsible for the physics of the game
    /// </summary>
    partial class PhysicsManager
    {
    }

    /// <summary>
    /// Methods
    /// </summary>
    partial class PhysicsManager
    {

        /// <summary>
        /// Method simply moves certain object throughout the canvas
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="dX">Delta X</param>
        /// <param name="dY">Delta Y</param>
        public void MoveObject(AbstractObject obj, double dX, double dY)
        {
            if (obj == null)
            {
                return;
            }

            // Check bounds
            BoundaryLocation boundaryMet = this.CheckBoundsForObject(obj, ref dX, ref dY);

            // Change its inner values
            obj.X += dX;
            obj.Y += dY;

            // Move with object on the Canvas
            obj.View.Rectangle.SetValue(Canvas.LeftProperty, obj.X);
            obj.View.Rectangle.SetValue(Canvas.TopProperty, obj.Y);

            // If the object reached the boundary, do the callback to the respective object
            if (boundaryMet != BoundaryLocation.None)
            {
                obj.BoundaryReachedCallback(boundaryMet);
            }
            else
            {
                obj.BoundaryNotReachedCallback();
            }
        }


        /// <summary>
        /// Method checks whether the object is not going out of bounds
        /// If Yes, stick it to these bounds
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dX"></param>
        /// <param name="dY"></param>
        /// <returns>Whether object reached some boundary (e.g. level bounds) - returns type of the boundary</returns>
        private BoundaryLocation CheckBoundsForObject(AbstractObject obj, ref double dX, ref double dY)
        {

            if (obj == null)
            {
                return BoundaryLocation.None;
            }

            double resolvedX = obj.X + dX;
            double resolvedY = obj.Y + dY;

            // This will be filled
            BoundaryLocation boundaryLocation = BoundaryLocation.None;

            // Left bounds
            if (resolvedX < 0)
            {
                dX = -(obj.X);
                boundaryLocation = BoundaryLocation.Left;
            }
            // Right bounds
            else if ((resolvedX + obj.Width) > ScreenManager.GetCanvasWidth())
            {
                dX = ScreenManager.GetCanvasWidth() - (obj.X + obj.Width);
                boundaryLocation = BoundaryLocation.Right;
            }
            // Upper bounds
            else if (resolvedY < 0)
            {
                dY -= (obj.Y);
                boundaryLocation = BoundaryLocation.Top;
            }
            // Lower bounds
            else if ((resolvedY + obj.Height) > ScreenManager.GetCanvasHeight())
            {
                dY = ScreenManager.GetCanvasHeight() - (resolvedY + obj.Height);
                boundaryLocation = BoundaryLocation.Bottom;
            }
            // Everything is ok...
            else
            {
                boundaryLocation = BoundaryLocation.None;
            }

            return boundaryLocation;
        }

        /// <summary>
        /// Simply detect whether two object collide
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns>True = collision</returns>
        public static bool DetectColisionOfObjects(AbstractObject obj1, AbstractObject obj2)
        {

            if (obj1 == null || obj2 == null || (obj1 == obj2))
            {
                return false;
            }

            // Do simple 2D rectangle intersection
            return 
                (
                (obj1.X <= (obj2.X + obj2.Width)) &&
                (obj2.X <= (obj1.X + obj1.Width)) &&
                (obj1.Y <= (obj2.Y + obj2.Height)) &&
                (obj2.Y <= (obj1.Y + obj1.Height))
                );
        }


        /// <summary>
        /// Method encapsulates collision handling
        /// </summary>
        public void HandleCollisions()
        {
            this.HandleBallCollisionWithBat();
            this.HandleBallCollisionWithObjects();
        }

        /// <summary>
        /// The method handles a situation when a collision of the Ball and the Bat can happen
        /// </summary>
        protected void HandleBallCollisionWithBat()
        {

            // Get references 
            Ball ball = Game.Instance.Level.Ball;
            Bat bat = Game.Instance.Level.Bat;

            // If It's sticked on the bat, do not count it as a collision :)
            if (ball.IsStickedOnTheBat)
            {
                return;
            }

            // Is there any collision?
            bool collision = PhysicsManager.DetectColisionOfObjects(ball, bat);
            
            if (collision)
            {

                Direction ballOriginalDirection = ball.CurrentDirection;
                // The only possible hit is from the top
                ball.CurrentDirection = PhysicsManager.GetOppositeDirectionTo(ball.CurrentDirection, BoundaryLocation.Top);

                // Ball came from SE or SW and is already below the top bound of the Ball
                if (ballOriginalDirection == Direction.SE || ballOriginalDirection == Direction.SW)
                {

                    // Hit the left side of the bat
                    if (ballOriginalDirection == Direction.SE && ((ball.X + ball.Width) - ball.Speed) <= Game.Instance.Level.Bat.X)
                    {
                        ball.CurrentDirection = Direction.SW;
                    }
                    else if (ballOriginalDirection == Direction.SW && ball.X >= ((Game.Instance.Level.Bat.X + Game.Instance.Level.Bat.Width) - ball.Speed))
                    {
                        ball.CurrentDirection = Direction.SE;
                    }
                }

                // Resolve the situation when Ball came directly from N direction (for instance, on the beginning of the game)
                if (ballOriginalDirection == Direction.S)
                {
                    ball.CurrentDirection = ball.GetDirectionWhenCommingToBatFromNorth();
                }
            }
        }

        /// <summary>
        /// Method handles possible collision of Ball with other objects
        /// </summary>
        protected void HandleBallCollisionWithObjects()
        {

            Ball ball = Game.Instance.Level.Ball;

            // We go through all objects
            foreach (AbstractObject obj in Game.Instance.Level.Objects)
            {
                // Skipped if destroyed
                if (obj.Destroyed)
                {
                    continue;
                }

                // Is there any collision?
                if (PhysicsManager.DetectColisionOfObjects(obj, ball))
                {

                    // Bound location might be changed later
                    BoundaryLocation location = BoundaryLocation.Bottom;

                    // Significant points for the Ball
                    double ballTopLeftX = ball.X;
                    double ballTopLeftY = ball.Y;
                    double ballBottomLeftX = ballTopLeftX;
                    double ballBottomLeftY = ballTopLeftY + ball.Height;
                    double ballRightX = ball.X + ball.Width;
                    double ballBottomRightX = ball.X + ball.Width;
                    double ballBottomRightY = ballBottomLeftY;

                    // Significant points for other object
                    double objRightX = obj.X + obj.Width;
                    double objDownY = obj.Y + obj.Height;
                    double objTopY = obj.Y;
                    double objLeftX = obj.X;
                    double objLeftY = obj.Y;

                    // Ball is coming from right => look at the right bounds
                    if (ball.CurrentDirection == Direction.W || ball.CurrentDirection == Direction.NW || ball.CurrentDirection == Direction.SW)
                    {
                        if (ball.CurrentDirection == Direction.W)
                        {
                            location = BoundaryLocation.Right;
                        }
                        else if (ball.CurrentDirection == Direction.NW)
                        {
                            if (Math.Abs(objRightX - ballTopLeftX) < Math.Abs(objDownY - ballTopLeftY))
                            {
                                location = BoundaryLocation.Right;
                            }
                        }
                        else if (ball.CurrentDirection == Direction.SW)
                        {
                            if (Math.Abs(objRightX - ballBottomLeftX) < Math.Abs(ballBottomLeftY - objTopY))
                            {
                                location = BoundaryLocation.Right;
                            }
                        }
                    }

                    // Ball is coming from the left
                    else if (ball.CurrentDirection == Direction.E || ball.CurrentDirection == Direction.NE || ball.CurrentDirection == Direction.SE)
                    {
                        if (ball.CurrentDirection == Direction.E)
                        {
                            location = BoundaryLocation.Left;
                        }
                        else if (ball.CurrentDirection == Direction.NE)
                        {
                            if (Math.Abs(ballRightX - objLeftX) < Math.Abs(objDownY - ballTopLeftY))
                            {
                                location = BoundaryLocation.Left;
                            }
                        }
                        else if (ball.CurrentDirection == Direction.SE)
                        {
                            if (Math.Abs(ballBottomRightX - objLeftX) < Math.Abs(ballBottomRightY - objLeftY))
                            {
                                location = BoundaryLocation.Left;
                            }
                        }
                    }

                    // Resolve final ball direction
                    ball.CurrentDirection = PhysicsManager.GetOppositeDirectionTo(ball.CurrentDirection, location);

                    // If there is a colision, destroy the object
                    obj.Destroy();

                    // Notify Level, that and object has been hit
                    Game.Instance.Level.ObjectWasHitCallback();
                }
            }
        }

        /// <summary>
        /// Returns opposite direction to the original direction and obstactle location
        /// </summary>
        /// <param name="originalDirection"></param>
        /// <param name="otherObjectLocation"></param>
        /// <returns>Opposite direction respecting trajectory</returns>
        public static Direction GetOppositeDirectionTo(Direction originalDirection, BoundaryLocation otherObjectLocation)
        {

            if (otherObjectLocation == BoundaryLocation.None)
            {
                return Direction.None;
            }

            else if (otherObjectLocation == BoundaryLocation.Top || otherObjectLocation == BoundaryLocation.Bottom)
            {
                return GetOppositeDirectionToTopOrBottom(originalDirection);
            }
            else if (otherObjectLocation == BoundaryLocation.Left || otherObjectLocation == BoundaryLocation.Right)
            {
                return GetOppositeDirectionToLeftOrRight(originalDirection);
            }

            return Direction.None;
        }

        /// <summary>
        /// Helper method when obstacle is on left or right
        /// </summary>
        /// <param name="originalDirection"></param>
        /// <returns></returns>
        public static Direction GetOppositeDirectionToLeftOrRight(Direction originalDirection)
        {
            if (originalDirection == Direction.N)
            {
                return Direction.S;
            }
            else if (originalDirection == Direction.NE)
            {
                return Direction.NW;
            }
            else if (originalDirection == Direction.E)
            {
                return Direction.W;
            }
            else if (originalDirection == Direction.SE)
            {
                return Direction.SW;
            }
            else if (originalDirection == Direction.S)
            {
                return Direction.N;
            }
            else if (originalDirection == Direction.SW)
            {
                return Direction.SE;
            }
            else if (originalDirection == Direction.W)
            {
                return Direction.E;
            }
            else if (originalDirection == Direction.NW)
            {
                return Direction.NE;
            }

            return Direction.None;
        }

        /// <summary>
        /// Helper method when obstacle is located on top or bottom
        /// </summary>
        /// <param name="originalDirection"></param>
        /// <returns></returns>
        public static Direction GetOppositeDirectionToTopOrBottom(Direction originalDirection)
        {
            if (originalDirection == Direction.N)
            {
                return Direction.S;
            }
            else if (originalDirection == Direction.NE)
            {
                return Direction.SE;
            }
            else if (originalDirection == Direction.E)
            {
                return Direction.W;
            }
            else if (originalDirection == Direction.SE)
            {
                return Direction.NE;
            }
            else if (originalDirection == Direction.S)
            {
                return Direction.N;
            }
            else if (originalDirection == Direction.SW)
            {
                return Direction.NW;
            }
            else if (originalDirection == Direction.W)
            {
                return Direction.E;
            }
            else if (originalDirection == Direction.NW)
            {
                return Direction.SW;
            }

            return Direction.None;
        }



        /// <summary>
        /// Distance Meter, returns minimum straight distance between two objects (their rectangles)
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <remarks>
        /// Might be used to deduce, whether is neccessary to compute collision of certain objects
        /// (if they are too far from each other, there's no need)
        /// </remarks>
        /// <returns>Minimum straight distance between objects (if error, return INF)</returns>
        [Obsolete("Usage is deprecated, It´s faster to compute the collision itself :)")]
        public static double GetStraightDistanceBetweenObjects(AbstractObject obj1, AbstractObject obj2)
        {

            double minDistance = (+Double.MaxValue);
            double temp = minDistance;

            // Top Left
            temp = PhysicsManager.GetStraightDistanceBetweenPointAndObject(obj1.X, obj1.Y, obj2);
            if (temp < minDistance) minDistance = temp;
            // Top Right
            temp = PhysicsManager.GetStraightDistanceBetweenPointAndObject(obj1.X + obj1.Width, obj1.Y, obj2);
            if (temp < minDistance) minDistance = temp;
            // Bottom left
            temp = PhysicsManager.GetStraightDistanceBetweenPointAndObject(obj1.X, obj1.Y + obj1.Height, obj2);
            if (temp < minDistance) minDistance = temp;
            // Bottom Right
            temp = PhysicsManager.GetStraightDistanceBetweenPointAndObject(obj1.X + obj1.Width, obj1.Y + obj1.Height, obj2);
            if (temp < minDistance) minDistance = temp;

            return minDistance;
        }

        /// <summary>
        /// Returns minimum straight distance between point (for instance, one edge of a rectangle) and object
        /// Comptutes the distance between one point and other 4 points of ther object's rectangle
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <remarks>Currently have no usage, becuase "GetStraightDistanceBetweenObjects" is obsolete</remarks>
        protected static double GetStraightDistanceBetweenPointAndObject(double x, double y, AbstractObject obj)
        {

            double minDistance = (+Double.MaxValue);
            double temp = minDistance;

            temp = PhysicsManager.GetStraightDistanceBetweenPoints(x, obj.X, y, obj.Y);
            if (temp < minDistance) minDistance = temp;
            temp = PhysicsManager.GetStraightDistanceBetweenPoints(x, obj.X + obj.Width, y, obj.Y);
            if (temp < minDistance) minDistance = temp;
            temp = PhysicsManager.GetStraightDistanceBetweenPoints(x, obj.X, y, obj.Y + obj.Height);
            if (temp < minDistance) minDistance = temp;
            temp = PhysicsManager.GetStraightDistanceBetweenPoints(x, obj.X + obj.Width, y, obj.Y + obj.Height);
            if (temp < minDistance) minDistance = temp;

            return minDistance;
        }

        /// <summary>
        /// Straight distance between two points using Pythagoras theorem :)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        /// <remarks>Currently have no usage, becuase "GetStraightDistanceBetweenObjects" is obsolete</remarks>
        protected static double GetStraightDistanceBetweenPoints(double x1, double x2, double y1, double y2)
        {

            double dX = Math.Abs(x1 - x2);
            double dY = Math.Abs(y1 - y2);

            return Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));
        }
    }
}
