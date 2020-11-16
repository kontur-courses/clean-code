using System;
using System.Collections.Generic;

namespace Markdown.Infrastructure.Formatters
{
    public interface IWrapper
    {
        public Func<IEnumerable<string>, IEnumerable<string>> Wrap(string open, string close);
    }
}