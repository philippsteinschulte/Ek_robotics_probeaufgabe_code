using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ek_robotics_probeaufgabe_code.Interpreter.Symbols
{
    internal class Multiplication : TreeNode
    {
        public Multiplication(TreeNode leftChild, TreeNode rightChild) : base(leftChild, rightChild)
        {
        }
    }
}
