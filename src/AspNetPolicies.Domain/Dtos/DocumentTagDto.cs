namespace AspNetPolicies.Domain.Dtos;

public class DocumentTagDto
{
    public int DocumentId { get; set; } = default!;
    public string Tag { get; set; } = null!;
}