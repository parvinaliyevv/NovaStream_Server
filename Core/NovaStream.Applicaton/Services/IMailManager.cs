namespace NovaStream.Application.Services;

public interface IMailManager
{
    void SendMailTo(string context, string destinationMail);
    Task SendMailToAsync(string context, string destinationMail);
}
