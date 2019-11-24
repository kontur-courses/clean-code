using System.Text;
using Markdown.MdTags;

namespace Markdown.Content
{
    class ContentFinder : IContentFinder
    {
        public (int lenght, string content) GetBlockquoteContent(string text, int index)
            => GetDefaultContent(text, index);

        public (int lenght, string content) GetEmContent(string text, int index)
            => GetDefaultContent(text, index);

        private static void GreaterThanAndLessThanHandler(StringBuilder content, char symbol, ref int length)
        {
            content.Append(symbol == '<' ? "&lt" : "&gt");
            length -= 2;
        }

        public (int lenght, string content) GetListContent(string text, int index)
            => GetDefaultContent(text, index);

        public (int lenght, string content) GetSimpleContent(string text, int index)
            => GetDefaultContent(text, index);

        public (int lenght, string content) GetStrikeContent(string text, int index)
            => GetDefaultContent(text, index);

        public (int lenght, string content) GetHorizontalContent(string text, int index)
            => GetDefaultContent(text, index);

        public (int lenght, string content) GetStrongContent(string text, int index)
            => GetDefaultContent(text, index);

        public (int lenght, string content) GetCodeContent(string text, int index)
        {
            var length = 0;
            var content = new StringBuilder();
            for (var i = index; i < text.Length; i++)
            {
                if (text[i] == '<' || (text[i] == '>' && content.Length != 0))
                {
                    GreaterThanAndLessThanHandler(content, text[i], ref length);
                    continue;
                }
                if (text[i] == '`' || text[i] == '\r' && text[i + 1] == '\n')
                    break;
                content.Append(text[i]);
            }

            return (length + content.Length, content.ToString());
        }

        public (int lenght, string content) GetHeaderContent(string text, int index)
        {
            var length = 0;
            var content = new StringBuilder();
            for (var i = index; i < text.Length; i++)
            {
                if (text[i] == '<' || (text[i] == '>' && content.Length != 0))
                {
                    GreaterThanAndLessThanHandler(content, text[i], ref length);
                    continue;
                }
                if (text[i] == '\r' && text[i + 1] == '\n')
                    break;
                if (text[i] == '\\' && i != text.Length - 1)
                {
                    SlashHandler(ref i, ref length, content, text[i + 1]);
                    continue;
                }
                content.Append(text[i]);
            }

            return (length + content.Length, content.ToString());
        }

        public (int lenght, string content) GetDefaultContent(string text, int index)
        {
            var length = 0;
            var content = new StringBuilder();
            for (var i = index; i < text.Length; i++)
            {
                if (text[i] == '<' || (text[i] == '>' && content.Length != 0))
                {
                    GreaterThanAndLessThanHandler(content, text[i], ref length);
                    continue;
                }
                if (OtherTagFound(text, i))
                    break;
                if (text[i] == '\\' && i != text.Length - 1)
                {
                    SlashHandler(ref i, ref length, content, text[i + 1]);
                    continue;
                }
                content.Append(text[i]);
            }

            return (length + content.Length, content.ToString());
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
