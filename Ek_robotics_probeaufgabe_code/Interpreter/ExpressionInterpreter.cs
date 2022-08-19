using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ek_robotics_probeaufgabe_code.Interpreter.Symbols;



namespace Ek_robotics_probeaufgabe_code.Interpreter
{
    class ExpressionInterpreter
    {

        public string expressionAsText { get; set; }
        public class Variable
        {
            public Variable(char name)
            {
                this.name = name;
            }
            public char name { get; private set; }
        }
        public ExpressionInterpreter(string expressionAsText)
        {
            this.expressionAsText = expressionAsText; 
        }

        /// <summary>
        /// Calculates the value of the expression, given to the ExpressionInterpreter
        /// when inserting the values of the varibales encode in the dictonary valuesOfVariables
        /// </summary>
        /// <param name="valuesOfVariables">Dictionary containing the varibales and Values for the expression</param>
        /// <returns>The result of the expresiion as an integer value</returns>
        /// <exception cref="ArgumentNullException">Exception is thrown when the expression is empty</exception>
        public int CalculateWith(Dictionary<Variable, int> valuesOfVariables)
        {
            if (this.expressionAsText.Equals("")) 
            {
                throw new ArgumentNullException(Constants.expressionEmptyError);
            }
            this.expressionAsText = preProcessExpression(this.expressionAsText, valuesOfVariables);
            TreeNode expressionAsTree = ParseString(expressionAsText); 
            return (int)expressionAsTree.evaluate();    
        }

        
        /// <summary>
        /// Returns the expression as a binary tree, which can be evaluated
        /// </summary>
        /// <param name="expressionAsText">The expression as a string</param>
        /// <returns>The expression as a binary tree</returns>
        /// <exception cref="ArgumentException">Exception is thrown when the expression contains invalid operators</exception>
        private TreeNode ParseString(string expressionAsText)
        { 
            // Remove outer parenthesis
            expressionAsText = RemoveOutsideParenthesis(expressionAsText);

            // Find the root, e.g the least important operator
            Tuple<string, int> root = GetEquationDivider(expressionAsText);
            string dividerElement = root.Item1;
            int dividerElementIndex = root.Item2;

            // Check if the recursion has reached a leaf, e.g no operators are left in the expression
            if (dividerElementIndex == -1)
            {
                if (Int32.TryParse(expressionAsText, out int leafValue))
                {
                    TreeNode leafNode = new Literal(null, null, leafValue);
                    return leafNode;
                }
                else
                {
                    throw new ArgumentException(Constants.expressionInvalidNumberLiterals);
                } 
            }
            else
            {
                // Divide the expression in the left and right subpart
                string leftSubString = expressionAsText.Substring(0, dividerElementIndex);
                string rightSubString = expressionAsText.Substring(dividerElementIndex + 1);
                // Build the correct treeNode and do the recursive call
                switch (dividerElement)
                {
                    case "+":
                        return new Addition(ParseString(leftSubString), ParseString(rightSubString));         
                    case "-":
                        return new Subtraction(ParseString(leftSubString), ParseString(rightSubString));                       
                    case "*":
                        return new Multiplication(ParseString(leftSubString), ParseString(rightSubString));
                    default:
                        throw new ArgumentException(Constants.expressionInvalidOperator);
                }
            }
        }

        /// <summary>
        /// Divides an expression into left and right part by finding the element with 
        /// the which will be evaluated at last
        /// </summary>
        /// <param name="expressionAsText">The expressin as a string</param>
        /// <returns>A tuple containing the element where to divide and the corresponding index</returns>
        private Tuple<string, int> GetEquationDivider(string expressionAsText)
        {
            string dividerElement = String.Empty;
            int dividerElementIndex = -1;
            for (int i = 0; i < expressionAsText.Length; i++)
            {
                string cur_el = Convert.ToString(expressionAsText[i]);
                if (cur_el.Equals("("))
                {
                    // Set i to the location of the corresponding closing bracket
                    i = i + getClosingBracket(expressionAsText.Substring(i));
                    continue;
                }
                if (cur_el.Equals("+") || cur_el.Equals("*") || cur_el.Equals("-"))
                {
                    if (dividerElement.Length == 0)
                    {
                        dividerElement = cur_el;
                        dividerElementIndex = i;
                    }
                    else if (getOperatorValue(dividerElement) >= getOperatorValue(cur_el))
                    {
                        dividerElement = cur_el;
                        dividerElementIndex = i;
                    }
                }
            }
            
            return new Tuple<string, int>(dividerElement, dividerElementIndex);
        }

