namespace MarkDown
{
    public class Character
    {
        public char Char { get; }
        public CharState CharState { get; }

        public Character(char character, CharState charState)
        {
            Char = character;
            CharState = charState;
        }
    }
}