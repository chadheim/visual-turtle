using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleVM
{
    public class Interpreter
    {
        SymbolTable symbols;

        public SymbolTable Symbols { get { return symbols; } }
   
        public Interpreter()
        {
            symbols = new SymbolTable();
        }

        public float Evaluate(AST.Node node)
        {
            switch(node.type)
            {
                case AST.NodeType.Program:
                    if (node.children != null)
                        foreach (AST.Node n in node.children)
                            Evaluate(n);
                  break;
                case AST.NodeType.Block:
                    if(node.children != null)
                        foreach (AST.Node n in node.children)
                            Evaluate(n);
                  break;
                case AST.NodeType.Call:
                    if(symbols.Exists(node.text))
                        if(symbols[node.text].procedure != null)
                            symbols[node.text].procedure();
                  break;
            }
            return 0;
        }
    }
}
