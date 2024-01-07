using ErrorOr;

namespace Quizer.Application.Services.Email
{
    public interface IEmailService
    {
        Task<ErrorOr<bool>> Send(string to, string subject, string body);
    }
}