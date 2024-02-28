using API.Services.Interfaces;
using Domain.Mails;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace API.Services.Implements
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendUserCreatedNotification(string fullname, string username, string email, string password)
        {
            using (MimeMessage emailMessage = new MimeMessage())
            {
                MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(fullname,
                    email);
                emailMessage.To.Add(emailTo);

                emailMessage.Subject = "Welcome to Auction Web";
                string emailTemplateText = @"<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Welcome to Auction Web!</title>
</head>
<body style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; margin: 0; padding: 0; background-color: #f8f9fa;"">

  <div style=""max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);"">
    <div style=""background-color: #007bff; color: #fff; padding: 20px; text-align: center; border-top-left-radius: 5px; border-top-right-radius: 5px;"">
      <h1>Welcome to Auction Web!</h1>
    </div>
    <div style=""padding: 20px;"">
      <p>Dear {0},</p>
      <p>We are delighted to welcome you to Auction Web! Your new account has been successfully created, and we're excited to have you on board. Below are your account details:</p>
        <ul style=""list-style-type: none; padding: 0;"">
        <li style=""margin-bottom: 10px;""><strong>Username:</strong> {1}</li>
        <li style=""margin-bottom: 10px;""><strong>Password:</strong> {2}</li>
        <li style=""margin-bottom: 10px;""><strong>Email:</strong> {3}</li>
      </ul>
      <p>Please keep this information secure, and do not share your password with anyone. If you have any questions or concerns regarding your account, feel free to contact our support team at <a href=""mailto:auctionweb789@gmail.com"">auctionweb789@gmail.com</a>.</p>
    </div>
    <div style=""background-color: #f1f1f1; padding: 10px; text-align: center; border-bottom-left-radius: 5px; border-bottom-right-radius: 5px;"">
      <p>Best regards, Auction Web</p>
    </div>
  </div>

</body>
</html>
";
                emailTemplateText = string.Format(emailTemplateText, fullname, username, password, email);
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = emailTemplateText;
                emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";

                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                using (SmtpClient mailClient = new SmtpClient())
                {
                    try
                    {
                        await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(_mailSettings.SenderEmail, _mailSettings.Password);
                        await mailClient.SendAsync(emailMessage);
                        await mailClient.DisconnectAsync(true);
                    }
                    catch (Exception ex)
                    {
                        // Log exception
                        Console.WriteLine($"Error sending email: {ex.Message}");
                        throw; // Re-throw the exception to let the caller know an error occurred
                    }
                }
            }
        }

    }
}
