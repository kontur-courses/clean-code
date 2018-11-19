using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private List<IReadable> globalReaders, localReaders;
        private Stack<Tuple<int, string>> closeBrackets;

        public Md()
        {
            globalReaders = new List<IReadable>();
            localReaders = new List<IReadable>();
            closeBrackets = new Stack<Tuple<int, string>>();
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
                var tokens = new SortedList<int, Token>();

                foreach (var reader in currentReaders)
                {
                    var t = reader.tryGetToken(ref input, i);
                    if(t != null)
                        tokens.Add(t.Priority, t);
                }

                if (tokens.Count != 0)
                {
                    if (i != 0)
                        result.Append(sep);

                    result.Append(tokens.ElementAt(0).Value.OpenBracket);
                    result.Append(Parse(tokens.ElementAt(0).Value.Value, false));   // если внутрь парсить не нужно, то оставляю value пустым
                    result.Append(tokens.ElementAt(0).Value.CloseBracket);

                    i += tokens.ElementAt(0).Value.TextValue.Length - 1;
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