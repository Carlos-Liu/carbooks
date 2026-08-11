using CarBooks.Domain.Catalog;
using CarBooks.Domain.Repositories;
using CarBooks.Domain.Shared.Errors;

namespace CarBooks.Domain.Tests.Catalog;

public sealed class BookManagerTests
{
    private readonly IBookRepository bookRepository = Substitute.For<IBookRepository>();
    private readonly ICategoryRepository categoryRepository = Substitute.For<ICategoryRepository>();
    private readonly ICategoryBooksRepository categoryBooksRepository = Substitute.For<ICategoryBooksRepository>();
    private readonly ITagRepository tagRepository = Substitute.For<ITagRepository>();
    private readonly IBookTagsRepository bookTagsRepository = Substitute.For<IBookTagsRepository>();
    private readonly BookManager bookManager;

    public BookManagerTests()
    {
        bookManager = new BookManager(
            bookRepository,
            categoryRepository,
            categoryBooksRepository,
            tagRepository,
            bookTagsRepository);
    }

    [Fact]
    public async Task AddBookAsync_NoCategoriesOrTags_PersistsBookOnly()
    {
        // Arrange & Act
        var book = await bookManager.AddBookAsync(
            "Go Like Hell",
            "A. J. Baime",
            coverUrl: null,
            translator: null,
            publisher: null,
            publishedOn: null,
            recommendation: null,
            isbn: null,
            coverImage: null,
            coverImageContentType: null,
            categoryIds: [],
            tagIds: [],
            CancellationToken.None);

        // Assert
        Assert.Equal("Go Like Hell", book.Name);
        await bookRepository.Received(1).AddAsync(book, Arg.Any<CancellationToken>());
        await categoryBooksRepository.DidNotReceive()
            .AddRangeAsync(Arg.Any<IEnumerable<CategoryBooks>>(), Arg.Any<CancellationToken>());
        await bookTagsRepository.DidNotReceive()
            .AddRangeAsync(Arg.Any<IEnumerable<BookTags>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddBookAsync_CoverImageWithoutContentType_ThrowsDomainValidationException()
    {
        // Arrange & Act
        var act = () => bookManager.AddBookAsync(
            "Go Like Hell",
            "A. J. Baime",
            null,
            null,
            null,
            null,
            null,
            null,
            coverImage: [1, 2, 3],
            coverImageContentType: null,
            categoryIds: [],
            tagIds: [],
            CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<DomainValidationException>(act);
    }

    [Fact]
    public async Task AddBookAsync_CoverImageWithContentType_SetsCoverImage()
    {
        // Arrange & Act
        var book = await bookManager.AddBookAsync(
            "Go Like Hell",
            "A. J. Baime",
            null,
            null,
            null,
            null,
            null,
            null,
            coverImage: [1, 2, 3],
            coverImageContentType: "image/png",
            categoryIds: [],
            tagIds: [],
            CancellationToken.None);

        // Assert
        Assert.True(book.HasCoverImage);
        Assert.Equal("image/png", book.CoverImageContentType);
    }

    [Fact]
    public async Task AddBookAsync_DuplicateCategoryIds_CreatesDistinctLinks()
    {
        // Arrange
        var categoryId = Guid.Parse("11111111-1111-4111-8111-111111110001");
        categoryRepository.FindAsync(categoryId, Arg.Any<CancellationToken>())
            .Returns(new Category(categoryId, "Category 1"));

        // Act
        var book = await bookManager.AddBookAsync(
            "Go Like Hell",
            "A. J. Baime",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            categoryIds: [categoryId, categoryId],
            tagIds: [],
            CancellationToken.None);

        // Assert
        await categoryBooksRepository.Received(1).AddRangeAsync(
            Arg.Is<IEnumerable<CategoryBooks>>(links =>
                links.Count() == 1
                && links.Single().CategoryId == categoryId
                && links.Single().BookId == book.Id),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddBookAsync_MissingCategory_ThrowsEntityNotFoundException()
    {
        // Arrange
        var missingCategoryId = Guid.Parse("11111111-1111-4111-8111-111111110099");
        categoryRepository.FindAsync(missingCategoryId, Arg.Any<CancellationToken>()).Returns((Category?)null);

        // Act
        var act = () => bookManager.AddBookAsync(
            "Go Like Hell",
            "A. J. Baime",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            categoryIds: [missingCategoryId],
            tagIds: [],
            CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(act);
    }

    [Fact]
    public async Task AddBookAsync_DuplicateTagIds_CreatesDistinctLinks()
    {
        // Arrange
        var tagId = Guid.Parse("33333333-3333-4333-8333-333333330001");
        tagRepository.FindAsync(tagId, Arg.Any<CancellationToken>())
            .Returns(new Tag(tagId, "Racing"));

        // Act
        var book = await bookManager.AddBookAsync(
            "Go Like Hell",
            "A. J. Baime",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            categoryIds: [],
            tagIds: [tagId, tagId],
            CancellationToken.None);

        // Assert
        await bookTagsRepository.Received(1).AddRangeAsync(
            Arg.Is<IEnumerable<BookTags>>(links =>
                links.Count() == 1
                && links.Single().TagId == tagId
                && links.Single().BookId == book.Id),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddBookAsync_MissingTag_ThrowsEntityNotFoundException()
    {
        // Arrange
        var missingTagId = Guid.Parse("33333333-3333-4333-8333-333333330099");
        tagRepository.FindAsync(missingTagId, Arg.Any<CancellationToken>()).Returns((Tag?)null);

        // Act
        var act = () => bookManager.AddBookAsync(
            "Go Like Hell",
            "A. J. Baime",
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            categoryIds: [],
            tagIds: [missingTagId],
            CancellationToken.None);

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(act);
    }

    [Fact]
    public async Task AddBookAsync_EmptyCoverImageBytes_DoesNotSetCoverImage()
    {
        // Arrange

        // Act
        var book = await bookManager.AddBookAsync(
            "Go Like Hell",
            "A. J. Baime",
            null,
            null,
            null,
            null,
            null,
            null,
            coverImage: [],
            coverImageContentType: null,
            categoryIds: [],
            tagIds: [],
            CancellationToken.None);

        // Assert
        Assert.False(book.HasCoverImage);
    }
}
