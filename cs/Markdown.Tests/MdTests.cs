﻿using System;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MdTests
    {
        public void Render_ValidMarkdownText_ConvertToHTML()
        {
            Action render = () => new Md().Render("simple text");

            render.Should()
                  .Throw<NotImplementedException>();
        }
    }
}
