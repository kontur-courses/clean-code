﻿using Markdown.Tags;

namespace Markdown.TagsMappers;

public interface ITagsMapper
{
    string Map(Tag tag);
}