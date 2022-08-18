using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ek_robotics_probeaufgabe_code.Interpreter.Symbols
{
    internal abstract class TreeNode
    {
        public TreeNode? leftChild { get; set; }
        public TreeNode? rightChild { get; set; }
        public TreeNode(
            TreeNode leftChild,
            TreeNode rightChild
        )
        {
            this.leftChild = leftChild;
            this.rightChild = rightChild;
        }
    }
}
