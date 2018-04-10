using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Arkanoid.Model;
using Arkanoid.Model.Misc;
using Arkanoid.Presenters;

namespace Arkanoid.Tests
{
    /// <summary>
    /// Testing functionality of Levels
    /// </summary>
    partial class UnitTestLevel : AbstractUnitTest
    {
    }

    partial class UnitTestLevel : AbstractUnitTest
    {

        public override bool Run()
        {

            Logger.Log("Running test " + this.GetType().Name);

            // Create objects and inspects the distance
            Brick myBrick = new Brick(100, 100, 100, 50);
            Brick otherBrick = new Brick(230.0, 110.0, 20.0, 20.0);

            if (myBrick == null || otherBrick == null)
            {
                this.unsuccessful++;
            }
            else
            {
                double distance = 0.0;

                distance = PhysicsManager.GetStraightDistanceBetweenObjects(myBrick, otherBrick);
                if (Math.Round(distance, 2) == 31.62)
                {
                    this.successful++;
                }
                else
                {
                    this.unsuccessful++;
                }

                otherBrick.Y = 471.0;
                otherBrick.X = 21.0;
                distance = PhysicsManager.GetStraightDistanceBetweenObjects(myBrick, otherBrick);
                if (Math.Round(distance, 2) == 326.38)
                {
                    this.successful++;
                }
                else
                {
                    this.unsuccessful++;
                }
            }


            // Teardown
            this.TearDown();

            // Upon return, check results
            if (this.unsuccessful > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
