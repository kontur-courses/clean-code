namespace Markdown
{
    public class TegStrong : Teg
    {
        public TegStrong()
        {
            Rule = new Rule((t) => true);
        }
        public override string ToString()
        {
            return "strong";
        }
    }
}
