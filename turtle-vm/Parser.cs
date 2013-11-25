using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleVM
{
    public class Parser
    {
        Tokenizer tokenizer;
        ASTBuilder builder;

        public Parser(Tokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
        }

        bool Accept(TokenType type, out Token token)
        {
            Token current = tokenizer.Current;
            if (current != null && current.Type == type)
            {
                token = current;
                tokenizer.Next();
                return true;
            }
            token = null;
            return false;
        }

        bool Expect(TokenType type, out Token token)
        {
            if (Accept(type, out token))
                return true;

            throw new Exception("Expected: " + type + " Actual: " + tokenizer.Current);
            //Debug.WriteLine("Expected: " + type + " Actual: " + tokenizer.Current);
            //return false;
        }

        bool Accept(TokenType type)
        {
            Token token;
            return Accept(type, out token);
        }

        bool Expect(TokenType type)
        {
            Token token;
            return Expect(type, out token);
        }

        public AST.Node Parse()
        {
            tokenizer.Next();

            builder = new ASTBuilder();

            builder.Push(new AST.Node(AST.NodeType.Program));
            Program();
            builder.Pop();

            return builder.Build();
        }
        
        // program = block "."
        void Program()
        {
            AST.Node n = new AST.Node(AST.NodeType.Block);
            builder.Add(n);
            builder.Push(n);
            Block();
            builder.Pop();

            Expect(TokenTypes.PERIOD);
        }

        // block =
        //     ["const" id ":=" number {"," id ":=" number} ";"]
        //     ["var" id {"," id} ";"]
        //     {"procedure" id ";" block ";"} 
        //     statement
        void Block()
        {
            if (Accept(TokenTypes.CONST))
            {
                do
                {
                    Token id, number;
                    Expect(TokenTypes.ID, out id);
                    Expect(TokenTypes.ASSIGN);
                    Expect(TokenTypes.NUMBER, out number);

                    AST.Node n = new AST.Node(AST.NodeType.ConstDecl);
                    n.text = id.Text;
                    n.number = float.Parse(number.Text);
                    builder.Add(n);

                } while (Accept(TokenTypes.COMMA));
                Expect(TokenTypes.SEMI);
            }

            if (Accept(TokenTypes.VAR))
            {
                do
                {
                    Token id;
                    Expect(TokenTypes.ID, out id);

                    AST.Node n = new AST.Node(AST.NodeType.VarDecl);
                    n.text = id.Text;
                    builder.Add(n);

                } while (Accept(TokenTypes.COMMA));
                Expect(TokenTypes.SEMI);
            }

            while (Accept(TokenTypes.PROC))
            {
                Token id;
                Expect(TokenTypes.ID, out id);
                Expect(TokenTypes.SEMI);

                AST.Node n = new AST.Node(AST.NodeType.ProcDecl);
                n.text = id.Text;
                builder.Add(n);
                builder.Push(n);
                Block();
                builder.Pop();

                Expect(TokenTypes.SEMI);
            }

            Statement();
        }

        // statement =
        //     id ":=" expression
        //     | "call" id
        //     | "begin" statement ";" {statement ";"} "end"
        //     | "if" condition "then" statement
        //     | "while" condition "do" statement
        void Statement()
        {
            Token token;
            if(Accept(TokenTypes.ID, out token))
            {
                Expect(TokenTypes.ASSIGN);

                AST.Node n = new AST.Node(AST.NodeType.Assign);
                n.text = token.Text;
                builder.Add(n);
                builder.Push(n);
                Expression();
                builder.Pop();
            }
            else if (Accept(TokenTypes.CALL))
            {
                Expect(TokenTypes.ID, out token);

                AST.Node n = new AST.Node(AST.NodeType.Call);
                n.text = token.Text;
                builder.Add(n);
            }
            else if (Accept(TokenTypes.BEGIN))
            {
                do
                {
                    Statement();
                } while (Accept(TokenTypes.SEMI));
                Expect(TokenTypes.END);
            }
            else if (Accept(TokenTypes.IF))
            {
                AST.Node n = new AST.Node(AST.NodeType.If);
                builder.Add(n);
                builder.Push(n);
                Condition();
                Expect(TokenTypes.THEN);
                Statement();
                builder.Pop();
            }
            else if (Accept(TokenTypes.WHILE))
            {
                AST.Node n = new AST.Node(AST.NodeType.While);
                builder.Add(n);
                builder.Push(n);
                Condition();
                Expect(TokenTypes.DO);
                Statement();
                builder.Pop();
            }
        }

        // condition =
        //     "odd" expression
        //     | expression ("="|"#"|"<"|"<="|">"|">=") expression
        void Condition()
        {
            if(Accept(TokenTypes.ODD))
            {
                AST.Node n = new AST.Node(AST.NodeType.UniOp);
                n.text = "odd";
                builder.Add(n);
                builder.Push(n);
                Expression();
                builder.Pop();
            }
            else
            {
                AST.Node n = new AST.Node(AST.NodeType.Comp);
                builder.Add(n);
                builder.Push(n);
                Expression();
                Token comp;
                Expect(TokenTypes.COMP, out comp);
                n.text = comp.Text;
                Expression();
                builder.Pop();
            }
        }

        // expression = ["+"|"-"] term {("+"|"-") term}
        void Expression()
        {
            Token token;
            Accept(TokenTypes.ADDSUB, out token);
            
            AST.Node n = new AST.Node(AST.NodeType.Term);
            builder.Add(n);
            builder.Push(n);

            Term();
            while (Accept(TokenTypes.ADDSUB, out token))
            {
                Term();
            }

            builder.Pop();
        }

        // term = factor {("*"|"/") factor}
        void Term()
        {
            AST.Node n = new AST.Node(AST.NodeType.Factor);
            builder.Add(n);
            builder.Push(n);
            Factor();
            Token token;
            while (Accept(TokenTypes.MULDIV, out token))
                Factor();
            builder.Pop();
        }

        // factor =
        //     id
        //     | number
        //     | "(" expression ")"
        void Factor()
        {
            Token token;
            if(Accept(TokenTypes.ID, out token))
            {
                AST.Node n = new AST.Node(AST.NodeType.Id);
                n.text = token.Text;
                builder.Add(n);
            }
            else if(Accept(TokenTypes.NUMBER, out token))
            {
                AST.Node n = new AST.Node(AST.NodeType.Number);
                n.number = float.Parse(token.Text);
                builder.Add(n);
            }
            else if(Accept(TokenTypes.LPAREN))
            {
                AST.Node n = new AST.Node(AST.NodeType.Expr);
                builder.Add(n);
                builder.Push(n);
                Expression();
                builder.Pop();
                Expect(TokenTypes.RPAREN);
            }
        }
    }
}
