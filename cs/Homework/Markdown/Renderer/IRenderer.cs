﻿using Markdown.Tokens;

namespace Markdown.Renderer
{
    public interface IRenderer
    {
        public string Render(MarkdownToken[] tokens);
    }
}