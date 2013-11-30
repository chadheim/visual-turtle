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
            float value = 0.0f;

            switch (node.type)
            {
                case AST.NodeType.Program:
                    if (node.children != null)
                        foreach (AST.Node n in node.children)
                            Evaluate(n);
                    break;
                case AST.NodeType.Block:
                    if (node.children != null)
                        foreach (AST.Node n in node.children)
                            Evaluate(n);
                    break;
                case AST.NodeType.Call:
                    if (symbols.Exists(node.text))
                        if (symbols[node.text].procedure != null)
                            symbols[node.text].procedure();
                        else if (symbols[node.text].node != null)
                            Evaluate(symbols[node.text].node);
                    break;
                case AST.NodeType.ConstDecl:
                    symbols.Add(node.text, new SymbolValue(node.number));
                    break;
                case AST.NodeType.VarDecl:
                    symbols.Add(node.text, new SymbolValue(0.0f));
                    break;
                case AST.NodeType.ProcDecl:
                    AST.Node body = null;
                    if (node.children != null && node.children[0] != null)
                        body = node.children[0];
                    symbols.Add(node.text, new SymbolValue(body));
                    break;
                case AST.NodeType.Assign:
                    symbols[node.text].number = Evaluate(node.children[0]);
                    break;
                case AST.NodeType.While:
                    while (Evaluate(node.children[0]) > 0)
                        Evaluate(node.children[1]);
                    break;
                case AST.NodeType.If:
                    if (Evaluate(node.children[0]) > 0)
                        Evaluate(node.children[1]);
                    break;
                case AST.NodeType.Id:
                    value = symbols[node.text].number;
                    break;
                case AST.NodeType.Number:
                    value = node.number;
                    break;
                case AST.NodeType.UniOp:
                    value = Evaluate(node.children[0]);
                    if (node.text.Equals("-"))
                        value = -value;
                    break;
                case AST.NodeType.BinOp:
                    float lhs = Evaluate(node.children[0]);
                    float rhs = Evaluate(node.children[1]);
                    if (node.text.Equals("+"))
                        value = lhs + rhs;
                    else if (node.text.Equals("-"))
                        value = lhs - rhs;
                    else if (node.text.Equals("/"))
                        value = lhs / rhs;
                    else if (node.text.Equals("*"))
                        value = lhs * rhs;
                    break;
            }

            return value;
        }
    }
}
