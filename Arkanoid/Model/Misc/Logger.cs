using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arkanoid.Model.Misc
{
    /// <summary>
    /// The class represents a simple logging machine for this little project :)
    /// </summary>
    static class Logger
    {
        // Use console or a file?
        public static bool useConsole = true;

        /// <summary>
        /// Sample function for logging
        /// Can write directly to stdout or to a file
        /// </summary>
        /// <param name="pMessage">string to log</param>
        public static void Log(String pMessage)
        {

            if (Logger.useConsole)
            {
                System.Diagnostics.Debug.WriteLine(pMessage); 
            }
        }
    }
}
