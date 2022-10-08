using System.Threading.Tasks;

namespace Insurance.Utility
{
    public interface IEmailService
    {
        Task SendOTPEmail(EmailOptions opt, SMTPSetting set);
    }
}