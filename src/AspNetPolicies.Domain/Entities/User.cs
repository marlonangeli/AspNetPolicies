using AspNetPolicies.Domain.Interfaces;

namespace AspNetPolicies.Domain.Entities
{
    public partial class User : IEntity<int>
    {
        public User()
        {
            DocumentsNavigation = new HashSet<Document>();
            Documents = new HashSet<Document>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Function { get; set; } = null!;

        public virtual ICollection<Document> DocumentsNavigation { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
