namespace AspNetPolicies.Security.Models;

public abstract class Permission
{ 
    public string Name { get; set; }
    public string Description { get; set; }
    public Actions? Actions { get; set; } = new();
    
    public abstract void SetActions();
}
