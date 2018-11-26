using FluentAssertions.Equivalency;

namespace Markdown.Tests
{
    public static class FluentAssertionsOptionsExtensions
    {
        public static EquivalencyAssertionOptions<TDeclaring> ExcludeMember<TDeclaring>(
            this EquivalencyAssertionOptions<TDeclaring> options,
            string fieldName)
        {
            return options.Excluding(info => info.SelectedMemberInfo.Name.Equals(fieldName) &&
                                             info.SelectedMemberInfo.DeclaringType == typeof(TDeclaring));
        }
    }
}
