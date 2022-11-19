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
            var board = new BoardParser().ParseBoard(boardLines);
            var status1 = new ChessProblem(board).CalculateChessStatus();
            Assert.AreEqual(ChessStatus.Check, status1);

            // Now check that internal board modifications during the first call do not change answer
            var status2 = new ChessProblem(board).CalculateChessStatus();
            Assert.AreEqual(ChessStatus.Check, status2);
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
            var board = new BoardParser().ParseBoard(boardLines);
            var expectedAnswer = File.ReadAllText(Path.ChangeExtension(filename, ".ans")).Trim();
            var status = new ChessProblem(board).CalculateChessStatus();
            Assert.AreEqual(expectedAnswer, status.ToString().ToLower(), "Failed test " + filename);
        }
    }
}