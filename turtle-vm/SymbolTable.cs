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
        SymbolTable parent;
        public SymbolTable Parent { get { return parent; } }

        public SymbolTable(SymbolTable parent)
        {
            symbols = new Dictionary<string, SymbolValue>();
            this.parent = parent;
        }

        public void Add(string id, SymbolValue value)
        {
            symbols.Add(id, value);
        }

        public SymbolValue this[string id]
        {
            set { symbols[id] = value; }
            get 
            {
                if (symbols.ContainsKey(id))
                    return symbols[id];

                if (parent != null)
                    return parent[id];

                //throws an exception
                return symbols[id];
            }
        }

        public bool Exists(string id)
        {
            if (symbols.ContainsKey(id))
                return true;

            if (parent != null)
                return parent.Exists(id);

            return false;
        }
    }
}
