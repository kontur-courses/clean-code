using System;

namespace Samples
{
    public class Tetris_TooLowLevel
    {
        public Tetris_TooLowLevel(int height, int width, bool[,] isFilled)
        {
            this.height = height;
            this.width = width;
            this.isFilled = isFilled;
        }

        private readonly int height;
        private readonly int width;
        private readonly bool[,] isFilled;

        public void ClearFullLines()
        {
            for (var y = 0; y < height; y++)
            {
                var count = 0;
                var fullY = 0;
                for (var x = 0; x < width; x++)
                    if (isFilled[x, y])
                    {
                        count++;
                        if (count == width) fullY = y;
                    }
                if (count == width)
                {
                    for (var yy = fullY; yy < height; yy++)
                        for (var x = 0; x < width; x++)
                            isFilled[x, yy] = isFilled[x, yy + 1];
                    for (var x = 0; x < width; x++)
                        isFilled[x, height] = false;
                }
            }
        }

        public void ClearFullLines_Refactored(ref int score)
        {
            for (var lineIndex = 1; lineIndex < height + 1; lineIndex++)
            {
                if (LineIsFull(lineIndex))
                {
                    score++;
                    ShiftLinesDown(lineIndex);
                    lineIndex--;
                    AddEmptyLineOnTop();
                }
            }
        }

        private void AddEmptyLineOnTop()
        {
            throw new NotImplementedException();
        }

        private void ShiftLinesDown(int lineIndex)
        {
            throw new NotImplementedException(lineIndex.ToString());
        }

        private bool LineIsFull(int y)
        {
            throw new NotImplementedException(y.ToString());
        }
    }
}