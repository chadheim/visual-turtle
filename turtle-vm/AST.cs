﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleVM
{
    public class AST
    {
        public enum NodeType { Program, Block, ConstDecl, VarDecl, ProcDecl, Assign, Id, Number, If, While, UniOp, BinOp, Comp, Call };

        public class Node
        {
            public NodeType type;
            public List<Node> children;

            public string text;
            public float number;

            public Node(NodeType type)
            {
                this.type = type;
            }
            
            public void Add(Node node)
            {
                if (node == null)
                    return;

                if (children == null)
                    children = new List<AST.Node>();

                children.Add(node);
            }
        }
        
    }
}
