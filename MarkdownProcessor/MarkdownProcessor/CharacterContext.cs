namespace MarkdownProcessor
{
    public class CharacterContext
    {
        public CharacterContext(int position, string text, bool previousCharacterIsEscaping)
        {
            Position = position;
            Text = text;
            PreviousCharacterIsEscaping = previousCharacterIsEscaping;
        }

        public int Position { get; }
        public string Text { get; }
        public bool PreviousCharacterIsEscaping { get; }
    }
}