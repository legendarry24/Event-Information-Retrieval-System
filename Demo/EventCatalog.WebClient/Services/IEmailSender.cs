using System.Threading.Tasks;

namespace EventCatalog.WebClient.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
