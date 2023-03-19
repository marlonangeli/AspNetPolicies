using AspNetPolicies.Domain.Interfaces;

namespace AspNetPolicies.Domain.Entities
{
    public partial class DocumentTag : IEntity<int>
    {
        public int DocumentId { get; set; }
        public string Tag { get; set; } = null!;

        public virtual Document Document { get; set; } = null!;
        public int Id { get; set; }
    }
}
