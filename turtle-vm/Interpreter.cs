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
            return 0;
        }
    }
}
