using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TurtleVM
{
    public static class TokenTypes
    {
        public static readonly TokenType ODD = new TokenType("^ODD", true, "ODD");
        public static readonly TokenType CONST = new TokenType("^CONST", true, "CONST");
        public static readonly TokenType ASSIGN = new TokenType("^:=", true, "ASSIGN");
        public static readonly TokenType COMMA = new TokenType("^,", true, "COMMA");
        public static readonly TokenType PERIOD = new TokenType("^.", true, "PERIOD");
        public static readonly TokenType SEMI = new TokenType("^;", true, "SEMI");
        public static readonly TokenType VAR = new TokenType("^VAR", true, "VAR");
        public static readonly TokenType PROC = new TokenType("^PROCEDURE", true, "PROC");
        public static readonly TokenType CALL = new TokenType("^CALL", true, "CALL");
        public static readonly TokenType BEGIN = new TokenType("^BEGIN", true, "BEGIN");
        public static readonly TokenType END = new TokenType("^END", true, "END");
        public static readonly TokenType IF = new TokenType("^IF", true, "IF");
        public static readonly TokenType WHILE = new TokenType("^WHILE", true, "WHILE");
        public static readonly TokenType THEN = new TokenType("^THEN", true, "THEN");
        public static readonly TokenType DO = new TokenType("^DO", true, "DO");
        public static readonly TokenType LPAREN = new TokenType("^\\(", true, "LPAREN");
        public static readonly TokenType RPAREN = new TokenType("^\\)", true, "RPAREN");
        public static readonly TokenType ID = new TokenType("^(\\w)+", true, "ID");
        public static readonly TokenType NUMBER = new TokenType("^-?[0-9]*\\.?[0-9]+", true, "NUMBER");
        public static readonly TokenType COMP = new TokenType("^(=|#|<=|>=|<|>)", true, "COMP");
        public static readonly TokenType ADDSUB = new TokenType("^(\\+|\\-)", true, "ADDSUB");
        public static readonly TokenType MULDIV = new TokenType("^(\\*|/)", true, "MULDIV");
        public static readonly TokenType WHITESPACE = new TokenType("^(\\s)+", false, "WHITESPACE");

        public static readonly List<TokenType> ALL_TYPES = new List<TokenType>() {
                WHITESPACE,
                CONST, VAR, PROC, CALL, BEGIN, END, IF, WHILE, THEN, DO, ODD,
                NUMBER, ID,
                ASSIGN, COMP, ADDSUB, MULDIV, LPAREN, RPAREN, COMMA, SEMI, PERIOD
        };
    }
}
