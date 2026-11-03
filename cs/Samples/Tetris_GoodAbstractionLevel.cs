using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Samples
{
    internal class Field
    {
        private readonly ImmutableArray<ImmutableHashSet<int>> filledCellsLineByLine;
        public readonly int Height, Width;
        public readonly int Score;

        public Field(int width, int height, ImmutableArray<ImmutableHashSet<int>> filledCellsLineByLine, int score = 0)
        {
            Width = width;
            Height = height;
            Score = score;
            this.filledCellsLineByLine = filledCellsLineByLine;
        }

        public Field ClearFullLines()
        {
            var notFullLines = GetAllNotFullLines();
            var clearedLinesCount = Height - notFullLines.Count;
            var newLinesArray = CreateNewLinesArray(clearedLinesCount, notFullLines);
            return new Field(Width, Height, newLinesArray, Score + clearedLinesCount);
        }

        private ImmutableArray<ImmutableHashSet<int>> CreateNewLinesArray(int emptyLinesCount,
            List<ImmutableHashSet<int>> nonEmptyLines)
        {
            var emptyLines = Enumerable.Repeat(ImmutableHashSet.Create<int>(), emptyLinesCount);
            return nonEmptyLines.Concat(emptyLines).ToImmutableArray();
        }

        private List<ImmutableHashSet<int>> GetAllNotFullLines()
        {
            return filledCellsLineByLine.Where(line => line.Count != Width).ToList();
        }
    }
}