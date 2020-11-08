using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownParser : IParser
    {
        private TextInfo textInfo;
        private Stack<TagType> states;

        public MarkdownParser()
        {
            textInfo = new TextInfo();
        }

        public TextInfo ParseText(string text)
        {
            throw new System.NotImplementedException();
        }

        private void StateText(char symbol)
        {
            throw new System.NotImplementedException();
        }

        private void StateStrong(char symbol)
        {
            throw new System.NotImplementedException();
        }

        private void StateEm(char symbol)
        {
            throw new System.NotImplementedException();
        }

        private void StateHeading(char symbol)
        {
            throw new System.NotImplementedException();
        }
        
        private void StateInTag(char symbol)
        {
            throw new System.NotImplementedException();
        }
    }
}