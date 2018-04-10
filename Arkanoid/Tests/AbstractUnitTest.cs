using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Arkanoid.Model.Misc;

namespace Arkanoid.Tests
{
    /// <summary>
    /// The core class for all unit tests
    /// </summary>
    abstract partial class AbstractUnitTest
    {

        // Num of successful and unsuccessful inner tests
        public uint successful = 0;
        public uint unsuccessful = 0;
    }

    abstract partial class AbstractUnitTest
    {
 
        /// <summary>
        /// Each respective test has to implement this method
        /// </summary>
        /// <returns>Success/failure</returns>
        public abstract bool Run();
        
        /// <summary>
        /// Tear down of each test (currently, we have no resources, so just do the log)
        /// </summary>
        public void TearDown()
        {
            Logger.Log(this.GetType().Name + " teardown, succ=" + this.successful + ", unsuc=" + this.unsuccessful);
        }
    }
}
