using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleVM
{
    public class SymbolTable
    {
        Dictionary<string, SymbolValue> symbols;

        public SymbolTable()
        {
            symbols = new Dictionary<string, SymbolValue>();
        }

        public void Add(string id, SymbolValue value)
        {
            symbols.Add(id, value);
        }

        public SymbolValue this[string id]
        {
            get { return symbols[id]; }
            set { symbols[id] = value; }
        }

        public bool Exists(string id)
        {
            return symbols.ContainsKey(id);
        }
    }
}
