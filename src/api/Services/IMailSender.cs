namespace api.Services
{
    public interface IMailSender
    {
        Task InviteAsync(
            string toEmail,
            string toName,
            string token);
    }
}
