using AspNetPolicies.Domain.Interfaces;

namespace AspNetPolicies.Domain.Entities;

public partial class User : IEntity<int>
{
    public User()
    {
        DocumentsOwnered = new HashSet<Document>();
        DocumentsAuthorized = new HashSet<Document>();
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Function { get; set; } = null!;

    public virtual ICollection<Document> DocumentsOwnered { get; set; }

    public virtual ICollection<Document> DocumentsAuthorized { get; set; }
}