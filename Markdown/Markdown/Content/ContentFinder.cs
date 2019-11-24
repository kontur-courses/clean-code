using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.MdTags;

namespace Markdown.Content
{
    class ContentFinder : IContentFinder
    {
        private Dictionary<string, List<string>> allowableTags = new Dictionary<string, List<string>>
        {

        };

        public (int lenght, string content) GetBlockquoteContent()
        {
            throw new NotImplementedException();
        }

        public (int lenght, string content) GetCodeContent()
        {
            throw new NotImplementedException();
        }

        public (int lenght, string content) GetCodeContent(int index, string text)
        {
            var content = string.Join("", text.Take(index).TakeWhile(symbol => symbol != '`'));
            return (content.Length, content);
        }

        public (int lenght, string content) GetEmContent()
        {

            throw new NotImplementedException();
        }

        public (int lenght, string content) GetHeaderContent()
        {
            throw new NotImplementedException();
        }

        public (int lenght, string content) GetHorizontalContent()
        {
            throw new NotImplementedException();
        }

        public (int lenght, string content) GetListContent()
        {
            throw new NotImplementedException();
        }

        public (int lenght, string content) GetSimpleContent()
        {
            throw new NotImplementedException();
        }

        public (int lenght, string content) GetStrikeContent()
        {
            throw new NotImplementedException();
        }

        public (int lenght, string content) GetStrongContent()
        {
            throw new NotImplementedException();
        }

        private static bool OtherTagFound(string text, int i)
            => Tag.AllTags.Contains(text[i].ToString());

        private static void SlashHandler(ref int i, ref int length, StringBuilder content, char symbolToAdd)
        {
            content.Append(symbolToAdd);
            i++;
            length++;
        }
    }
}
