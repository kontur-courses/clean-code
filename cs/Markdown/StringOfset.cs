
namespace Markdown
{
    internal struct StringOfset
    {
        internal readonly string text { get; }
        internal int ofset { get; }
        internal StringOfset(string text, int ofset) 
        {
            this.text = text;
            this.ofset = ofset;
        }
    }
}
