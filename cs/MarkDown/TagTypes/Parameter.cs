namespace MarkDown.TagTypes
{
    public class Parameter
    {
        public string OpeningSymbol { get; }
        public string ClosingSymbol { get; }
        public string Name { get; }
        public Parameter(string openingSymbol, string closingSymbol, string name)
        {
            OpeningSymbol = openingSymbol;
            ClosingSymbol = closingSymbol;
            Name = name;
        }
    }
}