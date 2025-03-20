public class AuditLogResponse
{
    public string? type { get; set; }
    public string? group { get; set; }
    public AuditResponseData? data { get; set; }
    public string? messageID { get; set; }
}