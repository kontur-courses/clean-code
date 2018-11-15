namespace Markdown
{
    public class TegEm : Teg
    {
        public TegEm() => TegRule = new TegRule((t) => !(t is TegStrong));
        public override string ToString() => "em";
    }
}
