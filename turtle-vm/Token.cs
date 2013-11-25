using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleVM
{
    public class Token
    {
        TokenType type;
        public TokenType Type { get { return type; } }

        string text;
        public string Text { get { return text; } }

        public Token(TokenType type, string text)
        {
            this.type = type;
            this.text = text;
        }

        public override string ToString()
        {
            return "[Type: " + type + " Text: " + text + "]";
        }
    }

}
