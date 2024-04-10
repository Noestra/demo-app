using Demo.Core.Ids;
using Demo.Core.Persistence;
using GraphQL.Types;

namespace Demo.Types;

public sealed class TagType : ObjectGraphType<Tag>
{
    public TagType()
    {
        Name = "Tag";
        Description = "An industrial tag (sensor)";

        Interface<NodeInterface>();

        Field<NonNullGraphType<IdGraphType>>("id")
            .Resolve(x => IdEncoding.Encode(TagId.From(x.Source.Id)));
        Field(x => x.Name)
            .Description("The name of the tag");
        Field(x => x.Unit, nullable: true)
            .Description("The unit for the sensor data");
        Field(x => x.Description, nullable: true)
            .Description("The description of the tag");
        Field<NonNullGraphType<DateTimeGraphType>>("createdAt")
            .Description("When the tag was added")
            .Resolve(ctx => ctx.Source.CreatedAt.ToDateTimeUtc());
    }
}