using Markdown.Styles;

namespace Markdown
{
    internal class HTMLConverter : ITextConverter
    {
        public string Convert(Style styledTokens)
        {
            string result = string.Empty;
            styledTokens.ChildTokens.ForEach(t => result += t.ToText());
            return result;
            //    ;
            //var htmlTag = GetHtmlTag(token);
            //string result = $"<{htmlTag}>";
            //if (token.Children.Count > 0)
            //{
            //    foreach (var child in token.Children)
            //        result += Convert(child, ref source);
            //}
            //else
            //    result += source.Substring(token.BeginIndex, token.EndIndex - token.BeginIndex);
            //result += $"<{htmlTag}/>";

            //return result;
        }

        //private string GetHtmlTag(Token token)
        //{
        //    switch (token.GetType().Name)
        //    {
        //        case nameof(ItalicStyle):
        //            return "em";
        //        default:
        //            return string.Empty;
        //    }
        //}
    }
}
