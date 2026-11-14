namespace Markdown.MDParsing
{
    public class OpenDelimiter
    {
        public int OutputIndex { get; }          
        public int SpaceTokensBefore { get; } 
        public bool IsDigitBefore { get; }
        public bool HasTextBefore { get; }
        
        public int DoubleTokensBefore { get; }

        public bool IsRejected { get; set; }

        public void Reject() => IsRejected = true;

        public OpenDelimiter(int outputIndex, int spaceTokensBefore,
            bool isDigitBefore, bool hasTextBefore,
            int doubleTokensBefore)
        {
            OutputIndex = outputIndex;
            SpaceTokensBefore = spaceTokensBefore;
            IsDigitBefore = isDigitBefore;
            HasTextBefore = hasTextBefore;
            DoubleTokensBefore = doubleTokensBefore;
        }
    }
}
