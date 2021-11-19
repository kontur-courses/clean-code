using System;
namespace Markdown
{
    public class TagWord : IToken
    {
        public string Content
        { get; set; }
        public bool IsPrevent { get => false; set => throw new NotImplementedException(); }

        public TagWord(string word)
        {
            Content = word;
        }
    }
}
