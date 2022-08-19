namespace Ek_robotics_probeaufgabe_code.Interpreter.Symbols
{
    internal class Literal : TreeNode
    {
        public int value { get; private set; }
        public Literal(TreeNode leftChild, TreeNode rightChild, int value) : base(leftChild, rightChild)
        {
            this.value = value;
        }

        public override int? evaluate()
        {
            return value;
        }
    }
}
