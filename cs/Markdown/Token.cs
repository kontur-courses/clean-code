namespace Markdown
{
    public class Token
    {
        protected readonly int _begin;
        protected readonly int _end;
        protected int Length => _end - _begin + 1;

        public Token(int begin, int end)
        {
            _begin = begin;
            _end = end;
        }

        public virtual string Render(string str)
        {
            return str.Substring(_begin, Length);
        }
    }
}
