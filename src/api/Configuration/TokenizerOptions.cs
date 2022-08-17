namespace api.Configuration
{
    public class TokenizerOptions
    {
        public const string Section = "Token";

        public string ClientId { get; set; }

        public string Instance { get; set; }

        public string Domain { get; set; }

        public string Policy { get; set; }

        public string RedirectUri { get; set; }

        public string SignUpUrl { get; set; }
    }
}
