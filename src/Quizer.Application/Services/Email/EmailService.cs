using ErrorOr;
using FluentEmail.Core;

namespace Quizer.Application.Services.Email;

public class EmailService : IEmailService
{
    private IFluentEmail _fluentEmail;

    public EmailService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    public async Task<ErrorOr<bool>> Send(string to, string subject, string body)
    {
        var response = await _fluentEmail
            .To(to)
            .Subject(subject)
            .Body(body)
            .SendAsync();

        if (!response.Successful)
            return Error.Failure(
                code: "Email.Send.Failed",
                description: "Failed to send email");

        return true;
    }
}