using Demo.Core.CQRS.Tags;
using Demo.Core.Ids;
using NodaTime;
using NodaTime.Testing;

namespace Demo.Core.Tests.CQRS.Tags;

public class CreateTests(Fixture fixture) : IClassFixture<Fixture>
{
    private static readonly Instant Time = Instant.FromUtc(2024, 4, 2, 13, 49, 0);

    private CreateTagCommandHandler CreateHandler()
    {
        return new CreateTagCommandHandler(new TestDbContextFactory(fixture.Options), new FakeClock(Time));
    }

    [Fact]
    public async Task CreateTest()
    {
        var handler = CreateHandler();

        var siteId = SiteId.From(fixture.Sites[1].Id);
        const string name = "test";
        const string unit = "C";
        const string description = "D";

        var tag = await handler.Handle(new CreateTagCommand(siteId, name, unit, description), default);
        Assert.Equal(siteId.Value, tag.SiteId);
        Assert.Equal(name, tag.Name);
        Assert.Equal(unit, tag.Unit);
        Assert.Equal(description, tag.Description);
        Assert.Equal(Time, tag.CreatedAt);
        Assert.Equal(null, tag.UpdatedAt);
        
        // fetch from DB
        await using var context = new DemoDbContext(fixture.Options);
        var dbTag = await context.Tags.FindAsync(tag.Id);
        Assert.NotNull(dbTag);
        Assert.Equal(siteId.Value, dbTag.SiteId);
        Assert.Equal(name, dbTag.Name);
        Assert.Equal(unit, dbTag.Unit);
        Assert.Equal(description, dbTag.Description);
        Assert.Equal(Time, dbTag.CreatedAt);
        Assert.Equal(null, tag.UpdatedAt);
    }
}