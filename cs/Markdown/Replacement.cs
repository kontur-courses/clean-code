namespace Markdown
{
    class Replacement
    {
        public readonly Substring OldValueSubstring;
        public readonly string NewValue;

        public Replacement(string newValue, Substring oldValueSubstring)
        {
            OldValueSubstring = oldValueSubstring;
            NewValue = newValue;
        }
    }
}
