namespace Ek_robotics_probeaufgabe_code.Interpreter.Symbols
{
    internal class Addition : TreeNode
    {
        public Addition(TreeNode leftChild, TreeNode rightChild) : base(leftChild, rightChild)
        {
        }

        public override int? evaluate()
        {
            return leftChild?.evaluate() + rightChild?.evaluate();
        }
    }
}
