using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Arkanoid.Model.Misc;
using Arkanoid.Views;

namespace Arkanoid.Model
{
    /// <summary>
    /// A sample brick, destroyable, non-movable object
    /// </summary>
    partial class Brick : AbstractObject
    {
    }

    /// <summary>
    /// Methods
    /// </summary>
    partial class Brick : AbstractObject
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Brick(double x, double y, double width, double height)
        :base(x, y, width, height)
        {
            this.Moveable = false;
            this.View = new BrickView(this.Width, this.Height);
        }

        // Overriden Move
        public override void Move(Direction direction)
        {
            return;
        }
    }
}
