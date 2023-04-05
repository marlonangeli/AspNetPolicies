namespace AspNetPolicies.Security.Models;

public class Actions
{
    public bool Read { get; set; } = false;
    public bool Write { get; set; } = false;
    public bool Delete { get; set; } = false;
    public string? Custom { get; set; } = null;
}