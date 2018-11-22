using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private List<IReadable> globalReaders;
        private List<IReadable> localReaders;

        public Md()
        {
            globalReaders = new List<IReadable>();
            globalReaders.Add(new ParagraphRegister());
            globalReaders.Add(new HorLineRegister());
            
            localReaders = new List<IReadable>();
            localReaders.Add(new StrongRegister());
            localReaders.Add(new EmRegister());
        }

        public string Render(string input)
        {
            return Parse(input, globalReaders);
        }

        public string Parse(string input, List<IReadable> readers)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                Token token = null;

                foreach (var reader in readers)
                {
                    var t = reader.tryGetToken(ref input, i);
                    if (token == null || t != null && t.Priority > token.Priority)
                        token = t;
                }

                if (token != null)
                {
                    result.Append(token.OpenTag);
                    result.Append(Parse(token.Value, localReaders));
                    result.Append(token.CloseTag);

                    i += token.OriginalTextLen - 1;
                }
                else
                {
                    result.Append(input[i]);
                }
            }
            return result.ToString();
        }
    }
}