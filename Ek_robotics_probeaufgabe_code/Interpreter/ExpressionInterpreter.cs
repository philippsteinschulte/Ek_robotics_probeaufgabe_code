using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ek_robotics_probeaufgabe_code.Interpreter.Symbols;



namespace worksample
{
    class ExpressionInterpreter
    {
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
            // ...
        }
        public int CalculateWith(Dictionary<Variable, int> valuesOfVariables)
        {
            // ...
        }
       
        private TreeNode ParseString(string expressionAsText, Dictionary<Variable, int> valuesOfVariables)
        {

            // TODO Dieser ganze kram muss nur einmal am Anfang gemacht werden, kann also ausgelagert werden, dann mu
            // ss die Methode auch das dictionary nicht übergeben bekommen 
            // Remove all whitespaces
            expressionAsText = Regex.Replace(expressionAsText, @"s", "");
            // Remove outside Parenthesis if there are some
            expressionAsText = RemoveOutsideParenthesis(expressionAsText); 
            // Substitute the variables with the actual values
            expressionAsText = substituteVariables(valuesOfVariables, expressionAsText); 
           
            // Find the root, e.g the least important operator
            string dividerElement = String.Empty;
            int dividerElementIndex = -1;
            for(int i = 0; i < expressionAsText.Length; i++)
            {
                string cur_el = Convert.ToString(expressionAsText[i]);
                if (cur_el.Equals("("))
                {
                    // Set i to the location of the corresponding closing bracket
                    i = getClosingBracket(expressionAsText.Substring(i)) + 1;
                }
                if (cur_el.Equals("+") || cur_el.Equals("*") || cur_el.Equals("-"))
                {
                    if (dividerElement.Length == 0)
                    {
                        dividerElement = cur_el;
                        dividerElementIndex = i;
                    }
                    else if (getOperatorValue(dividerElement) <= getOperatorValue(cur_el))
                    {
                        dividerElement = cur_el;
                        dividerElementIndex = i;
                    }
                }
            }

            // Check if the recursion has reached a leaf, e.g no operators are left in the expression
            if (dividerElementIndex == -1)
            {
                // Parse string to int
                if (Int32.TryParse(expressionAsText, out int leafValue))
                {
                    TreeNode leafNode = new Literal(null, null, leafValue);
                    return leafNode;
                }
                else
                {
                    throw new ArgumentException("Expression contains invalid number literals");
                } 
            }
            else
            {
                string leftSubString = expressionAsText.Substring(0, dividerElementIndex-1);
                string rightSubString = expressionAsText.Substring(dividerElementIndex+1, expressionAsText.Length);

                if (dividerElement.Equals("+"))
                {
                    TreeNode subtree = new Multiplication(ParseString(leftSubString), ParseString(rightSubString));
                }
                else if (dividerElement.Equals("-"))
                {

                }
                else if (dividerElement.Equals("+"))
                {

                }
            }
            // Build the treeNode and evaluate the leftChild and the rightChild

        }

        private int getClosingBracket(string subExpression)
        {
            for(int i = 0; i < subExpression.Length; i++)
            {
                string cur_el = Convert.ToString(subExpression[i]);
                if (cur_el.Equals(")"))
                {
                    return i; 
                }
                else if (cur_el.Equals("("))
                {
                    // Set i to the index of the inner closing bracket + 1
                    i = getClosingBracket(subExpression.Substring(i)) + 1;
                }
            }

            throw new ArgumentException("Clamping of the expression is wrong");
        }

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
                throw new ArgumentException("Operants can only be of +, - and *"); 
            }
        }

        private string RemoveOutsideParenthesis(string expressionAsText)
        {
            // Check first and last element for parenthesis
            if (expressionAsText[0] == '(')
            {
                expressionAsText = expressionAsText.Substring(1);
            }
            if (expressionAsText[expressionAsText.Length] == ')')
            {
                expressionAsText = expressionAsText.Remove(expressionAsText.Length - 1); 
            }

            return expressionAsText;
        }

        private string substituteVariables(Dictionary<Variable, int> valuesOfVariables, string expressionAsText){
            foreach(var variableValueCombination in valuesOfVariables)
            {
                char operand = variableValueCombination.Key.name;
                int value = variableValueCombination.Value;
                expressionAsText = expressionAsText.Replace(operand, Convert.ToChar(value));
            }

            return expressionAsText;
        }

    }
}
