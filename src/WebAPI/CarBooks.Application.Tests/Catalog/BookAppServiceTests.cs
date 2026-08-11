using CarBooks.Application.Catalog;
using CarBooks.Application.Shared.Catalog.Dtos;
using CarBooks.Domain.Catalog;
using CarBooks.Domain.Repositories;
using CarBooks.Domain.Shared;
using CarBooks.Domain.Shared.Errors;
using CarBooks.Infrastructure.Media;
using Microsoft.AspNetCore.Http;
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
    private readonly IDataUriFactory dataUriFactory = Substitute.For<IDataUriFactory>();
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
        dataUriFactory.Create(Arg.Any<byte[]?>(), Arg.Any<string?>()).Returns((string?)null);
        bookTagsRepository.ListTagsByBookIdsAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new Dictionary<Guid, IReadOnlyList<Tag>>());
        bookAppService = new BookAppService(
            catalogManager,
            bookManager,
            bookTagsRepository,
            dataUriFactory,
            NullLogger<BookAppService>.Instance);
    }

    [Fact]
    public async Task CreateBookAsync_NullRequest_ThrowsArgumentNullException()
    {
        // Arrange & Act
        var act = () => bookAppService.CreateBookAsync(null!, CancellationToken.None);

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
        var result = await bookAppService.CreateBookAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal("Go Like Hell", result.Name);
        Assert.Equal("A. J. Baime", result.Author);
    }

    [Fact]
    public async Task CreateBookAsync_OversizedCoverImage_ThrowsDomainValidationException()
    {
        // Arrange
        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(Consts.MaxCoverImageBytes + 1);
        formFile.ContentType.Returns("image/png");
        var request = new CreateBookDto
        {
            Name = "Go Like Hell",
            Author = "A. J. Baime",
            CoverImage = formFile,
        };

        // Act
        var exception = await Assert.ThrowsAsync<DomainValidationException>(() =>
            bookAppService.CreateBookAsync(request, CancellationToken.None));

        // Assert
        Assert.Contains("bytes or fewer", exception.Message);
    }

    [Fact]
    public async Task CreateBookAsync_UnsupportedCoverContentType_ThrowsDomainValidationException()
    {
        // Arrange
        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(10);
        formFile.ContentType.Returns("application/pdf");
        formFile.OpenReadStream().Returns(new MemoryStream([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]));
        var request = new CreateBookDto
        {
            Name = "Go Like Hell",
            Author = "A. J. Baime",
            CoverImage = formFile,
        };

        // Act
        var act = () => bookAppService.CreateBookAsync(request, CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DomainValidationException>(act);
    }

    [Fact]
    public async Task CreateBookAsync_SupportedCoverImage_ReturnsCoverDataUri()
    {
        // Arrange
        var bytes = new byte[] { 1, 2, 3, 4 };
        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(bytes.Length);
        formFile.ContentType.Returns("image/jpeg");
        formFile.OpenReadStream().Returns(new MemoryStream(bytes));
        dataUriFactory.Create(Arg.Any<byte[]?>(), "image/jpeg").Returns("data:image/jpeg;base64,AQIDBA==");
        var request = new CreateBookDto
        {
            Name = "Go Like Hell",
            Author = "A. J. Baime",
            CoverImage = formFile,
        };

        // Act
        var result = await bookAppService.CreateBookAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal("data:image/jpeg;base64,AQIDBA==", result.CoverImage);
    }

    [Fact]
    public async Task CreateBookAsync_EmptyCoverFile_TreatsCoverAsAbsent()
    {
        // Arrange
        var formFile = Substitute.For<IFormFile>();
        formFile.Length.Returns(0);
        var request = new CreateBookDto
        {
            Name = "Go Like Hell",
            Author = "A. J. Baime",
            CoverImage = formFile,
        };

        // Act
        var result = await bookAppService.CreateBookAsync(request, CancellationToken.None);

        // Assert
        Assert.Null(result.CoverImage);
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
        Assert.Single(result.Books[0].Tags);
        Assert.Equal("Racing", result.Books[0].Tags[0].Name);
        Assert.Equal("Second Book", result.Books[1].Name);
        Assert.Empty(result.Books[1].Tags);
    }
}
