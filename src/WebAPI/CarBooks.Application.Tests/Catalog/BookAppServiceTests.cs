using CarBooks.Application.Catalog;
using CarBooks.Application.Shared.Catalog.Dtos;
using CarBooks.Domain.Catalog;
using CarBooks.Domain.Repositories;
using CarBooks.Domain.Shared;
using CarBooks.Domain.Shared.Errors;
using CarBooks.Domain.Shared.Media;
using CarBooks.Infrastructure.Media;
using Microsoft.Extensions.Logging.Abstractions;

namespace CarBooks.Application.Tests.Catalog;

public sealed class BookAppServiceTests
{
    private readonly IBookRepository bookRepository = Substitute.For<IBookRepository>();
    private readonly ICategoryRepository categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly ICategoryBooksRepository categoryBooksRepository = Substitute.For<ICategoryBooksRepository>();
    private readonly ITagRepository tagRepository = Substitute.For<ITagRepository>();
    private readonly IBookTagsRepository bookTagsRepository = Substitute.For<IBookTagsRepository>();
    private readonly IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICoverThumbnailGenerator coverThumbnailGenerator = Substitute.For<ICoverThumbnailGenerator>();
    private readonly BookAppService bookAppService;

    public BookAppServiceTests()
    {
        var bookManager = new BookManager(
            bookRepository,
            categoryRepository,
            categoryBooksRepository,
            tagRepository,
            bookTagsRepository,
            unitOfWork);
        var catalogManager = new CatalogManager(categoryRepository, bookRepository);
        coverThumbnailGenerator.Generate(Arg.Any<byte[]>(), Arg.Any<string>())
            .Returns(call => new ImageContent(call.ArgAt<byte[]>(0), call.ArgAt<string>(1)));
        bookTagsRepository.ListTagsByBookIdsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new Dictionary<Guid, IReadOnlyList<Tag>>());
        bookAppService = new BookAppService(
            catalogManager,
            bookManager,
            bookRepository,
            bookTagsRepository,
            coverThumbnailGenerator,
            NullLogger<BookAppService>.Instance);
    }

    [Fact]
    public async Task CreateBookAsync_NullRequest_ThrowsArgumentNullException()
    {
        // Arrange & Act
        var act = () => bookAppService.CreateBookAsync(null!, null, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }

    [Fact]
    public async Task CreateBookAsync_WithoutCover_ReturnsCreatedBook()
    {
        // Arrange
        var request = new CreateBookDto
        {
            Name = "Go Like Hell",
            Author = "A. J. Baime",
        };

        // Act
        var result = await bookAppService.CreateBookAsync(request, null, CancellationToken.None);

        // Assert
        Assert.Equal("Go Like Hell", result.Name);
        Assert.Equal("A. J. Baime", result.Author);
    }

    [Fact]
    public async Task CreateBookAsync_OversizedCoverImage_ThrowsDomainValidationException()
    {
        // Arrange
        var request = new CreateBookDto
        {
            Name = "Go Like Hell",
            Author = "A. J. Baime",
        };
        var coverImage = new CoverImageDto
        {
            Content = new byte[Consts.MaxCoverImageBytes + 1],
            ContentType = "image/png",
        };

        // Act
        var exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
            bookAppService.CreateBookAsync(request, coverImage, CancellationToken.None));

        // Assert
        Assert.Contains("bytes or fewer", exception.Message);
    }

    [Fact]
    public async Task CreateBookAsync_UnsupportedCoverContentType_ThrowsDomainValidationException()
    {
        // Arrange
        var request = new CreateBookDto
        {
            Name = "Go Like Hell",
            Author = "A. J. Baime",
        };
        var coverImage = new CoverImageDto
        {
            Content = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
            ContentType = "application/pdf",
        };

        // Act
        var act = () => bookAppService.CreateBookAsync(request, coverImage, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DomainValidationException>(act);
    }

    [Fact]
    public async Task CreateBookAsync_SvgCoverContentType_ThrowsDomainValidationException()
    {
        // Arrange
        var request = new CreateBookDto
        {
            Name = "Go Like Hell",
            Author = "A. J. Baime",
        };
        var coverImage = new CoverImageDto
        {
            Content = [1, 2, 3, 4],
            ContentType = "image/svg+xml",
        };

        // Act
        var act = () => bookAppService.CreateBookAsync(request, coverImage, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DomainValidationException>(act);
    }

    [Fact]
    public async Task CreateBookAsync_SupportedCoverImage_ReturnsCoverThumbnailUrl()
    {
        // Arrange
        var bytes = new byte[] { 1, 2, 3, 4 };
        coverThumbnailGenerator.Generate(bytes, "image/jpeg")
            .Returns(new ImageContent([9, 8], "image/jpeg"));
        var request = new CreateBookDto
        {
            Name = "Go Like Hell",
            Author = "A. J. Baime",
        };
        var coverImage = new CoverImageDto
        {
            Content = bytes,
            ContentType = "image/jpeg",
        };

        // Act
        var result = await bookAppService.CreateBookAsync(request, coverImage, CancellationToken.None);

        // Assert
        Assert.Equal($"/api/books/{result.Id}/cover/thumbnail", result.CoverThumbnailUrl);
    }

    [Fact]
    public async Task CreateBookAsync_CoverUrlAndCoverImage_PreservesBoth()
    {
        // Arrange
        var bytes = new byte[] { 1, 2, 3, 4 };
        coverThumbnailGenerator.Generate(bytes, "image/jpeg")
            .Returns(new ImageContent([9, 8], "image/jpeg"));
        var request = new CreateBookDto
        {
            Name = "Go Like Hell",
            Author = "A. J. Baime",
            CoverUrl = "https://example.com/covers/go-like-hell.jpg",
        };
        var coverImage = new CoverImageDto
        {
            Content = bytes,
            ContentType = "image/jpeg",
        };

        // Act
        var result = await bookAppService.CreateBookAsync(request, coverImage, CancellationToken.None);

        // Assert
        Assert.Equal("https://example.com/covers/go-like-hell.jpg", result.CoverUrl);
        Assert.Equal($"/api/books/{result.Id}/cover/thumbnail", result.CoverThumbnailUrl);
    }

    [Fact]
    public async Task CreateBookAsync_EmptyCoverFile_TreatsCoverAsAbsent()
    {
        // Arrange
        var request = new CreateBookDto
        {
            Name = "Go Like Hell",
            Author = "A. J. Baime",
        };
        var coverImage = new CoverImageDto
        {
            Content = [],
        };

        // Act
        var result = await bookAppService.CreateBookAsync(request, coverImage, CancellationToken.None);

        // Assert
        Assert.Null(result.CoverThumbnailUrl);
    }

    [Fact]
    public async Task GetBooksByCategoryIdAsync_ExistingCategory_ReturnsMappedCategoryAndBooks()
    {
        // Arrange
        var categoryId = Guid.Parse("11111111-1111-4111-8111-111111110001");
        var bookId = Guid.Parse("22222222-2222-4222-8222-222222220001");
        var category = new Category(categoryId, "Category 1");
        var books = new List<Book>
        {
            new(bookId, "First Book", "A. J. Baime"),
            new(Guid.Parse("22222222-2222-4222-8222-222222220002"), "Second Book", "John Smith"),
        };
        books[0].SetCoverThumbnail([1, 2, 3], "image/jpeg");
        var racingTag = new Tag(Guid.Parse("33333333-3333-4333-8333-333333330001"), "Racing");
        categoryRepository.FindAsync(categoryId, Arg.Any<CancellationToken>()).Returns(category);
        bookRepository.ListByCategoryAsync(categoryId, Arg.Any<CancellationToken>()).Returns(books);
        bookTagsRepository.ListTagsByBookIdsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new Dictionary<Guid, IReadOnlyList<Tag>>
            {
                [bookId] = [racingTag],
            });

        // Act
        var result = await bookAppService.GetBooksByCategoryIdAsync(categoryId, CancellationToken.None);

        // Assert
        Assert.Equal(categoryId, result.Category.Id);
        Assert.Equal(2, result.Category.BookCount);
        Assert.Equal(2, result.Books.Count);
        Assert.Equal("First Book", result.Books[0].Name);
        Assert.Equal($"/api/books/{bookId}/cover/thumbnail", result.Books[0].CoverThumbnailUrl);
        Assert.Single(result.Books[0].Tags);
        Assert.Equal("Racing", result.Books[0].Tags[0].Name);
        Assert.Equal("Second Book", result.Books[1].Name);
        Assert.Null(result.Books[1].CoverThumbnailUrl);
        Assert.Empty(result.Books[1].Tags);
    }

    [Fact]
    public async Task GetCoverThumbnailAsync_ExistingThumbnail_ReturnsThumbnailPayload()
    {
        // Arrange
        var bookId = Guid.Parse("22222222-2222-4222-8222-222222220001");
        bookRepository.FindCoverThumbnailAsync(bookId, Arg.Any<CancellationToken>())
            .Returns(new ImageContent([1, 2, 3], "image/jpeg"));

        // Act
        var result = await bookAppService.GetCoverThumbnailAsync(bookId, CancellationToken.None);

        // Assert
        Assert.Equal([1, 2, 3], result.Content);
        Assert.Equal("image/jpeg", result.ContentType);
    }

    [Fact]
    public async Task GetCoverThumbnailAsync_MissingThumbnail_ThrowsEntityNotFoundException()
    {
        // Arrange
        var bookId = Guid.Parse("22222222-2222-4222-8222-222222220001");
        bookRepository.FindCoverThumbnailAsync(bookId, Arg.Any<CancellationToken>())
            .Returns((ImageContent?)null);

        // Act
        var act = () => bookAppService.GetCoverThumbnailAsync(bookId, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(act);
    }
}
