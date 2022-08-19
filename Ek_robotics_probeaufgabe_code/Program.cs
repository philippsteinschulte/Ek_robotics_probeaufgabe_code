using static Ek_robotics_probeaufgabe_code.Interpreter.ExpressionInterpreter;
using Ek_robotics_probeaufgabe_code.Interpreter;

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