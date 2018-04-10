using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Arkanoid.Model.Misc
{
    /// <summary>
    /// Sample class that presents critical errors to the user
    /// </summary>
    static partial class ErrorPresenter
    {
    }

    /// <summary>
    /// Methods
    /// </summary>
    static partial class ErrorPresenter
    {
        /// <summary>
        /// Sample method for error presenting
        /// </summary>
        /// <param name="pMessage">string to present</param>
        public static void Show(String pMessage)
        {
            MessageBox.Show(pMessage);
        }
    }
}
