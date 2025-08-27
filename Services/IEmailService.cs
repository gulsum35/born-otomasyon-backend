using System.Threading.Tasks;

namespace BornOtomasyonApi.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
