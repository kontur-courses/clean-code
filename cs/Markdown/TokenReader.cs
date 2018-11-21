using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Markdown
{
    public class TokenReader
    {
        private static char[] whiteSpaceSymbols =  {' ','\n','\0'};
        private IState zeroState;
        private IState whiteSpaceState;
        private IState trashCollectorState;
        private Dictionary<IState, Tag> terminateStates;
        
        
        public TokenReader(Dictionary<string,Tag> tags)
        {    
            terminateStates = new Dictionary<IState, Tag>();
            zeroState = new State();
            var noTrashSymbols = new List<char>();
            
            foreach (var token in tags)
            {
                var current = zeroState;
                foreach (var ch in token.Key)
                {
                    if(current == zeroState)
                        noTrashSymbols.Add(ch);
                    if (!current.ContainsKey(ch))
                        current.Add(ch,new State());
                    current = current[ch];
                }
                terminateStates.Add(current,token.Value);
            }
            
            whiteSpaceState = new State();
            foreach (var whiteSpaceSymbol in whiteSpaceSymbols)
            {
                noTrashSymbols.Add(whiteSpaceSymbol);
                whiteSpaceState.Add(whiteSpaceSymbol,whiteSpaceState);
                zeroState.Add(whiteSpaceSymbol,whiteSpaceState);
            }  
            noTrashSymbols.Add('\\');
            zeroState.Add('\\',new SlashEcranationState());
           

            trashCollectorState = new TrashCollectorState(noTrashSymbols);
        }

        public LinkedList<Token> ReadTokens(string source)
        {
            var retList = new LinkedList<Token>();
            var current = zeroState;
            var sb = new StringBuilder();
            var token = new Token();
            using (var enumerator = new TwoWayEnumerator<char>(source.GetEnumerator()))
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.IsNumber())
                        token.HasNumber = true;
                        
                    if (current.ContainsKey(enumerator.Current))
                    {
                        if(enumerator.Current != '\\')
                            sb.Append(enumerator.Current);
                        current = current[enumerator.Current];
                    }
                    else
                    {
                        if (current == zeroState)
                        {
                            current = trashCollectorState;
                            enumerator.MovePrevious();
                        }
                        else
                        {
                            token.Value = sb.ToString();
                            if (terminateStates.ContainsKey(current))
                                token.PosibleTag = terminateStates[current];
                            if (current == whiteSpaceState)
                                token.IsWhiteSpace = true;
                            retList.AddLast(token);
                            
                            token = new Token();
                            enumerator.MovePrevious();
                            current = zeroState;
                            sb.Clear();
                        }
                    }
                }

                if (sb.Length != 0)
                {
                    token.Value = sb.ToString();
                    if (terminateStates.ContainsKey(current))
                        token.PosibleTag = terminateStates[current];
                    if (current == whiteSpaceState)
                        token.IsWhiteSpace = true;
                    retList.AddLast(token);
                }
            }
            return retList;
        }
    }


    public interface IState
    {
        bool ContainsKey(char ch);
        IState this[char ch] { get;}
        void Add(char ch, IState state);
    }
    
    
    public class State : Dictionary<char, IState> , IState
    {
        public bool ContainsKey(char ch)
        {
            return base.ContainsKey(ch);
        }

        public IState this[char ch]
        {
            get { return base[ch]; }
        }
    }

    public class SlashEcranationState : IState
    {
        public IState this[char c]
        {
            get {return new State();}
        }

        public bool ContainsKey(char c)
        {
            return true;
        }

        public void Add(char ch, IState state)
        {
            
        }
    }

    public class TrashCollectorState : IState
    {
        private HashSet<char> NoTrashTokens;

        public TrashCollectorState(IEnumerable<char> noTrash)
        {
            NoTrashTokens = new HashSet<char>();
            foreach (var token in noTrash)
                NoTrashTokens.Add(token);
        }

        public bool ContainsKey(char ch)
        {
            return !NoTrashTokens.Contains(ch);
        }

        public IState this[char ch]
        {
            get { return this; }
        }

        public void Add(char ch, IState state)
        {
            
        }
    }


    public static class CharExtensions
    {
        public static bool IsNumber(this char c)
        {
            return c >= '0' && c <= '9';
        }
    }
}