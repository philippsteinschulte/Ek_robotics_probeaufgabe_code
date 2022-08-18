using static worksample.ExpressionInterpreter;
using worksample;

int example()
{
    var interpreter = new ExpressionInterpreter("3*x + 20- y *(z + 17)");
    var values = new Dictionary<Variable, int>();
    values.Add(new Variable('x'), 1);
    values.Add(new Variable('y'), 2);
    values.Add(new Variable('z'), 3);
    int result = interpreter.CalculateWith(values);
    return result;
}