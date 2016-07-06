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
            var board = new Board(File.ReadAllLines(filename));
            var expectedAnswer = File.ReadAllText(Path.ChangeExtension(filename, ".ans")).Trim();
            var whiteStatus = new ChessProblem(board).GetStatusFor(PieceColor.White);
            Assert.AreEqual(expectedAnswer, whiteStatus.ToString().ToLower(), "Failed test " + filename);
        }

        [Test]
        public void Test()
        {
            var testsCount = 0;
            foreach (var filename in Directory.GetFiles("ChessTests", "*.in"))
            {
                TestOnFile(filename);
                testsCount++;
            }
            Console.WriteLine("Tests passed: " + testsCount);
        }
    }
}