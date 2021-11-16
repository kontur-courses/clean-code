using Markdown.Models;

namespace Markdown.Tokens
{
    public class ItalicPattern : ITokenPattern
    {
        private const string Symbol = "_";

        public bool IsStart(Context context)
        {
            if (!IsSymbolCorrect(context))
                return false;

            return context.HasNextSymbol() && char.IsLetter(context.GetNextSymbol());
        }

        public bool IsEnd(Context context)
        {
            if (!IsSymbolCorrect(context))
                return false;

            return context.HasPreviousSymbol() && char.IsLetter(context.GetPreviousSymbol());
        }

        private static bool IsSymbolCorrect(Context context) => context.Text[context.Index].ToString() == Symbol;
    }
}