using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer;

public class Md
{
    private static readonly IDocumentsConverter Converter;

    static Md()
    {
        Converter = GetConverter();
    }

    public string Render(string mdSource) =>
        Converter.Convert(mdSource);

    private static IDocumentsConverter GetConverter()
    {
        using var container = new WindsorContainer();

        var kernel = container.Kernel;
        kernel.Resolver.AddSubResolver(new CollectionResolver(kernel, true));

        container.Register(
            Classes.FromAssemblyContaining<IDocumentsConverter>()
                .Where(component =>
                    component.Namespace is not null && component.Namespace.Contains(nameof(Implementations)))
                .WithService.AllInterfaces()
                .LifestyleSingleton()
        );

        var converter = container.Resolve<IDocumentsConverter>();
        return converter;
    }
}