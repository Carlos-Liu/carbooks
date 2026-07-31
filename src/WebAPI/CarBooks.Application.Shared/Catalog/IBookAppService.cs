using CarBooks.Application.Shared.Catalog.Dtos;

namespace CarBooks.Application.Shared.Catalog;

public interface IBookAppService : IApplicationService
{
    /// <summary>Returns a category together with its books.</summary>
    /// <exception cref="Domain.Shared.Errors.EntityNotFoundException">The id matches no category.</exception>
    Task<CategoryBooksDto> GetBooksByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken);
}
