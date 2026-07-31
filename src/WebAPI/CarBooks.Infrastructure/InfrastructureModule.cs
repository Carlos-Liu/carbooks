using Autofac;
using CarBooks.Infrastructure.Media;

namespace CarBooks.Infrastructure;

/// <summary>
/// Registers the cross-cutting helpers owned by the infrastructure layer.
/// </summary>
public sealed class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DataUriFactory>()
            .As<IDataUriFactory>()
            .SingleInstance();
    }
}
