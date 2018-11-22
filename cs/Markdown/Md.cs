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
            return Parse(input, true);
        }

        private string Parse(string input, bool isGlobalTag)
        {
            StringBuilder result = new StringBuilder();
            var currentReaders = isGlobalTag ? globalReaders : localReaders;

            for (int i = 0; i < input.Length; i++)
            {
                var tokens = new List<Token>();

                foreach (var reader in currentReaders)
                {
                    var t = reader.tryGetToken(ref input, i);
                    if(t != null)
                        tokens.Add(t);
                }

                if (tokens.Count != 0)
                {
                    var token = tokens.OrderBy(t => t.Priority).First();

                    if (i != 0 && isGlobalTag)
                        result.Append("\n");

                    result.Append(token.OpenTag);
                    result.Append(Parse(token.Value, false));
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