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
            //AreEqual("program", 1, root);
            //AreEqual("declare-var", 1, root.children[0]);
            //AreEqual("id", 0, root.children[0].children[0]);
        }

        [TestMethod]
        public void TestParseProcedure()
        {
            AST.Node root = Parse("PROCEDURE test; x := 1; .");
            Assert.AreEqual(AST.NodeType.Program, root.type);
            //AreEqual("program", 1, root);
            //AreEqual("procedure", 2, root.children[0]);
            //AreEqual("id", root.children[0].children[0]);
            //AreEqual("body", root.children[0].children[1]);
        }

        [TestMethod]
        public void TestParseIf()
        {
            AST.Node root = Parse("IF x<=10 THEN y:=1 .");
            Assert.AreEqual(AST.NodeType.Program, root.type);
            //AreEqual("program", 1, root);
            //AreEqual("if", 2, root.children[0]);
            //AreEqual("condition", root.children[0].children[0]);
            //AreEqual("assign", root.children[0].children[1]);
        }

        [TestMethod]
        public void TestParseProgramEmpty()
        {
            AST.Node root = Parse(".");
            Assert.AreEqual(AST.NodeType.Program, root.type);
        }

        [TestMethod]
        public void TestParseProgramSample()
        {
            StringBuilder b = new StringBuilder();
            b.AppendLine("CONST");
            b.AppendLine("  m :=  7,");
            b.AppendLine("  n := 85;");
            b.AppendLine("VAR");
            b.AppendLine("  x, y, z, q, r;");
            b.AppendLine("PROCEDURE multiply;");
            b.AppendLine("VAR a, b;");
            b.AppendLine("BEGIN");
            b.AppendLine("  a := x;");
            b.AppendLine("  b := y;");
            b.AppendLine("  z := 0;");
            b.AppendLine("  WHILE b > 0 DO BEGIN");
            b.AppendLine("    IF ODD b THEN z := z + a;");
            b.AppendLine("    a := 2 * a;");
            b.AppendLine("    b := b / 2");
            b.AppendLine("  END");
            b.AppendLine("END;");
            b.AppendLine("PROCEDURE divide;");
            b.AppendLine("VAR w;");
            b.AppendLine("BEGIN");
            b.AppendLine("  r := x;");
            b.AppendLine("  q := 0;");
            b.AppendLine("  w := y;");
            b.AppendLine("  WHILE w <= r DO w := 2 * w;");
            b.AppendLine("  WHILE w > y DO BEGIN");
            b.AppendLine("    q := 2 * q;");
            b.AppendLine("    w := w / 2;");
            b.AppendLine("    IF w <= r THEN BEGIN");
            b.AppendLine("      r := r - w;");
            b.AppendLine("      q := q + 1");
            b.AppendLine("    END");
            b.AppendLine("  END");
            b.AppendLine("END;");
            b.AppendLine("PROCEDURE gcd;");
            b.AppendLine("VAR f, g;");
            b.AppendLine("BEGIN");
            b.AppendLine("  f := x;");
            b.AppendLine("  g := y;");
            b.AppendLine("  WHILE f # g DO BEGIN");
            b.AppendLine("    IF f < g THEN g := g - f;");
            b.AppendLine("    IF g < f THEN f := f - g");
            b.AppendLine("  END;");
            b.AppendLine("  z := f");
            b.AppendLine("END;");
            b.AppendLine("BEGIN");
            b.AppendLine("  x := m;");
            b.AppendLine("  y := n;");
            b.AppendLine("  CALL multiply;");
            b.AppendLine("  x := 25;");
            b.AppendLine("  y :=  3;");
            b.AppendLine("  CALL divide;");
            b.AppendLine("  x := 84;");
            b.AppendLine("  y := 36;");
            b.AppendLine("  CALL gcd");
            b.AppendLine("END.");

            AST.Node root = Parse(b.ToString());
            Assert.AreEqual(AST.NodeType.Program, root.type);
            return;
        }
    }
}
