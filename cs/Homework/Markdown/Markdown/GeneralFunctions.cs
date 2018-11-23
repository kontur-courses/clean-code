namespace Markdown
{
    public static class GeneralFunctions
    {
        public static bool WasOddCountEscaping(string line, int position)
        {
            var escapesCount = 0;
            for (var i = position - 1; i >= 0; i--)
            {
                if (line[i] != '\\')
                    break;
                escapesCount++;
            }

            return escapesCount % 2 == 1;
        }

        public static int SymbolInRowCount(char symbol, string line, int position)
        {
            var repeatCount = 1;
            position++;
            while (position < line.Length && line[position] == symbol)
            {
                repeatCount++;
                position++;
            }

            return repeatCount;
        }
    }
}