        /// <summary>
        /// Removes whitespaces and outer parenthesis. Then substututes the variables for their values
        /// </summary>
        /// <param name="expressionAsText">The expression as a string</param>
        /// <param name="valuesOfVariables">the variables and values which will be substituted</param>
        /// <returns></returns>
        private string preProcessExpression(string expressionAsText, Dictionary<Variable, int> valuesOfVariables)
        {
            // Remove all whitespaces
            expressionAsText = String.Concat(expressionAsText.Where(c => !Char.IsWhiteSpace(c))); ;
            // Remove outside Parenthesis if there are some
            expressionAsText = RemoveOutsideParenthesis(expressionAsText);
            // Substitute the variables with the actual values
            expressionAsText = substituteVariables(valuesOfVariables, expressionAsText);

            return expressionAsText;
        }

        /// <summary>
        /// Finds the corresponding closing bracket for an opening one. Calls itself recursivly when another 
        /// opening bracket is found
        /// </summary>
        /// <param name="subExpression">The expression as a string</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Exception is thrown when the number of closing and opening brackets 
        /// does not match</exception>
        private int getClosingBracket(string subExpression)
        {
            // We start at index 1 here because index 0 is where the opening bracket lies
            for(int i = 1; i < subExpression.Length; i++)
            {
                string cur_el = Convert.ToString(subExpression[i]);
                if (cur_el.Equals(")"))
                {
                    return i; 
                }
                else if (cur_el.Equals("("))
                {
                    // Set i to the index of the inner closing bracket + 1
                    i = i + getClosingBracket(subExpression.Substring(i));
                }
            }

            throw new ArgumentException(Constants.expressionWrongBracketing);
        }

        /// <summary>
        /// Since a *  gets evaluated before a + or - this function delivers a value for the three, which corresponds to their
        /// hierachy. These values can then be used to compare the operators
        /// </summary>
        /// <param name="operator_el">The operator as a String</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Exception is thrown when the operator is not "+", "-" or "*"</exception>
        private int getOperatorValue(string operator_el)
        {
            if (operator_el.Equals("+") || operator_el.Equals("-"))
            {
                return 0;
            }
            else if(operator_el.Equals("*"))
            {
                return 1;
            }
            else
            {
                throw new ArgumentException(Constants.expressionInvalidOperator); 
            }
        }

        /// <summary>
        /// Remove the outer parenthesis from an equation, if there are some.
        /// </summary>
        /// <param name="expressionAsText">The expression as a String</param>
        /// <returns>The expression without outer parenthesis</returns>
        private string RemoveOutsideParenthesis(string expressionAsText)
        {
            // Check first and last element for parenthesis
            if (expressionAsText[0] == '(')
            {
                // Find coresponding closing bracket and check if it is the last element 
                int closingBracketIndex = getClosingBracket(expressionAsText);
                if (closingBracketIndex == expressionAsText.Length - 1)
                {
                    expressionAsText = expressionAsText.Substring(1);
                    expressionAsText = expressionAsText.Remove(expressionAsText.Length - 1);
                }
            }
            return expressionAsText;
        }

        /// <summary>
        /// Substitutes the variables in the equation for their corresponding values in the dictionary
        /// </summary>
        /// <param name="valuesOfVariables">Dictionary containing the values for the variables</param>
        /// <param name="expressionAsText">The expression as a string</param>
        /// <returns></returns>
        private string substituteVariables(Dictionary<Variable, int> valuesOfVariables, string expressionAsText){
            foreach(var variableValueCombination in valuesOfVariables)
            {
                char operand = variableValueCombination.Key.name;
                int value = variableValueCombination.Value;
                expressionAsText = expressionAsText.Replace(operand, Convert.ToChar(Convert.ToString(value)));
            }

            return expressionAsText;
        }

    }
}
