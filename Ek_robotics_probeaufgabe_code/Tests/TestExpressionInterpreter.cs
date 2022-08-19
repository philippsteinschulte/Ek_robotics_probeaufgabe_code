using NUnit.Framework;
using Ek_robotics_probeaufgabe_code.Interpreter;
using static Ek_robotics_probeaufgabe_code.Interpreter.ExpressionInterpreter;
using static Ek_robotics_probeaufgabe_code.Interpreter.Constants;
using System;

namespace Ek_robotics_probeaufgabe_code.Tests
{
    /// <summary>
    /// This is the test class for the Expression interpreter. 
    /// The naming convention for all tests is as follows: 
    /// public void FunctionName__TestInput__ExpectedResult
    /// </summary>
    [TestFixture]
    class TestExpressionInterpreter
    {

        // Test all valid input possibilities
        [Test]
        public void CalculateWith__Equation_with_outer_bracketing__correct_result()
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

        [Test]
        public void CalculateWith__Equation_with_whitespaces__correct_result()
        {
            // Arrange 
            string testExpression = "3*x+20- y* (z+ 1 7)";
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

        [Test]
        public void CalculateWith__Equation_without_bracketing__correct_result()
        {
            // Arrange 
            string testExpression = "3*x+20-y*z+17";
            ExpressionInterpreter interpreter = new ExpressionInterpreter(testExpression);
            var values = new Dictionary<Variable, int>();
            values.Add(new Variable('x'), 1);
            values.Add(new Variable('y'), 2);
            values.Add(new Variable('z'), 3);
            // Action 
            int result = interpreter.CalculateWith(values);
            // Assert
            int expexted = 34;
            Assert.AreEqual(result, expexted);
        }

        // Test for invalid inputs and error cases
        [Test]
        public void CalculateWith__Empty_equation__throw_ArgumentNullException()
        {
            // Arrange 
            string testExpression = "";
            ExpressionInterpreter interpreter = new ExpressionInterpreter(testExpression);
            var values = new Dictionary<Variable, int>();    
            // Action an Assert
            var exception = Assert.Throws<ArgumentNullException>(() => interpreter.CalculateWith(values));
            Assert.AreEqual(Constants.expressionEmptyError, exception!.ParamName);
        }

        [Test]
        public void CalculateWith__Equation_with_invalid_operator__throw_ArgumentException()
        {
            // Arrange 
            string testExpression = "3*x+20-y#z+17";
            ExpressionInterpreter interpreter = new ExpressionInterpreter(testExpression);
            var values = new Dictionary<Variable, int>();
            // Action an Assert
            Assert.Throws<ArgumentException>(() => interpreter.CalculateWith(values));
        }
        [Test]
        public void CalculateWith__Equation_with_wrong_bracketing__throw_ArgumentException()
        {
            // Arrange 
            string testExpression = "((3*x+20)-y)*z+17)";
            ExpressionInterpreter interpreter = new ExpressionInterpreter(testExpression);
            var values = new Dictionary<Variable, int>();
            // Action an Assert
            Assert.Throws<ArgumentException>(() => interpreter.CalculateWith(values));
        }
    }
}
