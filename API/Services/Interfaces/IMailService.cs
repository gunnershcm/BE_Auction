namespace API.Services.Interfaces
{
    public interface IMailService
    {
        Task SendUserResetNotification(string fullname, string username, string email, string password);
        Task SendUserAuctionNotification(string fullname, string email);
    }
}
