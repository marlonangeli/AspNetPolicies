using AspNetPolicies.Domain.Interfaces;

namespace AspNetPolicies.Domain.Entities
{
    public partial class Document : IEntity<int>
    {
        public Document()
        {
            DocumentCategories = new HashSet<DocumentCategory>();
            DocumentRevisions = new HashSet<DocumentRevision>();
            DocumentTags = new HashSet<DocumentTag>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int OwnerUserId { get; set; }

        public virtual User OwnerUser { get; set; } = null!;
        public virtual ICollection<DocumentCategory> DocumentCategories { get; set; }
        public virtual ICollection<DocumentRevision> DocumentRevisions { get; set; }
        public virtual ICollection<DocumentTag> DocumentTags { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
