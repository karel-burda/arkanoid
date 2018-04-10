using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows; 
using System.Windows.Media;
using System.Windows.Shapes;

using Arkanoid.Model.Misc;

namespace Arkanoid.Views
{
    partial class BatView : AbstractObjectView
    {
    }

    partial class BatView : AbstractObjectView
    {
        
        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public BatView(double width, double height)
        :base (width, height)
        {

        }

        // Overriden methods
        public override void SetUpGUI()
        {
            const String shapeName = "rectBat";

            // A basic check
            if (Application.Current.Resources[shapeName] == null || Application.Current.Resources[shapeName].GetType() != typeof(Rectangle))
            {
                throw new Exception("BatView: Not a rectangle");
            }
            else
            {
                this.Rectangle = (Rectangle)Application.Current.Resources[shapeName];
            }
        }
    }
}
