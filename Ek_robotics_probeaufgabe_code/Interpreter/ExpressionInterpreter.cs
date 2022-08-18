using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ek_robotics_probeaufgabe_code.Interpreter.Symbols;



namespace worksample
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
        public int CalculateWith(Dictionary<Variable, int> valuesOfVariables)
        {
            this.expressionAsText = preProcessExpression(this.expressionAsText, valuesOfVariables);
            TreeNode expressionAsTree = ParseString(expressionAsText); 
            return (int)expressionAsTree.evaluate();    
        }

       
        private TreeNode ParseString(string expressionAsText)
        { 
            // Remove outer parenthesis
            expressionAsText = RemoveOutsideParenthesis(expressionAsText);

            // Find the root, e.g the least important operator
            string dividerElement = String.Empty;
            int dividerElementIndex = -1;
            for(int i = 0; i < expressionAsText.Length; i++)
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
                        throw new ArgumentException("Invalid operator in the expression");
                }
            }
        }

        private string preProcessExpression(string expressionAsText, Dictionary<Variable, int> valuesOfVariables)
        {
            // Remove all whitespaces --> TODO funztz gerade nicht 
            expressionAsText = Regex.Replace(expressionAsText, @"s", "");
            // Remove outside Parenthesis if there are some
            expressionAsText = RemoveOutsideParenthesis(expressionAsText);
            // Substitute the variables with the actual values
            expressionAsText = substituteVariables(valuesOfVariables, expressionAsText);

            return expressionAsText;
        }

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

        private string substituteVariables(Dictionary<Variable, int> valuesOfVariables, string expressionAsText){
            foreach(var variableValueCombination in valuesOfVariables)
            {
                char operand = variableValueCombination.Key.name;
                int value = variableValueCombination.Value;
                // TODO-->Dieser doppelte Typecast geht bestimmt noch hübscher
                expressionAsText = expressionAsText.Replace(operand, Convert.ToChar(Convert.ToString(value)));
            }

            return expressionAsText;
        }

    }
}
