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
    public class InterpreterTests
    {
        void Interpret(Interpreter interpreter, string input)
        {
            Tokenizer tokenizer = new Tokenizer(TokenTypes.ALL_TYPES, input);
            Parser parser = new Parser(tokenizer);
            interpreter.Evaluate(parser.Parse());
        }

        [TestMethod]
        public void TestInterpretConst()
        {
            string input = "CONST x := 5.5; .";

            Interpreter interpreter = new Interpreter();
            Assert.IsFalse(interpreter.Symbols.Exists("x"));

            Interpret(interpreter, input);

            Assert.IsTrue(interpreter.Symbols.Exists("x"));
            Assert.AreEqual(5.5f, interpreter.Symbols["x"].number);
        }

        [TestMethod]
        public void TestInterpretConstMulti()
        {
            string input = "CONST x := 5.5, y := 1; .";

            Interpreter interpreter = new Interpreter();
            Assert.IsFalse(interpreter.Symbols.Exists("x"));
            Assert.IsFalse(interpreter.Symbols.Exists("y"));

            Interpret(interpreter, input);

            Assert.IsTrue(interpreter.Symbols.Exists("x"));
            Assert.AreEqual(5.5f, interpreter.Symbols["x"].number);
            Assert.IsTrue(interpreter.Symbols.Exists("y"));
            Assert.AreEqual(1f, interpreter.Symbols["y"].number);
        }

        [TestMethod]
        public void TestInterpretVar()
        {
            string input = "VAR x; .";

            Interpreter interpreter = new Interpreter();
            Assert.IsFalse(interpreter.Symbols.Exists("x"));

            Interpret(interpreter, input);

            Assert.IsTrue(interpreter.Symbols.Exists("x"));
        }

        [TestMethod]
        public void TestInterpretVarMulti()
        {
            string input = "VAR x, y; .";

            Interpreter interpreter = new Interpreter();
            Assert.IsFalse(interpreter.Symbols.Exists("x"));
            Assert.IsFalse(interpreter.Symbols.Exists("y"));

            Interpret(interpreter, input);

            Assert.IsTrue(interpreter.Symbols.Exists("x"));
            Assert.IsTrue(interpreter.Symbols.Exists("y"));
        }

        [TestMethod]
        public void TestInterpretProc()
        {
            string input = "PROCEDURE x;; .";

            Interpreter interpreter = new Interpreter();
            Assert.IsFalse(interpreter.Symbols.Exists("x"));

            Interpret(interpreter, input);

            Assert.IsTrue(interpreter.Symbols.Exists("x"));
        }

        [TestMethod]
        public void TestInterpretProcMulti()
        {
            string input = "PROCEDURE x;; PROCEDURE y;; .";

            Interpreter interpreter = new Interpreter();
            Assert.IsFalse(interpreter.Symbols.Exists("x"));
            Assert.IsFalse(interpreter.Symbols.Exists("y"));

            Interpret(interpreter, input);

            Assert.IsTrue(interpreter.Symbols.Exists("x"));
            Assert.IsTrue(interpreter.Symbols.Exists("y"));
        }

        [TestMethod]
        public void TestInterpretCall()
        {
            string input = "VAR x; PROCEDURE foo; x := 1; CALL foo .";

            Interpreter interpreter = new Interpreter();
            Assert.IsFalse(interpreter.Symbols.Exists("x"));
            Assert.IsFalse(interpreter.Symbols.Exists("foo"));

            Interpret(interpreter, input);

            Assert.IsTrue(interpreter.Symbols.Exists("x"));
            Assert.IsTrue(interpreter.Symbols.Exists("foo"));

            Assert.AreEqual(1.0f, interpreter.Symbols["x"].number);
        }

        [TestMethod]
        public void TestInterpretIf()
        {
            string input = "VAR x; IF 2 > 1 THEN x := 1 .";

            Interpreter interpreter = new Interpreter();
            Assert.IsFalse(interpreter.Symbols.Exists("x"));

            Interpret(interpreter, input);

            Assert.IsTrue(interpreter.Symbols.Exists("x"));

            Assert.AreEqual(1.0f, interpreter.Symbols["x"].number);
        }

        [TestMethod]
        public void TestInterpretWhile()
        {
            string input = "VAR x; WHILE x < 10 DO x := x + 1 .";

            Interpreter interpreter = new Interpreter();
            Assert.IsFalse(interpreter.Symbols.Exists("x"));

            Interpret(interpreter, input);

            Assert.IsTrue(interpreter.Symbols.Exists("x"));

            Assert.AreEqual(10.0f, interpreter.Symbols["x"].number);
        }

        [TestMethod]
        public void TestInterpretExpr()
        {
            string input = "VAR x, y; BEGIN x := 1; y := 2; x := ( x + 1 ) * ( y + 4 ) / ( 5 - 1 ) END .";

            Interpreter interpreter = new Interpreter();
            Assert.IsFalse(interpreter.Symbols.Exists("x"));
            Assert.IsFalse(interpreter.Symbols.Exists("y"));

            Interpret(interpreter, input);

            Assert.IsTrue(interpreter.Symbols.Exists("x"));
            Assert.IsTrue(interpreter.Symbols.Exists("y"));

            Assert.AreEqual(3.0f, interpreter.Symbols["x"].number);
            Assert.AreEqual(2.0f, interpreter.Symbols["y"].number);
        }

        [TestMethod]
        public void TestInterpretCallScope()
        {
            string input = "VAR x; PROCEDURE foo; VAR x; x := 100; BEGIN x := 2; CALL foo END.";

            Interpreter interpreter = new Interpreter();
            Assert.IsFalse(interpreter.Symbols.Exists("x"));
            Assert.IsFalse(interpreter.Symbols.Exists("foo"));

            Interpret(interpreter, input);

            Assert.IsTrue(interpreter.Symbols.Exists("x"));
            Assert.IsTrue(interpreter.Symbols.Exists("foo"));

            Assert.AreEqual(2.0f, interpreter.Symbols["x"].number);
        }
    }
}
