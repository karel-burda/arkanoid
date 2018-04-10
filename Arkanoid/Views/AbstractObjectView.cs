using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using Arkanoid.Model;
using Arkanoid.Model.Misc;
using Arkanoid.Presenters;

namespace Arkanoid.Views
{

    /// <summary>
    /// Class represents the frontend of each "Abstract Object"
    /// The frontend consists of background color, rectangle itself, borders, etc.
    /// <seealso cref="AbstractObject"/>
    /// </summary>
    abstract partial class AbstractObjectView
    {
        // Rectangle object
        public Rectangle Rectangle { get; set; }
    }

    /// <summary>
    /// Methods
    /// </summary>
    abstract partial class AbstractObjectView
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public AbstractObjectView(double width, double height)
        {

            // Create new inner rectangle
            this.Rectangle = new Rectangle();

            if (this.Rectangle != null)
            {

                this.SetUpGUI();

                this.Rectangle.Width = width;
                this.Rectangle.Height = height;
            }
            else
            {
                throw new Exception("Cannot create inner rectangle");
            }
        }

        /// <summary>
        /// Method is overriden in each child and defining the frontend od each graphical object's View
        /// </summary>
        public abstract void SetUpGUI();

        /// <summary>
        /// Removes the frontend
        /// </summary>
        public void RemoveView()
        {
            Game.Instance.Canvas.Children.Remove(this.Rectangle);
        }

        /// <summary>
        /// Render the object to the Canvas
        /// </summary>
        public void Render(double x, double y)
        {

            // Starting position
            Canvas.SetLeft(this.Rectangle, x);
            Canvas.SetTop(this.Rectangle, y);

            // Draw the object (if not yet presented)
            if (!Game.Instance.Canvas.Children.Contains(this.Rectangle))
            {
                Game.Instance.Canvas.Children.Add(this.Rectangle);
            }

            // Force the GUI update (to be sure)
            this.Rectangle.InvalidateVisual();
            this.Rectangle.UpdateLayout();
            Game.Instance.Canvas.UpdateLayout();
            Game.Instance.Canvas.InvalidateVisual();
        }

        /// <summary>
        /// Overriden method
        /// </summary>
        /// <returns>Object description</returns>
        public override string ToString()
        {
            return (this.GetType().Name + ", Rectangle = " + this.Rectangle.ToString());
        }
    }
}
