namespace AspNetPolicies.Domain.Dtos;

public class DocumentDto
{
    public string Name { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int OwnerUserId { get; set; } = default!;
}