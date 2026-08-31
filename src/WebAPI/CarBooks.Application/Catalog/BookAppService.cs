using CarBooks.Application.Catalog.Mapping;
using CarBooks.Application.Shared.Catalog;
using CarBooks.Application.Shared.Catalog.Dtos;
using CarBooks.Domain.Catalog;
using CarBooks.Domain.Repositories;
using CarBooks.Domain.Shared;
using CarBooks.Domain.Shared.Errors;
using CarBooks.Infrastructure.Media;
using Microsoft.Extensions.Logging;

namespace CarBooks.Application.Catalog;

internal sealed class BookAppService : IBookAppService
{
    private static readonly HashSet<string> AllowedCoverImageContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/gif",
        "image/webp",
    };

    private readonly CatalogManager catalogManager;
    private readonly BookManager bookManager;
    private readonly IBookTagsRepository bookTagsRepository;
    private readonly IDataUriFactory dataUriFactory;
    private readonly ILogger<BookAppService> logger;

    public BookAppService(
        CatalogManager catalogManager,
        BookManager bookManager,
        IBookTagsRepository bookTagsRepository,
        IDataUriFactory dataUriFactory,
        ILogger<BookAppService> logger)
    {
        this.catalogManager = catalogManager;
        this.bookManager = bookManager;
        this.bookTagsRepository = bookTagsRepository;
        this.dataUriFactory = dataUriFactory;
        this.logger = logger;
    }

    public async Task<CategoryBooksDto> GetBooksByCategoryIdAsync(
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var result = await catalogManager.GetCategoryBooksAsync(categoryId, cancellationToken);
        var tagsByBookId = await bookTagsRepository.ListTagsByBookIdsAsync(
            result.Books.Select(book => book.Id),
            cancellationToken);

        logger.LogInformation(
            "Returning {BookCount} books for category {CategoryId}.",
            result.Books.Count,
            result.Category.Id);

        return new CategoryBooksDto(
            result.Category.ToDto(result.Books.Count),
            result.Books.ToDtos(dataUriFactory, tagsByBookId));
    }

    public async Task<BookDto> CreateBookAsync(
        CreateBookDto request,
        CoverImageDto? coverImage,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var (coverImageContent, contentType) = await ReadCoverImageAsync(coverImage, cancellationToken);

        var book = await bookManager.AddBookAsync(
            request.Name,
            request.Author,
            request.CoverUrl,
            request.Translator,
            request.Publisher,
            request.PublishedOn,
            request.Recommendation,
            request.Isbn,
            coverImageContent,
            contentType,
            request.CategoryIds,
            request.TagIds,
            cancellationToken);

        var tagsByBookId = await bookTagsRepository.ListTagsByBookIdsAsync([book.Id], cancellationToken);
        var tags = tagsByBookId.GetValueOrDefault(book.Id);

        logger.LogInformation("Created book {BookId} ({BookName}).", book.Id, book.Name);

        return book.ToDto(dataUriFactory, tags);
    }

    private static Task<(byte[]? Content, string? ContentType)> ReadCoverImageAsync(
        CoverImageDto? coverImage,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (coverImage is null || coverImage.Content.Length == 0)
        {
            return Task.FromResult<(byte[]? Content, string? ContentType)>((null, null));
        }

        if (coverImage.Content.Length > Consts.MaxCoverImageBytes)
        {
            throw new DomainValidationException($"Cover image must be {Consts.MaxCoverImageBytes} bytes or fewer.");
        }

        var contentType = coverImage.ContentType?.Trim() ?? string.Empty;
        if (!AllowedCoverImageContentTypes.Contains(contentType))
        {
            throw new DomainValidationException(
                "Cover image must be a JPEG, PNG, GIF or WebP file.");
        }

        return Task.FromResult<(byte[]? Content, string? ContentType)>((coverImage.Content, contentType));
    }
}
