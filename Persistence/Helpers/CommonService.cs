using Domain.Constants.Enums;

namespace Persistence.Helpers
{
    public static class CommonService
    {
        public static string CreateRandomCode()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            Random random = new Random();
            string randomPassword = new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
            return randomPassword;
        }
    }
}
