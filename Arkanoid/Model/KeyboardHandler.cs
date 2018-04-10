using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using Arkanoid.Model;
using Arkanoid.Model.Levels;
using Arkanoid.Model.Misc;
using Arkanoid.Presenters;

namespace Arkanoid.Model
{   
    /// <summary>
    /// Handles key presses from user
    /// </summary>
    partial class KeyboardHandler
    {

    }

    /// <summary>
    /// Methods
    /// </summary>
    partial class KeyboardHandler
    {

        /// <summary>
        /// Key has been pressed event
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arguments</param>
        public void KeyDown(object sender, KeyEventArgs e)
        {
            
            // Arrow keys are pressed, move with the Bat
            if (e.Key == Key.Left)
            {
               Game.Instance.Level.Bat.Move(Direction.W);
            }
            else if (e.Key == Key.Right)
            {
                Game.Instance.Level.Bat.Move(Direction.E);
            }
            // Elevate the ball
            else if (e.Key == Key.Up)
            {
                Game.Instance.Level.Ball.ElevateBallFromTheBat();
            }

            // Space pressed
            else if (e.Key == Key.Space)
            {
                Game.Instance.Pause();
            }
        }
    }
}
