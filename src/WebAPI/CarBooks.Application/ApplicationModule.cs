using System.Reflection;
using Autofac;
using CarBooks.Application.Shared;
using CarBooks.Domain.Catalog;
using Module = Autofac.Module;

namespace CarBooks.Application;

/// <summary>
/// Registers domain services and every application service in this assembly.
/// </summary>
public sealed class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CatalogManager>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .Where(type => typeof(IApplicationService).IsAssignableFrom(type) && type is { IsAbstract: false, IsClass: true })
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}
