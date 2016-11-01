using System.Threading.Tasks;

namespace MvcMovie.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
