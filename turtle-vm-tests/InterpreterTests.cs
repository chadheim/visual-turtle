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

    }
}
