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
            localReaders = new List<IReadable>();
        }

        public void registerGlobalReader(IReadable reader)
        {
            globalReaders.Add(reader);
        }

        public void registerLocalReader(IReadable reader)
        {
            localReaders.Add(reader);
        }

        public string Parse(string input, bool isGlobalTag=true)        // TODO Убрать возможность извне менять параметр глобальности
        {
            StringBuilder result = new StringBuilder();
            string sep = isGlobalTag ? "\n" : "";
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

                    if (i != 0)
                        result.Append(sep);

                    result.Append(token.OpenBracket);
                    result.Append(Parse(token.Value, false));   // если внутрь парсить не нужно, то оставляю value пустым (при создании токена)
                    result.Append(token.CloseBracket);

                    i += tokens[0].TextValue.Length - 1;
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