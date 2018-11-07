using System;
using System.IO;
using NUnit.Framework;

namespace Chess
{
    [TestFixture]
    public class ChessProblem_Test
    {
        private static string[] _testBoardLines =
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
        
        [Test]
        public void RepeatedMethodCallDoNotChangeBehaviour()
        {
            var board = BoardParser.ParseBoardByLines(_testBoardLines);
            var chesProblem = new ChessProblem(board);
            
            Assert.AreEqual(ChessStatus.Check, chesProblem.CalculateChessStatus());

            // Now check that internal board modifications during the first call do not change answer
            Assert.AreEqual(ChessStatus.Check, chesProblem.CalculateChessStatus());
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
            var board = BoardParser.ParseBoardByLines(boardLines);
            var chesProblem = new ChessProblem(board);
            
            var expectedAnswer = File.ReadAllText(Path.ChangeExtension(filename, ".ans")).Trim();
            var resultStatus = chesProblem.CalculateChessStatus();
            Assert.AreEqual(expectedAnswer, resultStatus.ToString().ToLower(), "Failed test " + filename);
        }
    }
}