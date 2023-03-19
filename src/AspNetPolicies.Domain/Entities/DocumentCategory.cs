namespace AspNetPolicies.Domain.Entities;

public partial class DocumentCategory
{
    public int DocumentId { get; set; }
    public string Category { get; set; } = null!;

    public virtual Document Document { get; set; } = null!;
}