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
    /// The class represents an abstract graphical object
    /// Each object has its own frontend definition
    /// <see cref="AbstractObjectView"/>
    /// </summary>
    abstract partial class AbstractObject
    {

        // Positions
        public double X { get; set; }
        public double Y { get; set; }

        // Dimensions
        public double Width { get; set; }
        public double Height { get; set; }

        // Is object moveable or static?
        public bool Moveable { get; set; }

        // Is object destroyed?
        public bool Destroyed { get; set; }

        // Is the object currently at the some border (bounds)
        public bool IsStickedToBounds { get; set; }

        // Speed = by how many pixels object is going to move in one time unit :)
        public double Speed { get; protected set; }

        // Object frontend
        public AbstractObjectView View { get; set; }
    }

    /// <summary>
    /// Methods
    /// </summary>
    abstract partial class AbstractObject
    {
        
        /// <summary>
        /// Default constructor
        /// </summary>
        protected AbstractObject()
        {
            // Set up default values
            this.X = 0.0;
            this.Y = 0.0;
            this.Width = 0;
            this.Height = 0;

            this.Moveable = false;
            this.Destroyed = false;
            this.IsStickedToBounds = false;
            this.Speed = 0.0;
        }

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="width">Width</param>
        /// <param name="height">height</param>
        protected AbstractObject(double x, double y, double width, double height)
        :this()
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// This inner method calls silently the Physics Manager
        /// </summary>
        /// <param name="dX">Delta X</param>
        /// <param name="dY">Delta Y</param>
        /// <seealso cref="Move"/>
        protected virtual void MoveBy(double dX, double dY)
        {

            if (!this.Moveable)
            {
                return;
            }

            // Move object X times according to its resolved speed factor
            // Objects are deliberately moving X times*(1 px) for instance, not like by +5px at once
            uint multiplier = GameSettings.GetTotalSpeedMultiplier(this, Game.Instance.Settings.Speed);

            for (uint i = 0; i < multiplier; i++)
            {
                Game.Instance.PhysicsHandler.MoveObject(this, dX, dY);
            }
        }

        /// <summary>
        /// Custom event that is called whenever an object reaches some bounds
        /// (e.g. ball has reach the top side, etc.)
        /// Each Object implementation can react to this event differently
        /// </summary>
        public virtual void BoundaryReachedCallback(BoundaryLocation location)
        {
            this.IsStickedToBounds = true;
        }


        /// <summary>
        /// Custom event that is called whenever an object NOT reach some bound
        /// (e.g. ball has not reached the top side, etc.)
        /// Each Object implementation can react to this event differently
        /// </summary>
        public virtual void BoundaryNotReachedCallback()
        {
            this.IsStickedToBounds = false;
        }

        /// <summary>
        /// Destroys the object
        /// Removes its View
        /// </summary>
        public virtual void Destroy()
        {
            // First, look at the settings
            if (Game.Instance.Settings.Mode == GameMode.ModeDestroyable)
            {
                // Remove view
                this.View.RemoveView();

                this.Destroyed = true;
            }
        }

        /// <summary>
        /// Simply moves with self to some direction - public method
        /// Each child has to provide its own implementation (for instance, Bat will not move up or down, etc.)
        /// </summary>
        /// <param name="direction">Direction</param>
        /// <seealso cref="Direction"/>
        public abstract void Move(Direction direction);

        /// <summary>
        /// Helper method
        /// </summary>
        /// <param name="other">Other object</param>
        /// <returns>True = Inner values are the same</returns>
        public bool AreInnerFieldsEqual(AbstractObject other)
        {
            return ((this.X == other.X) && (this.Y == other.Y) && (this.Width == other.Width) && (this.Height == other.Height));
        }

        // Overriden methods
        public override string ToString()
        {
            return (this.GetType().Name + ", x = " + this.X + ", y = " + this.Y + ", w = " + this.Width + ", h = " + this.Height);
        }

        // Inspired by: http://msdn.microsoft.com/en-us/library/ms173147%28v=vs.80%29.aspx
        public override bool Equals(System.Object obj)
        {

            // If parameter is null, return false
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast, return false
            AbstractObject other = obj as AbstractObject;

            if ((System.Object)other == null)
            {
                return false;
            }

            // Inner fields match?
            return (this.AreInnerFieldsEqual(other));
        }

        // Inspired by: http://msdn.microsoft.com/en-us/library/ms173147%28v=vs.80%29.aspx
        public override int GetHashCode()
        {
            return (Convert.ToInt32(this.X) ^ Convert.ToInt32(this.Y) ^ Convert.ToInt32(this.Width) ^ Convert.ToInt32(this.Height));
        }

        // Inspired by: http://msdn.microsoft.com/en-us/library/ms173147%28v=vs.80%29.aspx
        public static bool operator ==(AbstractObject a, AbstractObject b)
        {
            // If both are null, or both are same instance, return true
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match
            return (a.AreInnerFieldsEqual(b));
        }

        // Inspired by: http://msdn.microsoft.com/en-us/library/ms173147%28v=vs.80%29.aspx
        public static bool operator !=(AbstractObject a, AbstractObject b)
        {
            return !(a == b);
        }
    }
}
