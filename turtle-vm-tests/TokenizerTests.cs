using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TurtleVM;
using System.Diagnostics;

namespace UnitTests
{
    [TestClass]
    public class TokenizerTests
    {
        List<Token> Tokenize(string input)
        {
            Tokenizer tokenizer = new Tokenizer(TokenTypes.ALL_TYPES, input);
            return tokenizer.Parse();
        }

        [TestMethod]
        public void TestTokenEmpty()
        {
            List<Token> tokens = Tokenize("");
            Assert.AreEqual(0, tokens.Count);
        }

        [TestMethod]
        public void TestTokenIfThen()
        {
            List<Token> tokens = Tokenize("IF THEN");
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(TokenTypes.IF, tokens[0].Type);
            Assert.AreEqual(TokenTypes.THEN, tokens[1].Type);
        }

        [TestMethod]
        public void TestTokenId()
        {
            List<Token> tokens = Tokenize("not_a_keyword");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenTypes.ID, tokens[0].Type);
        }

        [TestMethod]
        public void TestTokenWhileDo()
        {
            List<Token> tokens = Tokenize("WHILE DO");
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(TokenTypes.WHILE, tokens[0].Type);
            Assert.AreEqual(TokenTypes.DO, tokens[1].Type);
        }

        [TestMethod]
        public void TestTokenAddSubMulDiv()
        {
            List<Token> tokens = Tokenize("+ - * /");
            Assert.AreEqual(4, tokens.Count);
            Assert.AreEqual(TokenTypes.ADDSUB, tokens[0].Type);
            Assert.AreEqual(TokenTypes.ADDSUB, tokens[1].Type);
            Assert.AreEqual(TokenTypes.MULDIV, tokens[2].Type);
            Assert.AreEqual(TokenTypes.MULDIV, tokens[3].Type);
        }

        [TestMethod]
        public void TestTokenParens()
        {
            List<Token> tokens = Tokenize("()");
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(TokenTypes.LPAREN, tokens[0].Type);
            Assert.AreEqual(TokenTypes.RPAREN, tokens[1].Type);
        }

        [TestMethod]
        public void TestTokenBeginEnd()
        {
            List<Token> tokens = Tokenize("BEGIN END");
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(TokenTypes.BEGIN, tokens[0].Type);
            Assert.AreEqual(TokenTypes.END, tokens[1].Type);
        }

        [TestMethod]
        public void TestTokenNumber()
        {
            List<Token> tokens = Tokenize("1 -1 1.2 -0.4");
            Assert.AreEqual(4, tokens.Count);
            Assert.AreEqual(TokenTypes.NUMBER, tokens[0].Type);
            Assert.AreEqual(TokenTypes.NUMBER, tokens[1].Type);
            Assert.AreEqual(TokenTypes.NUMBER, tokens[2].Type);
            Assert.AreEqual(TokenTypes.NUMBER, tokens[3].Type);
        }
        
        [TestMethod]
        public void TestTokenComp()
        {
            List<Token> tokens = Tokenize("= # <= >= < >");
            Assert.AreEqual(6, tokens.Count);
            Assert.AreEqual(TokenTypes.COMP, tokens[0].Type);
            Assert.AreEqual(TokenTypes.COMP, tokens[1].Type);
            Assert.AreEqual(TokenTypes.COMP, tokens[2].Type);
            Assert.AreEqual(TokenTypes.COMP, tokens[3].Type);
            Assert.AreEqual(TokenTypes.COMP, tokens[4].Type);
            Assert.AreEqual(TokenTypes.COMP, tokens[5].Type);
        }

        [TestMethod]
        public void TestTokenAssign()
        {
            List<Token> tokens = Tokenize(":=");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenTypes.ASSIGN, tokens[0].Type);
        }

        [TestMethod]
        public void TestTokenConst()
        {
            List<Token> tokens = Tokenize("CONST");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenTypes.CONST, tokens[0].Type);
        }

        [TestMethod]
        public void TestTokenComma()
        {
            List<Token> tokens = Tokenize(",");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenTypes.COMMA, tokens[0].Type);
        }

        [TestMethod]
        public void TestTokenSemi()
        {
            List<Token> tokens = Tokenize(";");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenTypes.SEMI, tokens[0].Type);
        }

        [TestMethod]
        public void TestTokenPeriod()
        {
            List<Token> tokens = Tokenize(".");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenTypes.PERIOD, tokens[0].Type);
        }

        [TestMethod]
        public void TestTokenVar()
        {
            List<Token> tokens = Tokenize("VAR");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenTypes.VAR, tokens[0].Type);
        }

        [TestMethod]
        public void TestTokenProc()
        {
            List<Token> tokens = Tokenize("PROCEDURE");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenTypes.PROC, tokens[0].Type);
        }

        [TestMethod]
        public void TestTokenCall()
        {
            List<Token> tokens = Tokenize("CALL");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenTypes.CALL, tokens[0].Type);
        }

        [TestMethod]
        public void TestTokenOdd()
        {
            List<Token> tokens = Tokenize("ODD");
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenTypes.ODD, tokens[0].Type);
        }

    }
}
