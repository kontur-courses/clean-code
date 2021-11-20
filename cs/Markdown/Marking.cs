namespace Markdown
{
    public class Marking
    {
        public char? SymbolBeforeMarking { get; set; }
        public string StringMarking { get; set; }
        public char? SymbolAfterMarking { get; set; }

        public Marking(char? symbolBeforeMarking, string stringMarking, char? symbolAfterMarking)
        {
            this.SymbolBeforeMarking = symbolBeforeMarking;
            this.StringMarking = stringMarking;
            this.SymbolAfterMarking = symbolAfterMarking;
        }
    }
}