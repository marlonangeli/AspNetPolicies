namespace AspNetPolicies.Domain.Entities;

public partial class DocumentTag
{
    public int DocumentId { get; set; }
    public string Tag { get; set; } = null!;

    public virtual Document Document { get; set; } = null!;
}