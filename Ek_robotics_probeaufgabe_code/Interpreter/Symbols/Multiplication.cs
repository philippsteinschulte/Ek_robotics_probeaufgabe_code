namespace Ek_robotics_probeaufgabe_code.Interpreter.Symbols
{
    internal class Multiplication : TreeNode
    {
        public Multiplication(TreeNode leftChild, TreeNode rightChild) : base(leftChild, rightChild)
        {
        }

        public override int? evaluate()
        {
            return leftChild?.evaluate() * rightChild?.evaluate();
        }
    }
}
