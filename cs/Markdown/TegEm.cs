namespace Markdown
{
    public class TegEm : Teg
    {
        public TegEm()
        {
            Rule = new Rule((t) => !(t is TegStrong));
        }
        public override string ToString()
        {
            return "em";
        }
    }
}
