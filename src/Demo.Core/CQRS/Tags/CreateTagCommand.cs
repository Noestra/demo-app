using Demo.Core.Ids;
using Demo.Core.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Demo.Core.CQRS.Tags;

public record CreateTagCommand(SiteId SiteId, string Name, string? Unit, string? Description) : IRequest<Tag>;

public class CreateTagCommandHandler(IDbContextFactory<DemoDbContext> contextFactory, IClock clock)
    : IRequestHandler<CreateTagCommand, Tag>
{
    public async Task<Tag> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var tag = new Tag(request.Name)
        {
            SiteId = request.SiteId.Value,
            Unit = request.Unit,
            Description = request.Description,
            CreatedAt = clock.GetCurrentInstant(),
            UpdatedAt = null,
        };
        await context.Tags.AddAsync(tag, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return tag;
    }
}