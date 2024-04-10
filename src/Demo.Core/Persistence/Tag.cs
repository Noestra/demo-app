using NodaTime;

namespace Demo.Core.Persistence;

public class Tag(string name)
{
    public int Id { get; set; }
    public int SiteId { get; set; }
    public string Name { get; set; } = name;
    public string? Unit { get; set; }
    public string? Description { get; set; }
    public Instant CreatedAt { get; set; }
    public Instant? UpdatedAt { get; set; }

    // navigation properties
    public Site Site = null!;
}