namespace Markdown
{
    public class MarkupElement
    {

            public readonly string Opening;
            public readonly string Closing;

            public MarkupElement(string opening, string closing)
            {
                Opening = opening;
                Closing = closing;
            }
            
            public MarkupElement(string mark) : this(mark,mark){}

    }
}