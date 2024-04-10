using System.ComponentModel;
using Demo.Core.CQRS.Tags;
using Demo.Core.Ids;
using Demo.Core.Persistence;
using GraphQL;
using GraphQL.Types;

namespace Demo.Mutations;

public sealed record CreateTagInput(
    [property: Description("The ID of the site in which to create the Tag")]
    [Id]
    string SiteId,
    [property: Description("The name of the new tag")]
    string Name,
    [property: Description("The unit of the new tag")]
    string? Unit = null,
    [property: Description("The unit of the new tag")]
    string? Description = null
);

public record CreateTagPayload(
    [property: Description("The newly created tag")]
    Tag Tag
);

public sealed partial class DemoMutation
{
    private void CreateTag()
    {
        Field<AutoRegisteringObjectGraphType<CreateTagPayload>>("createTag")
            .Description("Creates a new tag within given site")
            .Argument<NonNullGraphType<AutoRegisteringInputObjectGraphType<CreateTagInput>>>("input")
            .ResolveAsync(async ctx =>
            {
                var input = ctx.GetArgument<CreateTagInput>("input");
                
                // decode the id
                var id = IdEncoding.Decode<SiteId>(input.SiteId);
                if (!id.HasValue)
                    throw new ExecutionError($"Invalid siteId");
                
                var tag = await SendCommand(ctx,
                    new CreateTagCommand(
                        id.Value,
                        input.Name,
                        input.Description,
                        input.Unit));
                return new CreateTagPayload(tag);
            });
    }
}