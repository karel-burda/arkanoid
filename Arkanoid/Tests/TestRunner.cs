using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Arkanoid.Model.Misc;
using Arkanoid.Tests;

namespace Arkanoid.Tests
{
    /// <summary>
    /// Sample class that runs all specified unit tests
    /// </summary>
    partial class TestRunner
    {
    }

    partial class TestRunner
    {
        /// <summary>
        /// This simply runs all defined unit tests (in its body)
        /// </summary>
        /// <returns>True = All tests have passed</returns>
        public bool RunTests()
        {

            Logger.Log("**********************Running all tests");

            // Return value
            bool retValue = false;

            // Create all tests
            UnitTestModel testModel = new UnitTestModel();
            UnitTestLevel testLevel = new UnitTestLevel();

            // Run them and inspect results
            try
            {
                // Model test
                retValue = testModel.Run();
                testModel = null;

                // Level test
                retValue = testLevel.Run();
                testLevel = null;
            }
            // If there is some exception, let's consider it as a failure
            catch (Exception)
            {
                retValue = false;
            }
            finally {
                // Delete all tests
                testModel = null;
                testLevel = null;
            }

            Logger.Log("**********************End all tests, return = " + retValue);

            // Returns appropriate value
            return retValue;
        }
    }
}
