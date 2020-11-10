namespace Markdown
{
    public class Word
    {
        public bool HasDigits;
        public int Length;


        public Word(int id, int position)
        {
            Id = id;
            Position = position;
        }

        public int Id { get; }
        public int Position { get; }
    }
}