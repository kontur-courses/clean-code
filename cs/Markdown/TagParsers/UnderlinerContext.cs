using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.TagParsers
{
    public enum UnderlinerContext
    {
        // before first searching
        None,
        // " _word"
        WordBeginnig,
        // "_word"
        LineBeginnig,
        // "word_ "
        // "word_\n"
        WordEnding,
        // "wo_rd"
        WordMiddle,
        BetweenDigits,

    }
}
