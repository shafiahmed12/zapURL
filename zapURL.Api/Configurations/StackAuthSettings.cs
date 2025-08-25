namespace zapURL.Api.Configurations;

public class StackAuthSettings
{
    public const string SectionName = "StackAuthSettings";
    public const string AccessTypeHeader = "X-Stack-Access-Type";
    public const string ProjectIdHeader = "X-Stack-Project-Id";
    public const string SecretServerKeyHeader = "X-Stack-Secret-Server-Key";
    public const string BaseAddress = "https://api.stack-auth.com";
    public const string ValidIssuer = "https://access-token.jwt-signature.stack-auth.com";

    public string AccessType { get; init; } = null!;
    public string SecretServerKey { get; init; } = null!;
    public string ProjectId { get; init; } = null!;
    public string Authority => $"{BaseAddress}/api/v1/projects/{ProjectId}";
}