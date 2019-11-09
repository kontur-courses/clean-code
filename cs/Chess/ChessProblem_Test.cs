using System;
using System.IO;
using NUnit.Framework;

namespace Chess
{
    [TestFixture]
    public class ChessProblem_Test
    {
        [Test]
        public void RepeatedMethodCallDoNotChangeBehaviour()
        {
            var boardLines = new[]
            {
                "        ",
                "        ",
                "        ",
                "   q    ",
                "    K   ",
                " Q      ",
                "        ",
                "        ",
            };
            
            var chess = new ChessProblem(Board.LoadFrom(boardLines));
            Assert.AreEqual(ChessStatus.Check, chess.GetChessStatus());
        }

        [Test]
        public void AllTests()
        {
            var dir = TestContext.CurrentContext.TestDirectory;
            var testsCount = 0;
            foreach (var filename in Directory.GetFiles(Path.Combine(dir, "ChessTests"), "*.in"))
            {
                TestOnFile(filename);
                testsCount++;
            }
            Console.WriteLine("Tests passed: " + testsCount);
        }

        private static void TestOnFile(string filename)
        {
            var boardLines = File.ReadAllLines(filename);
            var chess = new ChessProblem(Board.LoadFrom(boardLines));
            var expectedAnswer = File.ReadAllText(Path.ChangeExtension(filename, ".ans")).Trim();
            Assert.AreEqual(expectedAnswer, chess.GetChessStatus().ToString().ToLower(), "Failed test " + filename);
        }
    }
}