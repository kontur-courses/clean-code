namespace Markdown
{
    internal class HeaderToken : Token

    {
        public HeaderToken(int openIndex) : base(openIndex) { }

        public new static string Separator => "#";

        internal override void Handle(MdParser parser)
        {
            if (OpenIndex != 0 && parser.TextToParse[OpenIndex - 1] != '\n')
                return;

            var closeIndex = parser.TextToParse.IndexOf('\n');

            if (closeIndex == -1)
                closeIndex = parser.TextToParse.Length - 1;

            CloseIndex = closeIndex;
            parser.Result.Add(this);
        }
    }
}