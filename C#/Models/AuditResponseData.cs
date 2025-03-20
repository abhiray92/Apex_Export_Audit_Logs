public class AuditResponseData
{
    public string? field { get; set; }
    public List<AuditLog>? value { get; set; }  // This should be List<AuditLog>
}