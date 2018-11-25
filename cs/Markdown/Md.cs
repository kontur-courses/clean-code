using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private readonly List<IReadable> blockReaders;
        private readonly List<IReadable> inlineReaders;

        public Md()
        {
            blockReaders = new List<IReadable> {new ParagraphRegister(),
                                                new HorLineRegister()};
            inlineReaders = new List<IReadable> {new StrongRegister(),
                                                 new EmphasisRegister()};
        }

        public string Render(string input)
        {
            return Parse(input, false);
        }

        private Token TryGetToken(string strData, int startPosIndex, bool isInline)
        {
            Token token = null;
            var readers = isInline ? inlineReaders : blockReaders;

            foreach (var reader in readers)
            {
                var t = reader.TryGetToken(strData, startPosIndex);

                if (token == null || t != null && (t.shift > token.shift
                                                   || t.shift == token.shift && t.priority > token.priority))
                    token = t;
            }
            return token;
        }

        private string Parse(string input, bool isInline)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                var token = TryGetToken(input, i, isInline);
                if (token != null)
                {
                    result.Append(token.openTag);
                    result.Append(Parse(token.value, true));
                    result.Append(token.closeTag);

                    i += token.shift - 1;
                }
                else
                    result.Append(input[i]);
            }
            return result.ToString();
        }
    }
}