using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Arkanoid.Model;

namespace Arkanoid.Model
{

    /// <summary>
    /// Enumeration defining game speed
    /// Values should be in interval (0, +INF)
    /// Values are later resolved to the floating point number (e.g. 70 = 0.7)
    /// Effectively represents speed multiplier
    /// </summary>
    public enum GameSpeed
    {
        SpeedSlow = 70,
        SpeedNormal = 100,
        SpeedFast = 130
    };

    /// <summary>
    /// Enum is defining Game modes
    /// "ModeDestroyable" = Bricks are going to be breaked
    /// "ModeUnDestroyable" = Bricks cannot be breaked
    /// </summary>
    public enum GameMode
    {
        ModeDestroyable = 0,
        ModeUnDestroyable
    }

    /// <summary>
    /// The class represents setting of a game and related meta-information
    /// </summary>
    partial class GameSettings
    {

        // Current score
        public ulong Score { get; set; }

        // Game mode
        public GameMode Mode { get; protected set; }

        // Game speed
        public GameSpeed Speed { get; protected set; }

        // Runnig?
        public bool IsRunning { get; set; }
    }

    /// <summary>
    /// Methods
    /// </summary>
    partial class GameSettings
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public GameSettings()
        {
            this.SetDefault();
        }

        /// <summary>
        /// Sets default inner values
        /// </summary>
        public void SetDefault()
        {
            this.Score = 0;
            this.Speed = GameSpeed.SpeedNormal;
            this.Mode = GameMode.ModeUnDestroyable;
            this.IsRunning = false;
        }

        /// <summary>
        /// The method returns a speed factor resolved from the enumeration
        /// <seealso cref="GameSpeed"/>
        /// </summary>
        public static double GetSpeedFactor(GameSpeed speed)
        {
            return ((double)speed/100);
        }

        /// <summary>
        /// Speed multiplier wrapper
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="speed"></param>
        /// <returns>How many times can object move per metrics unit (e.g. one pixel)</returns>
        /// <seealso cref="AbstractObject.MoveBy"/>
        public static uint GetTotalSpeedMultiplier(AbstractObject obj, GameSpeed speed)
        {
            if (obj == null)
            {
                return 0;
            }

            return Convert.ToUInt32(Math.Ceiling(obj.Speed * GameSettings.GetSpeedFactor(speed)));
        }

        /// <summary>
        /// Helper method
        /// </summary>
        /// <returns>Base metric unit for moving (e.g. one point)</returns>
        public static double GetMovingUnit()
        {
            return (1.0);
        }
    }
}
