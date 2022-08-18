using NUnit.Framework;
using worksample;
using static worksample.ExpressionInterpreter;

namespace Ek_robotics_probeaufgabe_code.Tests
{
    [TestFixture]
    class TestExpressionInterpreter
    {
        [Test]
        public void test_test()
        {
            // Arrange 
            string testExpression = "(3*x+20-y*(z+17))";
            ExpressionInterpreter interpreter = new ExpressionInterpreter(testExpression);
            var values = new Dictionary<Variable, int>();
            values.Add(new Variable('x'), 1);
            values.Add(new Variable('y'), 2);
            values.Add(new Variable('z'), 3);
            // Action 
            int result = interpreter.CalculateWith(values);
            // Assert
            int expexted = -17;
            Assert.AreEqual(result, expexted);
        }

    }
}
