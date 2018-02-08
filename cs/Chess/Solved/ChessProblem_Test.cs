using System;
using System.IO;
using NUnit.Framework;

namespace Chess.Solved
{
    [TestFixture]
    public class ChessProblem_Test
    {
        private static void TestOnFile(string filename)
        {
            var board = new BoardParser().ParseBoard(File.ReadAllLines(filename));
            var expectedAnswer = File.ReadAllText(Path.ChangeExtension(filename, ".ans")).Trim();
            var whiteStatus = new ChessProblem_Solved(board).GetStatusFor(PieceColor.White);
            Assert.AreEqual(expectedAnswer, whiteStatus.ToString().ToLower(), "Failed test " + filename);
        }

        [Test]
        public void Test()
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
    }
}