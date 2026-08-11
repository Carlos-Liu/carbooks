using Autofac;
using CarBooks.Domain.Repositories;
using CarBooks.Repository.Catalog;

namespace CarBooks.Repository;

/// <summary>
/// Binds the domain repository abstractions to their EF Core implementations. Lifetime matches the
/// request-scoped <see cref="Database.Ef.CarBooksDbContext"/> they depend on.
/// </summary>
public sealed class RepositoryModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EfCategoryRepository>()
            .As<ICategoryRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<EfBookRepository>()
            .As<IBookRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<EfCategoryBooksRepository>()
            .As<ICategoryBooksRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<EfTagRepository>()
            .As<ITagRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<EfBookTagsRepository>()
            .As<IBookTagsRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<EfUnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();
    }
}
