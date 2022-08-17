namespace api.Configuration
{
    public class MailOptions
    {
        public const string Section = "SendGrid";

        public string Secret { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public Sender? From { get; set; }

        public class Sender
        {
            public string Email { get; set; } = string.Empty;

            public string Name { get; set; } = string.Empty;
        }
    }
}
