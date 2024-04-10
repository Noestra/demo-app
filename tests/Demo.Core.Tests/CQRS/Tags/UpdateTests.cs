using Demo.Core.CQRS.Tags;
using Demo.Core.Ids;

namespace Demo.Core.Tests.CQRS.Tags;

public class UpdateTests(Fixture fixture) : IClassFixture<Fixture>
{
    private UpdateTagCommandHandler CreateHandler()
    {
        return new UpdateTagCommandHandler(new TestDbContextFactory(fixture.Options));
    }

    [Fact]
    public async Task UpdateSuccess()
    {
        var handler = CreateHandler();

        var id = TagId.From(fixture.Tags[2].Id);
        const string newName = "sdfvdsfv";
        const string newUnit = "dfvb";
        const string newDescription = "asdhtt";

        var tag = await handler.Handle(new UpdateTagCommand(id, newName, newUnit, newDescription), default);
        // make sure some fields not changed
        Assert.Equal(fixture.Sites[0].Id, tag.SiteId);
        // make sure others are changed
        Assert.Equal(newName, tag.Name);
        Assert.Equal(newUnit, tag.Unit);
        Assert.Equal(newDescription, tag.Description);
        
        // fetch from DB
        await using var context = new DemoDbContext(fixture.Options);
        var dbTag = await context.Tags.FindAsync(id.Value);
        Assert.NotNull(dbTag);
        // make sure some fields not changed
        Assert.Equal(fixture.Sites[0].Id, dbTag.SiteId);
        // make sure others are changed
        Assert.Equal(newName, dbTag.Name);
        Assert.Equal(newUnit, dbTag.Unit);
        Assert.Equal(newDescription, dbTag.Description);
    }

    [Fact]
    public async Task UpdateFailure()
    {
        var handler = CreateHandler();

        const string newName = "sdfvdsfv";
        const string newDescription = "dfgdf";
        var id = TagId.From(345234);

        await Assert.ThrowsAsync<ArgumentException>(() => 
            handler.Handle(new UpdateTagCommand(id, newName, null, newDescription), default)
        );
    }
}