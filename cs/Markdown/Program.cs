using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token
    {
        public readonly string Name;
        public readonly string TextValue, Value;
        public readonly string OpenBracket, CloseBracket;
        public readonly int Priority;

        public Token(string name, string textValue, string value, string openBracket, int priority, string closeBracket = null)
        {
            Name = name;
            TextValue = textValue;
            Value = value;

            OpenBracket = openBracket;
            CloseBracket = closeBracket;
            Priority = priority;
        }
    }

    public interface IReadable
    {
        Token tryGetToken(ref string input, int startPos);
    }

    public abstract class BaseRegister : IReadable
    {
        public abstract Token tryGetToken(ref string input, int startPos);
    }

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

        public string Parse(string input, bool isGlobalTag)
        {
            var strList = new List<string>();
            int indexOfJustString = 0;

            for (int i = 0; i < input.Length; i++)
            {
                var tokens = new SortedList<int, Token>();
                var currentReaders = isGlobalTag ? globalReaders : localReaders;

                foreach (var reader in currentReaders)
                {
                    var t = reader.tryGetToken(ref input, i);
                    tokens.Add(t.Priority, t);
                }

                if (tokens.Count != 0)
                {
                    if(indexOfJustString != i - 1)
                        strList.Add(input.Substring(indexOfJustString, i));

                    strList.Add(tokens[0].OpenBracket + Parse(tokens[0].Value, false) + tokens[0].CloseBracket);

                    i += tokens[0].TextValue.Length - 1;
                    indexOfJustString = i;
                }
            }
            if (indexOfJustString != input.Length - 1)
                strList.Add(input.Substring(indexOfJustString, input.Length));
            return string.Join((isGlobalTag ? "\n" : ""), strList);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            // TODO Обернуть в try case
            Console.WriteLine("Hello");
        }
    }
}
