namespace Markdown.Models;

public record TagPair
{
    public Type ConverterType { get; init; }
    public TagToken OpenToken { get; init; }
    public TagToken CloseToken { get; init; }
    
    public bool IsContains(TagPair otherTagPair)
    {
        ArgumentNullException.ThrowIfNull(otherTagPair);

        return OpenToken.TagIndex < otherTagPair.OpenToken.TagIndex
               && otherTagPair.OpenToken.TagIndex < CloseToken.TagIndex
               && OpenToken.TagIndex < otherTagPair.CloseToken.TagIndex
               && otherTagPair.CloseToken.TagIndex < CloseToken.TagIndex;
    }

    public static TagPairBuilder CreateBuilder(Type converterType)
    {
        var tagPairBuilder = new TagPairBuilder();
        tagPairBuilder.AddConverterType(converterType);
        return tagPairBuilder;
    }

    public class TagPairBuilder
    {
        private Type converterType;
        private TagToken openToken;
        private TagToken closeToken;

        public TagPairBuilder AddOpenToken(TagToken openToken)
        {
            this.openToken = openToken;
            return this;
        }

        public TagPairBuilder AddCloseToken(TagToken closeToken)
        {
            this.closeToken = closeToken;
            return this;
        }

        public TagPairBuilder AddConverterType(Type converterType)
        {
            this.converterType = converterType;
            return this;
        }

        public TagPair Build()
        {
            return new TagPair {OpenToken = openToken, CloseToken = closeToken, ConverterType = converterType};
        }
    }
}