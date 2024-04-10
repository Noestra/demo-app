using System.ComponentModel;
using Demo.Core.CQRS.Tags;
using Demo.Core.Ids;
using Demo.Core.Persistence;
using GraphQL;
using GraphQL.Types;

namespace Demo.Mutations;

public sealed record UpdateTagInput(
    [property: Description("The ID of the tag to update")]
    [Id]
    string TagId,
    [property: Description("The new name")]
    string Name,
    [property: Description("The new unit")]
    string? Unit = null,
    [property: Description("The new description")]
    string? Description = null
);

public record UpdateTagPayload(
    [property: Description("The updated tag")]
    Tag Tag
);

public sealed partial class DemoMutation
{
    private void UpdateTag()
    {
        Field<AutoRegisteringObjectGraphType<UpdateTagPayload>>("updateTag")
            .Description("Update a tag")
            .Argument<NonNullGraphType<AutoRegisteringInputObjectGraphType<UpdateTagInput>>>("input")
            .ResolveAsync(async ctx =>
            {
                var input = ctx.GetArgument<UpdateTagInput>("input");

                // decode the id
                var id = IdEncoding.Decode<TagId>(input.TagId);
                if (!id.HasValue)
                    throw new ExecutionError($"Invalid tagId");

                var tag = await SendCommand(ctx,
                    new UpdateTagCommand(
                        id.Value,
                        input.Name,
                        input.Description,
                        input.Unit));

                return new UpdateTagPayload(tag);
            });
    }
}