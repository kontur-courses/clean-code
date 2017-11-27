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
		public void RepeatedMethodCallDoNotChangeBehaviour()
		{
			var board = new[]
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
			ChessProblem.LoadFrom(board);
			ChessProblem.CalculateChessStatus();
			Assert.AreEqual(ChessStatus.Check, ChessProblem.ChessStatus);
			
			// Now check that internal board modifictions during the first call do not change answer
			ChessProblem.CalculateChessStatus();
			Assert.AreEqual(ChessStatus.Check, ChessProblem.ChessStatus);
		}

		[Test]
		public void FullTests()
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