using Demo.Core.Ids;
using Demo.Core.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Demo.Core.CQRS.Tags;

public record UpdateTagCommand(TagId Id, string Name, string? Unit, string? Description) : IRequest<Tag>;

public class UpdateTagCommandHandler(IDbContextFactory<DemoDbContext> contextFactory, IClock clock)
    : IRequestHandler<UpdateTagCommand, Tag>
{
    public async Task<Tag> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var tag = await context.Tags.FindAsync([request.Id.Value], cancellationToken: cancellationToken);
        if (tag is null)
            throw new ArgumentException("not found");

        tag.Name = request.Name;
        tag.Unit = request.Unit;
        tag.Description = request.Description;
        tag.UpdatedAt = clock.GetCurrentInstant();

        await context.SaveChangesAsync(cancellationToken);

        return tag;
    }
}