using Demo.Core.Persistence;

namespace Demo.Core.Tests.CQRS.Tags;

public class TagComparer : IEqualityComparer<Tag>
{
    public bool Equals(Tag? x, Tag? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Id == y.Id &&
               x.SiteId == y.SiteId &&
               x.Name == y.Name &&
               x.Unit == y.Unit &&
               x.CreatedAt.Equals(y.CreatedAt) &&
               x.UpdatedAt.Equals(y.UpdatedAt);
    }

    public int GetHashCode(Tag obj)
    {
        return HashCode.Combine(obj.Id, obj.SiteId, obj.Name, obj.Unit, obj.CreatedAt, obj.UpdatedAt);
    }
}