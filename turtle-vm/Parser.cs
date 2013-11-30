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
            return Program();
        }
        
        // program = block "."
        AST.Node Program()
        {
            AST.Node prgm = new AST.Node(AST.NodeType.Program);
            prgm.Add(Block());

            Expect(TokenTypes.PERIOD);

            return prgm;
        }

        // block =
        //     ["const" id ":=" number {"," id ":=" number} ";"]
        //     ["var" id {"," id} ";"]
        //     {"procedure" id ";" block ";"} 
        //     statement
        AST.Node Block()
        {
            AST.Node block = new AST.Node(AST.NodeType.Block);

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
                    block.Add(n);

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
                    block.Add(n);

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
                n.Add(Block());
                block.Add(n);

                Expect(TokenTypes.SEMI);
            }

            block.Add(Statement());

            return block;
        }

        // statement =
        //     id ":=" expression
        //     | "call" id
        //     | "begin" statement ";" {statement ";"} "end"
        //     | "if" condition "then" statement
        //     | "while" condition "do" statement
        AST.Node Statement()
        {
            AST.Node stmt = null;

            Token token;
            if(Accept(TokenTypes.ID, out token))
            {
                Expect(TokenTypes.ASSIGN);

                stmt = new AST.Node(AST.NodeType.Assign);
                stmt.text = token.Text;
                stmt.Add(Expression());
            }
            else if (Accept(TokenTypes.CALL))
            {
                Expect(TokenTypes.ID, out token);

                stmt = new AST.Node(AST.NodeType.Call);
                stmt.text = token.Text;
            }
            else if (Accept(TokenTypes.BEGIN))
            {
                stmt = new AST.Node(AST.NodeType.Block);
                do
                {
                    stmt.Add(Statement());
                } while (Accept(TokenTypes.SEMI));
                Expect(TokenTypes.END);
            }
            else if (Accept(TokenTypes.IF))
            {
                stmt = new AST.Node(AST.NodeType.If);
                stmt.Add(Condition());
                Expect(TokenTypes.THEN);
                stmt.Add(Statement());
            }
            else if (Accept(TokenTypes.WHILE))
            {
                stmt = new AST.Node(AST.NodeType.While);
                stmt.Add(Condition());
                Expect(TokenTypes.DO);
                stmt.Add(Statement());
            }

            return stmt;
        }

        // condition =
        //     "odd" expression
        //     | expression ("="|"#"|"<"|"<="|">"|">=") expression
        AST.Node Condition()
        {
            AST.Node cond = null;

            if(Accept(TokenTypes.ODD))
            {
                cond = new AST.Node(AST.NodeType.UniOp);
                cond.text = "odd";
                cond.Add(Expression());
            }
            else
            {
                cond = new AST.Node(AST.NodeType.Comp);
                cond.Add(Expression());

                Token comp;
                Expect(TokenTypes.COMP, out comp);
                cond.text = comp.Text;

                cond.Add(Expression());
            }

            return cond;
        }

        // expression = ["+"|"-"] term {("+"|"-") term}
        AST.Node Expression()
        {
            AST.Node expr, t0, t1, op0, op1;

            Token token;
            Accept(TokenTypes.ADDSUB, out token);

            t0 = Term();
            expr = t0;
            op0 = null;

            while (Accept(TokenTypes.ADDSUB, out token))
            {
                t1 = Term();

                op1 = new AST.Node(AST.NodeType.BinOp);
                op1.text = token.Text;

                if (op0 != null)
                {
                    t0 = op0.children[1];
                    op0.children[1] = op1;
                }
                else
                {
                    expr = op1;
                }
                op1.Add(t0);
                op1.Add(t1);

                op0 = op1;
            }

            return expr;
        }

        // term = factor {("*"|"/") factor}
        AST.Node Term()
        {
            AST.Node term, f0, f1, op0, op1;
            Token token;

            f0 = Factor();
            term = f0;
            op0 = null;

            while (Accept(TokenTypes.MULDIV, out token))
            {
                f1 = Factor();

                op1 = new AST.Node(AST.NodeType.BinOp);
                op1.text = token.Text;

                if (op0 != null)
                {
                    f0 = op0.children[1];
                    op0.children[1] = op1;
                }
                else
                {
                    term = op1;
                }
                op1.Add(f0);
                op1.Add(f1);

                op0 = op1;
            }

            return term;
        }

        // factor =
        //     id
        //     | number
        //     | "(" expression ")"
        AST.Node Factor()
        {
            AST.Node factor = null;

            Token token;
            if(Accept(TokenTypes.ID, out token))
            {
                factor = new AST.Node(AST.NodeType.Id);
                factor.text = token.Text;
            }
            else if(Accept(TokenTypes.NUMBER, out token))
            {
                factor = new AST.Node(AST.NodeType.Number);
                factor.number = float.Parse(token.Text);
            }
            else if(Accept(TokenTypes.LPAREN))
            {
                factor = Expression();
                Expect(TokenTypes.RPAREN);
            }
            else
            {
                throw new Exception("Expected: id|number|(expr) Actual: " + tokenizer.Current);
            }

            return factor;
        }
    }
}
