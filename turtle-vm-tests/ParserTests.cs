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
    public class ParserTests
    {
        AST.Node Parse(string input)
        {
            Tokenizer tokenizer = new Tokenizer(TokenTypes.ALL_TYPES, input);
            Parser parser = new Parser(tokenizer);
            return parser.Parse();
        }

        [TestMethod]
        public void TestParseConst()
        {
            AST.Node root = Parse("CONST x := 5.5; .");
            Assert.AreEqual(AST.NodeType.Program, root.type);
            Assert.AreEqual(AST.NodeType.Block, root.children[0].type);
            Assert.AreEqual(AST.NodeType.ConstDecl, root.children[0].children[0].type);

            AST.Node node = root.children[0].children[0];
            Assert.AreEqual("x", node.text);
            Assert.AreEqual(5.5f, node.number);
        }

        [TestMethod]
        public void TestParseVar()
        {
            AST.Node root = Parse("VAR x; .");
            Assert.AreEqual(AST.NodeType.Program, root.type);
            Assert.AreEqual(AST.NodeType.Block, root.children[0].type);
            Assert.AreEqual(AST.NodeType.VarDecl, root.children[0].children[0].type);
        }

        [TestMethod]
        public void TestParseProcedure()
        {
            AST.Node root = Parse("PROCEDURE test; x := 1; .");
            Assert.AreEqual(AST.NodeType.Program, root.type);
            Assert.AreEqual(AST.NodeType.Block, root.children[0].type);
            Assert.AreEqual(AST.NodeType.ProcDecl, root.children[0].children[0].type);
            Assert.AreEqual("test", root.children[0].children[0].text);
            Assert.AreEqual(AST.NodeType.Block, root.children[0].children[0].children[0].type);
        }

        [TestMethod]
        public void TestParseAssign()
        {
            AST.Node root = Parse("x := 1 .");
            Assert.AreEqual(AST.NodeType.Program, root.type);
            Assert.AreEqual(AST.NodeType.Block, root.children[0].type);
            Assert.AreEqual(AST.NodeType.Assign, root.children[0].children[0].type);
            Assert.AreEqual("x", root.children[0].children[0].text);
            Assert.AreEqual(AST.NodeType.Number, root.children[0].children[0].children[0].type);
            Assert.AreEqual(1.0f, root.children[0].children[0].children[0].number);
        }

        [TestMethod]
        public void TestParseCall()
        {
            AST.Node root = Parse("CALL test .");
            Assert.AreEqual(AST.NodeType.Program, root.type);
            Assert.AreEqual(AST.NodeType.Block, root.children[0].type);
            Assert.AreEqual(AST.NodeType.Call, root.children[0].children[0].type);
            Assert.AreEqual("test", root.children[0].children[0].text);
        }

        [TestMethod]
        public void TestParseBeginEnd()
        {
            AST.Node root = Parse("BEGIN END .");
            Assert.AreEqual(AST.NodeType.Program, root.type);
            Assert.AreEqual(AST.NodeType.Block, root.children[0].type);
            Assert.AreEqual(AST.NodeType.Block, root.children[0].children[0].type);
        }

        [TestMethod]
        public void TestParseIf()
        {
            AST.Node root = Parse("IF x<=10 THEN y:=1 .");
            Assert.AreEqual(AST.NodeType.Program, root.type);
            Assert.AreEqual(AST.NodeType.Block, root.children[0].type);
            Assert.AreEqual(AST.NodeType.If, root.children[0].children[0].type);
            Assert.AreEqual(AST.NodeType.Comp, root.children[0].children[0].children[0].type);
            Assert.AreEqual(AST.NodeType.Assign, root.children[0].children[0].children[1].type);
        }

        [TestMethod]
        public void TestParseWhile()
        {
            AST.Node root = Parse("WHILE x<=10 DO y:=1 .");
            Assert.AreEqual(AST.NodeType.Program, root.type);
            Assert.AreEqual(AST.NodeType.Block, root.children[0].type);
            Assert.AreEqual(AST.NodeType.While, root.children[0].children[0].type);
            Assert.AreEqual(AST.NodeType.Comp, root.children[0].children[0].children[0].type);
            Assert.AreEqual(AST.NodeType.Assign, root.children[0].children[0].children[1].type);
        }

        [TestMethod]
        public void TestParseProgramEmpty()
        {
            AST.Node root = Parse(".");
            Assert.AreEqual(AST.NodeType.Program, root.type);
        }
    }
}
