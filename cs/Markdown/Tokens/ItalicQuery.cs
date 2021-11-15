using System;
using Markdown.Models;

namespace Markdown.Tokens
{
    public class ItalicQuery : ITokenQuery
    {
        public bool IsStart(Context context) => throw new NotImplementedException();
        public bool IsEnd(Context context) => throw new NotImplementedException();
    }
}