using System.Threading.Tasks;

namespace MvcMovie.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
