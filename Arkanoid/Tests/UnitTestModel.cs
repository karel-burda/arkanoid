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
    /// Presents test that is focused on testing the application model
    /// </summary>
    partial class UnitTestModel : AbstractUnitTest
    {
    }

    partial class UnitTestModel : AbstractUnitTest
    {

        public override bool Run()
        {

            Logger.Log("Running test " + this.GetType().Name);

            // Test Game object
            Logger.Log("Testing Game object");
            Game game = Game.Instance;
            if(game == null)
            {
                this.unsuccessful++;
            }
            else {
                this.successful++;
            }

            // Create some primitive objects
            Logger.Log("Creating some primitive objects");
            // Test Bricks
            Brick myBrick = new Brick(10,20,100,200);
            if (myBrick == null)
            {
                this.unsuccessful++;
            }
            else
            {
                this.successful++;

                // Check bounds and inner rectangle as well
                if (myBrick.X == 10 && myBrick.Y == 20 && myBrick.Width == 100 && myBrick.Height == 200 
                    && myBrick.View.Rectangle.Width == 100 && myBrick.View.Rectangle.Height == 200)
                {
                    this.successful++;
                }
                else
                {
                    this.unsuccessful++;
                }

                // Test equalty of objects
                Brick myBrickCopy = new Brick(myBrick.X, myBrick.Y, myBrick.Width, myBrick.Height);
                Brick otherBrick = new Brick(0.0, 0.0, 90.0, 60.0);
                
                // Test equalty
                if (myBrick == myBrickCopy)
                {
                    this.successful++;
                }
                else
                {
                    this.unsuccessful++;
                }
                if (myBrick == otherBrick)
                {
                    this.unsuccessful++;
                }
                else
                {
                    this.successful++;
                }

                if (myBrick.Equals(myBrickCopy))
                {
                    this.successful++;
                }
                else
                {
                    this.unsuccessful++;
                }
                if (myBrick.Equals(otherBrick))
                {
                    this.unsuccessful++;
                }
                else
                {
                    this.successful++;
                }

                if (myBrick != otherBrick)
                {
                    this.successful++;
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
