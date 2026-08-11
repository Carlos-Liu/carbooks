using CarBooks.Application.Catalog.Mapping;
using CarBooks.Application.Shared.Catalog;
using CarBooks.Application.Shared.Catalog.Dtos;
using CarBooks.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CarBooks.Application.Catalog;

internal sealed class TagAppService : ITagAppService
{
    private readonly ITagRepository tagRepository;
    private readonly ILogger<TagAppService> logger;

    public TagAppService(ITagRepository tagRepository, ILogger<TagAppService> logger)
    {
        this.tagRepository = tagRepository;
        this.logger = logger;
    }

    public async Task<IReadOnlyList<TagDto>> GetTagsAsync(CancellationToken cancellationToken)
    {
        var tags = await tagRepository.ListAsync(cancellationToken);

        logger.LogInformation("Returning {TagCount} catalog tags.", tags.Count);

        return tags.Select(tag => tag.ToDto()).ToList();
    }
}
