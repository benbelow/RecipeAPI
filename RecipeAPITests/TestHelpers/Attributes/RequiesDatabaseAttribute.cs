using System;
using NUnit.Framework;
using RecipeAPITests.TestHelpers.Database;

namespace RecipeAPITests.TestHelpers.Attributes
{
    public class RequiresDatabaseAttribute : Attribute, ITestAction
    {
        public ActionTargets Targets
        {
            get { return ActionTargets.Test; }
        }

        public void BeforeTest(TestDetails testDetails)
        {
            TestDb.Delete();
        }

        public void AfterTest(TestDetails testDetails)
        {
            TestDb.Delete();
        }
    }
}
