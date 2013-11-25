using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleVM
{
    public class SymbolValue
    {
        public AST.Node node;
        public float number;
        public string text;

        public delegate void Procedure();
        public Procedure procedure;

        public SymbolValue(AST.Node node)
        {
            this.node = node;
        }

        public SymbolValue(float number)
        {
            this.number = number;
        }

        public SymbolValue(string text)
        {
            this.text = text;
        }

        public SymbolValue(Procedure procedure)
        {
            this.procedure = procedure;
        }
    }
}
