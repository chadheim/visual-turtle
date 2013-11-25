using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleVM
{
    class ASTBuilder
    {
        Stack<AST.Node> nodes;

        AST.Node root;
        
        public ASTBuilder()
        {
            nodes = new Stack<AST.Node>();
        }

        public AST.Node Build()
        {
            return root;
        }

        public void Push(AST.Node n)
        {
            if (nodes.Count == 0)
                root = n;

            nodes.Push(n);
        }

        public void Pop()
        {
            nodes.Pop();
        }

        public void Add(AST.Node child)
        {
            if (nodes.Count > 0)
                nodes.Peek().Add(child);
        }
    }
}
