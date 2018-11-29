namespace Markdown
{
    public class TagReader
    {
        public int Position { get; private set; }
        public string Source { get; }
        public ITagInfo[] Tags { get; }
        private TokenList TokenList { get; }
        private CollectionView<char> CollectionView { get; }


        public TagReader(string source, params ITagInfo[] tags)
        {
            Source = source;
            Tags = tags;
            TokenList = new TokenList(new PTagInfo().GetNewToken(Position));
            CollectionView = new CollectionView<char>(Source, Position);
        }

        public string Evaluate()
        {
            while (Position < Source.Length)
            {
                CollectionView.Position = Position;
                if (TrySkipEscapeSymbol()) continue;
                if (TryCloseTag()) continue;
                if (TryOpenTag()) continue;
                
                TokenList.AddCharacter(CollectionView.GetValue(0));

                Position++;
            }
            return TokenList.GetValue();
        }

        public void SkipAndAdd(int amount)
        {
            for (var startPosition = Position; Position < startPosition + amount; Position++)
               TokenList.AddCharacter(Source[Position]);
        }

        public void Skip(int amount)
        {
            Position += amount;
        }

        private bool TryOpenTag()
        {
            foreach (var tag in Tags)
            {
                if (!TokenList.TryOpenTag(tag, CollectionView))
                    continue;
                tag.OnTagStart(this);
                return true;
            }

            return false;
        }

        private bool TryCloseTag()
        {
            if (!TokenList.TryCloseTag(CollectionView, out var tag)) return false;
            tag.OnTagEnd(this);
            return true;
        }

        private bool TrySkipEscapeSymbol()
        {
            if (!CollectionView.IsEscapeSymbol(0)) return false;
            Skip(1);
            SkipAndAdd(1);
            return true;
        }
    }
}
