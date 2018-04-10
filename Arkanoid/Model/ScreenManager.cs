using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Arkanoid.Presenters;

namespace Arkanoid.Model
{
    /// <summary>
    /// Static class that provides certain method connected to screen and windows
    /// </summary>
    static partial class ScreenManager
    {
    }

    /// <summary>
    /// Methods
    /// </summary>
    static partial class ScreenManager
    {
        /// <summary>
        /// Returns Canvas width
        /// </summary>
        /// <returns>Width</returns>
        public static double GetCanvasWidth()
        {
            return Game.Instance.AppWindow.Canvas.ActualWidth;
        }

        /// <summary>
        /// Returns Canvas height
        /// </summary>
        /// <returns>Height</returns>
        public static double GetCanvasHeight()
        {
            return Game.Instance.AppWindow.Canvas.ActualHeight;
        }
    }
}
