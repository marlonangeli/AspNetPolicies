using AspNetPolicies.Domain.Interfaces;

namespace AspNetPolicies.Domain.Entities;

public partial class DocumentRevision : IEntity<int>
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public int RevisionNumber { get; set; }
    public DateTime RevisionDate { get; set; }
    public string Content { get; set; } = null!;

    public virtual Document Document { get; set; } = null!;
}