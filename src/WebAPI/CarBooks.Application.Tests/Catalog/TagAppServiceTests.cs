using CarBooks.Application.Catalog;
using CarBooks.Domain.Catalog;
using CarBooks.Domain.Repositories;
using Microsoft.Extensions.Logging.Abstractions;

namespace CarBooks.Application.Tests.Catalog;

public sealed class TagAppServiceTests
{
    private readonly ITagRepository tagRepository = Substitute.For<ITagRepository>();
    private readonly TagAppService tagAppService;

    public TagAppServiceTests()
    {
        tagAppService = new TagAppService(tagRepository, NullLogger<TagAppService>.Instance);
    }

    [Fact]
    public async Task GetTagsAsync_TagsExist_ReturnsMappedTags()
    {
        // Arrange
        tagRepository.ListAsync(Arg.Any<CancellationToken>()).Returns(
        [
            new Tag(Guid.Parse("33333333-3333-4333-8333-333333330001"), "Racing"),
            new Tag(Guid.Parse("33333333-3333-4333-8333-333333330002"), "History"),
        ]);

        // Act
        var result = await tagAppService.GetTagsAsync(CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Racing", result[0].Name);
        Assert.Equal("History", result[1].Name);
    }

    [Fact]
    public async Task GetTagsAsync_EmptyCatalog_ReturnsEmptyList()
    {
        // Arrange
        tagRepository.ListAsync(Arg.Any<CancellationToken>()).Returns([]);

        // Act
        var result = await tagAppService.GetTagsAsync(CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
