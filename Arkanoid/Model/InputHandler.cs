using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Arkanoid.Model;

namespace Arkanoid.Model
{
    /// <summary>
    /// The class encapsulates and handles all inputs from user
    /// It contains separate handler for mouse and for keyboard
    /// </summary>
    partial class InputHandler
    {

        // Handler accessors
        public MouseHandler MouseHandler { get; private set; }
        public KeyboardHandler KeyboardHandler { get; private set; }
    }

    /// <summary>
    /// Methods
    /// </summary>
    partial class InputHandler
    { 
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public InputHandler()
        { 

            // Create new inner instances
            this.MouseHandler = new MouseHandler();
            this.KeyboardHandler = new KeyboardHandler();
        }
    }
}
