using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private List<IReadable> blockReaders;
        private List<IReadable> inlineReaders;

        public Md()
        {
            blockReaders = new List<IReadable>();
            inlineReaders = new List<IReadable>();

            blockReaders.Add(new ParagraphRegister());
            blockReaders.Add(new HorLineRegister());
            
            inlineReaders.Add(new StrongRegister());
            inlineReaders.Add(new EmphasisRegister());
        }

        public string Render(string input)
        {
            return Parse(input, blockReaders);
        }

        private Token TryGetToken(ref string strData, int startPosIndex, List<IReadable> readers)
        {
            Token token = null;
            foreach (var reader in readers)
            {
                var t = reader.tryGetToken(ref strData, startPosIndex);
                if (token == null || t != null && (t.OriginalTextLen > token.OriginalTextLen
                                                   || t.OriginalTextLen == token.OriginalTextLen && t.Priority > token.Priority))
                    token = t;
            }
            return token;
        }

        public string Parse(string input, List<IReadable> readers)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                var token = TryGetToken(ref input, i, readers);
                if (token != null)
                {
                    result.Append(token.OpenTag);
                    result.Append(Parse(token.Value, inlineReaders));
                    result.Append(token.CloseTag);

                    i += token.OriginalTextLen - 1;
                }
                else
                    result.Append(input[i]);
            }
            return result.ToString();
        }
    }
}