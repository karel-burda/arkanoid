using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Xml;

namespace Arkanoid.Model.Misc
{

    /// <summary>
    /// Introduces certain helper and auxiliary methods
    /// </summary>
    static partial class HelperMethods
    {
    }

    static partial class HelperMethods
    {

        /// <summary>
        /// Performs a deep copy of Rectangle object
        /// </summary>
        /// <remarks>Inspired by ShrinanD Vyas: http://shrinandvyas.blogspot.cz/2011/08/wpf-how-to-deep-copy-wpf-object-eg.html </remarks>
        /// <param name="origRect">Rectangle to copy</param>
        /// <returns>Deep copy of rectangle</returns>
        public static Rectangle CopyRectangleDeep (Rectangle origRect)
        {
            // Basic check
            if (origRect == null || origRect.GetType() != typeof(Rectangle))
            {
                return null;
            }

            // Serialize and DeSerialize
            var xaml = XamlWriter.Save(origRect);
            var xamlString = new StringReader(xaml);
            var xmlTextReader = new XmlTextReader(xamlString);
            var rectangleDeep = (Rectangle)XamlReader.Load(xmlTextReader);

            return rectangleDeep;
        }
    }
}
