using System.Text;
using Markdown.Enums;
using Markdown.Exstensions;
using Markdown.Interfaces;
using Markdown.Interfacess;
using Markdown.Tokens;

namespace Markdown
{
    public class Tokenizer<T>
    where T : Enum
    {
        private readonly ITokenTyper<T> _tokenTyper;
        private readonly ITokenSetter<T> _tokenSeter;
        private readonly string line;
        public Tokenizer(string line, ITokenSetter<T> tokenSeter, ITokenTyper<T> tokenTyper)
        {
            this.line = line;
            _tokenSeter = tokenSeter;
            _tokenTyper = tokenTyper;
        }

        public List<Token> TokenizeLine()
        {
            var tokens = new List<Token>();
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                var current = line[i];
                var type = _tokenTyper.GetSymbolType(i);
                _tokenSeter.SetToken(tokens,type,ref i,line,stringBuilder);
            }
            _tokenSeter.CloseTags(tokens);
            _tokenSeter.DeleteEmptyTags(tokens);
            return tokens;
        }
    }
}

