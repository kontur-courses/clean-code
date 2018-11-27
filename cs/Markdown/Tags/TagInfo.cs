using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public interface ITagInfo
    {
        Predicate<StringView> StartCondition { get; }
        Predicate<StringView> EndCondition { get; }
        Action<TagReader> OnTagStart { get; }
        Action<TagReader> OnTagEnd { get; }
        string HtmlTagText { get; }
        int TagLength { get; }
        Token GetNewToken(int position);
    }
}
