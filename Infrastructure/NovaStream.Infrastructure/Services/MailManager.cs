namespace NovaStream.Infrastructure.Services;

public class MailManager : IMailManager
{
    private readonly SenderMailOptions _mailOptions;


    public MailManager(IOptions<SenderMailOptions> mailOptions)
    {
        _mailOptions = mailOptions.Value;
    }


    public void SendMailTo(string context, string destinationMail)
    {
        var mail = _mailOptions.Mail;
        var password = _mailOptions.AccessPassword;

        MailMessage message = new();
        message.From = new MailAddress(mail);
        message.To.Add(new MailAddress(destinationMail));
        message.Subject = "Email Verification";
        message.Body = $"<html><body> Your Email Confirmation PIN Code is: {context}</body></html>";
        message.IsBodyHtml = true;

        var host = "smtp.gmail.com";
        var port = 587;

        var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(mail, password),
            EnableSsl = true
        };

        try { client.Send(message); }
        catch (Exception ex) { Console.WriteLine(ex); }
    }
    public Task SendMailToAsync(string context, string destinationMail)
        => Task.Factory.StartNew(() => SendMailTo(context, destinationMail));

    public static string CreateConfirmationPIN(int length)
    {
        Random random = new();
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
