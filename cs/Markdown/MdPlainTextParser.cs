using System.Collections.Generic;

namespace Markdown
{
    public class MdPlainTextParser : IParser
    {

        public ParsingResult Parse(string mdText, int startBoundary, int endBoundary)
        {
            var text = mdText.Substring(startBoundary, endBoundary - startBoundary + 1);
            return ParsingResult.Ok(new HyperTextElement("PlainText", text), startBoundary, endBoundary);
        }
    }
}