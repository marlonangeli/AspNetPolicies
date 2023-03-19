namespace AspNetPolicies.Domain.Dtos;

public class DocumentRevisionDto
{
    public int DocumentId { get; set; } = default!;
    public int RevisionNumber { get; set; } = default!;
    public DateTime RevisionDate { get; set; } = default!;
    public string Content { get; set; } = null!;
}