using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Arkanoid.Model;
using Arkanoid.Model.Misc;

namespace Arkanoid.Model.Levels
{
    /// <summary>
    /// Level 1 definition
    /// </summary>
    partial class Level1 : AbstractLevel
    {

    }
    
    /// <summary>
    /// Methods
    /// </summary>
    partial class Level1 : AbstractLevel
    {

        // Overriden stuff
        public override bool SetUp()
        {

            this.LevelID = "Level 1";
            
            // Set up a few bricks and other stuff :)
            Brick brick = new Brick(100.0, 100.0, 60.0, 30.0);
            this.Objects.Add(brick);
            brick = new Brick(260.0, 145.0, 75.0, 20.0);
            this.Objects.Add(brick);
            brick = new Brick(480.0, 205.0, 100.0, 40.0);
            this.Objects.Add(brick);
            brick = new Brick(470.0, 100.0, 60.0, 40.0);
            this.Objects.Add(brick);

            return true;
        }

        // Overriden
        public override double GetDefaultBallSpeed()
        {
            return 8.0*GameSettings.GetMovingUnit();
        }

        // Overriden
        public override double GetDefaultBatSpeed()
        {
            return 10.0*GameSettings.GetMovingUnit();
        }
    }
}
