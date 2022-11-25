namespace Markdown
{
    public class Tag
    {
        public static List<Func<Token, List<Token>, bool>> DefaultTagInteractionRules = new List<Func<Token, List<Token>, bool>>
        {
            
        };

        public string OpenMark;
        public string CloseMark;
        public string HtmlTag;
        public List<Func<Token, List<Token>, bool>> TagInteractionRules;
        public Func<Tag, Token, string, int, bool> OpenMarkRule;
        public Func<Tag, Token, string, int, bool> CloseMarkRule;
        public Func<Tag, Token, string, int, bool> TokenCloseRule;
        public Func<Tag, Token, string, int, bool> CharProcessingRule;
        public string[] CustomBoolNames;
        public Dictionary<string, Func<string, int, string>> PropertiesAndTheirReceiving;

        public Tag(string openMark, string closeMark, string htmlTag,
            string[] customBoolNames,
            Func<Tag, Token, string, int, bool> openMarkRule,
            Func<Tag, Token, string, int, bool> closeMarkRule,
            Func<Tag, Token, string, int, bool> tokenCloseRule,
            Func<Tag, Token, string, int, bool> charProcessingRule,
            List<Func<Token, List<Token>, bool>> tagInteractionRules,
            Dictionary<string, Func<string, int, string>> propertiesAndTheirReceiving = null!,
            Func<string, int, string> contentFinder = null!)
        {
            OpenMark = openMark;
            CloseMark = closeMark;
            HtmlTag = htmlTag;
            CustomBoolNames = customBoolNames;
            OpenMarkRule = openMarkRule;
            CloseMarkRule = closeMarkRule;
            TokenCloseRule = tokenCloseRule;
            CharProcessingRule = charProcessingRule;
            TagInteractionRules = Tag.DefaultTagInteractionRules.Concat(tagInteractionRules).ToList();
            PropertiesAndTheirReceiving = propertiesAndTheirReceiving;
        }

        public override bool Equals(object? obj)
        {
            return (obj as Tag)?.HtmlTag == HtmlTag;
        }
    }
}
