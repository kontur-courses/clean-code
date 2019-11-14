using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Markdown
{
    class TagInserter
    {
        public TagInserter()
        {

        }

        public string Insert(string text, Dictionary<int, Tag> toInsert)
        {
            StringBuilder outText = new StringBuilder();
            var pairs = toInsert.ToList().OrderBy(a=>a.Key);
            int currentIndex = 0;
            foreach (var pair in pairs)
            {
                outText.Append(text.Substring(currentIndex, pair.Key - currentIndex));
                currentIndex = pair.Key;
                switch (pair.Value)
                {
                    case Tag.Em:
                        currentIndex++;
                        outText.Append("<em>");
                        break;
                    case Tag.Em_close:
                        currentIndex++;
                        outText.Append("</em>");
                        break;
                    case Tag.Strong:
                        currentIndex += 2;
                        outText.Append("<strong>");
                        break;
                    case Tag.Strong_close:
                        currentIndex += 2;
                        outText.Append("</strong>");
                        break;
                }
            }
            outText.Append(text.Substring(currentIndex));
            return outText.ToString();
        }
    }
}
