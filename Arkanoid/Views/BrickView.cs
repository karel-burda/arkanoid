using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

using Arkanoid.Model.Misc;
using Arkanoid.Views;

namespace Arkanoid.Views
{
    partial class BrickView : AbstractObjectView
    {
    }

    partial class BrickView : AbstractObjectView
    {

        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public BrickView(double width, double height)
        :base (width, height)
        {

        }

        // Overriden methods
        public override void SetUpGUI()
        {

            const String shapeName = "rectBrickBasic";

            // A basic check
            if (Application.Current.Resources[shapeName] == null || Application.Current.Resources[shapeName].GetType() != typeof(Rectangle))
            {
                throw new Exception("BrickView: Not a rectangle");
            }
            else
            {
                Rectangle srcRect = (Rectangle)Application.Current.Resources[shapeName];
                
                // We have more bricks, copy the element
                this.Rectangle = HelperMethods.CopyRectangleDeep(srcRect);
            }
        }
    }
}
