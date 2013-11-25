using System;
using System.Text.RegularExpressions;

namespace TurtleVM
{
    public class TokenType
    {
        Regex pattern;
        bool output;
        string desc;

        public bool Output { get { return output; } }

        public TokenType(String pattern, bool output, string desc)
        {
            this.pattern = new Regex(pattern);
            this.output = output;
            this.desc = desc;
        }

        public Match Match(string input)
        {
            return pattern.Match(input);
        }

        public override string ToString()
        {
            if(desc != null)
                return "[" + desc + "]";
            return "[" + pattern.ToString() + "]";
        }
    }
}
