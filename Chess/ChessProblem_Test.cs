using System;
using System.IO;
using NUnit.Framework;

namespace Chess
{
	[TestFixture]
	public class ChessProblem_Test
	{
		private static void TestOnFile(string filename)
		{
			var board = File.ReadAllLines(filename);
			ChessProblem.LoadFrom(board);
			var expectedAnswer = File.ReadAllText(Path.ChangeExtension(filename, ".ans")).Trim();
			ChessProblem.CalculateChessStatus();
			Assert.AreEqual(expectedAnswer, ChessProblem.ChessStatus.ToString().ToLower(), "Failed test " + filename);
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