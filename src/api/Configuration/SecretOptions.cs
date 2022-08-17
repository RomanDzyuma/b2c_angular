namespace api.Configuration
{
    public class SecretOptions
    {
        public const string Section = "KeyVault";

        public string? Uri { get; set; }

        public string? CertificateName { get; set; }

        public string? TenantId { get; set; }

        public string? ClientId { get; set; }

        public string? SecretValue { get; set; }
    }
}
