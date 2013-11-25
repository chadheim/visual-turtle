using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TurtleVM
{
    public class Tokenizer
    {
        List<TokenType> tokenTypes;
        string input;
        Token token;

        public Token Current { get { return token; } }

        public delegate void OnMatched(Token token);
        public event OnMatched onMatched;

        public Tokenizer(List<TokenType> tokenTypes, string input)
        {
            this.tokenTypes = tokenTypes;
            this.input = input;
        }

        bool Match()
        {
            foreach (TokenType tokenType in tokenTypes)
            {
                Match match = tokenType.Match(input);
                if(match.Success)
                {

                    if(tokenType.Output)
                    {
                        token = new Token(tokenType, match.Value);

                        if(onMatched != null)
                            onMatched(token);
                    }
                    else
                    {
                        token = null;
                    }

                    input = input.Substring(match.Length);

                    return true;
                }
            }

            return false;
        }

        public bool Next()
        {
            while (Match())
            {
                if (token != null)
                    return true;
            }
            return false;
        }

        public List<Token> Parse()
        {
            List<Token> tokens = new List<Token>();

            while (Next())
                tokens.Add(token);

            return tokens;
        }
    }
}
