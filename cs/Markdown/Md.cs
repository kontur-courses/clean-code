using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class Md
    {
        public string Render(string paragraph)
        {
            var charProcessor = new CharProcessor();
            var tagInserter = new TagInserter();
            var letters = paragraph.ToCharArray();
            foreach (var letter in letters)
                charProcessor.ProcessChar(letter);
            charProcessor.ProcessChar(default);
            var inserts = charProcessor.GetInserts();
            var HTMLtext = tagInserter.Insert(paragraph, inserts);
            return HTMLtext;
        }

    }

    enum Tag
    {
        Em,
        Em_close,
        Strong,
        Strong_close,
        Empty

    }

}
