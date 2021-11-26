using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown
{
    public class ItalicToken : Token
    {
        public override string OpenedHtmlTag => "<em>";

        public override string ClosedHtmlTag => "</em>";

        public override int RawLengthOpen => 1;

        public override int RawLengthClose => 1;

        public override bool CanBeOpened(string text, int i)
        {
            return !TextHelper.CheckIfIthIsSpecificChar(text, i + 1, '_')
                && !TextHelper.CheckIfIthIsSpecificChar(text, i + 1, ' ')
                && !TextHelper.CheckIfIthIsSpecificChar(text, i - 1, '_')
                && !TextHelper.IsThreeGroundsInRow(text, i);
        }

        public override bool CanBeClosed(string text, int i)
        {
            return !(TextHelper.CheckIfIthIsSpecificChar(text, i - 1, ' ')
                  || TextHelper.CheckIfIthIsSpecificChar(text, i - 1, '_')
                  || TextHelper.CheckIfIthIsSpecificChar(text, i + 1, '_'));
        }

        public override ItalicToken CreateNewTokenOfSameType(int start)
        {
            return new ItalicToken(start);
        }

        public ItalicToken(int start) : base(start)
        {
        }

        public ItalicToken(int start, int length) : base(start, length)
        {
        }
    }
}
